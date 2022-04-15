using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using BinarySerialization;
using log4net;
using UkTote.Message;

namespace UkTote
{
    public partial class ToteGateway : CancellableQueueWorker<MessageBase>, IDisposable, IToteGateway
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(ToteGateway));

        private const string DateFormat = "ddMMyyyy";
        private const int BufferSize = 65536;

        public event Action<AccountLoginError> OnLoginError;
        public event Action<AccountLoginSuccess> OnLoginSuccess;
        public event Action<AccountLogoutError> OnLogoutError;
        public event Action<AccountLogoutSuccess> OnLogoutSuccess;
        public event Action<MeetingEndDateErrorReply> OnMeetingEndDateError;
        public event Action<MeetingEndDateReply> OnMeetingEndDate;
        public event Action<MeetingReply> OnMeeting;
        public event Action<RacecardReply> OnRacecard;
        public event Action<RacePoolReply> OnRacePool;
        public event Action<RaceReply> OnRace;
        public event Action<RunnerReply> OnRunner;
        public event Action<MeetingPoolReply> OnMeetingPool;
        public event Action<SellBetFailed> OnSellBetFailed;
        public event Action<SellBetSuccess> OnSellBetSuccess;
        public event Action<PayEnquirySuccess> OnPayEnquirySuccess;
        public event Action<PayEnquiryFailed> OnPayEnquiryFailed;
        public event Action<MsnReply> OnMsnReply;
        public event Action<CurrentMsnReply> OnCurrentMsnReply;
        public event Action<CurrentBalanceReply> OnCurrentBalanceReply;
        public event Action<int, int> OnBatchProgress;
        public event Action OnConnected;
        public event Action<string> OnDisconnected;
        public event Action<string> OnIdle;
        public event Action OnRuOk;
        public event Action<byte[]> OnRawPacketReceived;
        public event Action<byte[]> OnRawPacketSent;

        // update events
        public event Action<MeetingUpdate> OnMeetingUpdate;
        public event Action<MeetingSalesUpdate> OnMeetingSalesUpdate;
        public event Action<RaceUpdate> OnRaceUpdate;
        public event Action<RacePoolUpdate> OnRacePoolUpdate;
        public event Action<MeetingPoolUpdate> OnMeetingPoolUpdate;
        public event Action<PoolSubstituteUpdate> OnPoolSubstituteUpdate;
        public event Action<RaceSalesUpdate> OnRaceSalesUpdate;
        public event Action<MeetingPoolSalesUpdate> OnMeetingPoolSalesUpdate;
        public event Action<ResultUpdate> OnResultUpdate;
        public event Action<SubstituteUpdate> OnSubstituteUpdate;
        public event Action<WeighedInUpdate> OnWeighedInUpdate;
        public event Action<EndOfRacingUpdate> OnEndOfRacingUpdate;
        public event Action<RacePoolSalesUpdate> OnRacePoolSalesUpdate;
        public event Action<RacePoolDividendUpdate> OnRacePoolDividendUpdate;
        public event Action<MeetingPoolDividendUpdate> OnMeetingPoolDividendUpdate;
        public event Action<SuperComplexPoolDividendUpdate> OnSuperComplexPoolDividendUpdate;
        public event Action<MatrixPoolDividendUpdate> OnMatrixPoolDividendUpdate;
        public event Action<ComplexRacePoolDividendUpdate> OnComplexRacePoolDividendUpdate;
        public event Action<RunnerUpdate> OnRunnerUpdate;

        // pay update events
        public event Action<MeetingPayUpdate> OnMeetingPayUpdate;
        public event Action<RacePayUpdate> OnRacePayUpdate;
        public event Action<MeetingPoolPayUpdate> OnMeetingPoolPayUpdate;
        public event Action<RacePoolPayUpdate> OnRacePoolPayUpdate;

        // will pay update events
        public event Action<MeetingPoolWillPayUpdate> OnMeetingPoolWillPayUpdate;
        public event Action<RaceWillPayUpdate> OnRaceWillPayUpdate;
        public event Action<RaceExtendedWillPayUpdate> OnRaceExtendedWillPayUpdate;
        public event Action<LegBreakdownUpdate> OnLegBreakdownUpdate;
        public event Action<MeetingPoolTotalUpdate> OnMeetingPoolTotalUpdate;
        public event Action<ComplexRacePoolTotalUpdate> OnComplexRacePoolTotalUpdate;

        private bool _shuttingDown = false;
        private int _nextBetId = 0; // TODO - this gets set to 0 at start of day and persisted
        private TcpClient _tcpClient = new TcpClient();
        private BinarySerializer _serializer;
        private CircularBuffer<byte> _circularBuffer;
        private byte[] _buffer = new byte[BufferSize];
        private Dictionary<Tuple<Enums.MessageType, Enums.ActionCode>, Type> _lookup = new Dictionary<Tuple<Enums.MessageType, Enums.ActionCode>, Type>();
        private Dictionary<Enums.MessageType, bool> _ignoreUpdates = new Dictionary<Enums.MessageType, bool>();
        private WatchdogTimer _watchdogTimer;
        private readonly int _watchdogTimeoutMs;
        private ConcurrentDictionary<int, Guid> _refLookup = new ConcurrentDictionary<int, Guid>();

        public int NextBetId
        {
            set => _nextBetId = value;
        }

        public bool IsConnected => _tcpClient != null && _tcpClient.Connected;

        public ToteGateway(int watchdogTimeoutMs)
            : base(0, 1)
        {
            _watchdogTimeoutMs = watchdogTimeoutMs;
            _serializer = new BinarySerializer
            {
                Endianness = Endianness.Big
            };
            _circularBuffer = new CircularBuffer<byte>(BufferSize * 4);

            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.AccountLogin, Enums.ActionCode.ActionSuccess)] = typeof(AccountLoginSuccess);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.AccountLogin, Enums.ActionCode.ActionFail)] = typeof(AccountLoginError);

            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.AccountLogout, Enums.ActionCode.ActionLogout)] = typeof(AccountLogoutSuccess);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.AccountLogout, Enums.ActionCode.ActionFail)] = typeof(AccountLogoutError);

            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.RacecardReqMsg, Enums.ActionCode.ActionUnknown)] = typeof(RacecardReply);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.RacecardReqMsg, Enums.ActionCode.ActionFail)] = typeof(RacecardReply);

            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.MeetingReqMsg, Enums.ActionCode.ActionUnknown)] = typeof(MeetingReply);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.MeetingReqMsg, Enums.ActionCode.ActionFail)] = typeof(MeetingReply);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.MeetingReqMsg, Enums.ActionCode.ActionOn)] = typeof(MeetingReply);

            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.RaceReqMsg, Enums.ActionCode.ActionUnknown)] = typeof(RaceReply);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.RaceReqMsg, Enums.ActionCode.ActionFail)] = typeof(RaceReply);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.RaceReqMsg, Enums.ActionCode.ActionOn)] = typeof(RaceReply);

            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.RunnerReqMsg, Enums.ActionCode.ActionUnknown)] = typeof(RunnerReply);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.RunnerReqMsg, Enums.ActionCode.ActionFail)] = typeof(RunnerReply);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.RunnerReqMsg, Enums.ActionCode.ActionRunning)] = typeof(RunnerReply);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.RunnerReqMsg, Enums.ActionCode.ActionNonRunner)] = typeof(RunnerReply);

            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.PoolReqMsg, Enums.ActionCode.ActionUnknown)] = typeof(RacePoolReply);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.PoolReqMsg, Enums.ActionCode.ActionFail)] = typeof(RacePoolReply);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.PoolReqMsg, Enums.ActionCode.ActionNormal)] = typeof(RacePoolReply);

            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.MeetingPoolReqMsg, Enums.ActionCode.ActionUnknown)] = typeof(MeetingPoolReply);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.MeetingPoolReqMsg, Enums.ActionCode.ActionFail)] = typeof(MeetingPoolReply);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.MeetingPoolReqMsg, Enums.ActionCode.ActionNormal)] = typeof(MeetingPoolReply);

            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.SellBetReqMsg, Enums.ActionCode.ActionSuccess)] = typeof(SellBetSuccess);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.SellBetReqMsg, Enums.ActionCode.ActionFail)] = typeof(SellBetFailed);

            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.PayBetReqMsg, Enums.ActionCode.ActionSuccess)] = typeof(PayEnquirySuccess);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.PayBetReqMsg, Enums.ActionCode.ActionFail)] = typeof(PayEnquiryFailed);

            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.MsnReqMsg, Enums.ActionCode.ActionSuccess)] = typeof(MsnReply);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.MsnReqMsg, Enums.ActionCode.ActionFail)] = typeof(MsnReply);

            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.CurrentMsnReqMsg, Enums.ActionCode.ActionSuccess)] = typeof(CurrentMsnReply);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.CurrentMsnReqMsg, Enums.ActionCode.ActionFail)] = typeof(CurrentMsnReply);

            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.CurrentBalanceReqMsg, Enums.ActionCode.ActionUnknown)] = typeof(CurrentBalanceReply);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.CurrentBalanceReqMsg, Enums.ActionCode.ActionFail)] = typeof(CurrentBalanceReply);

            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.MeetingUpdateMsg, Enums.ActionCode.ActionOn)] = typeof(MeetingUpdate);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.MeetingUpdateMsg, Enums.ActionCode.ActionCancelled)] = typeof(MeetingUpdate);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.MeetingUpdateMsg, Enums.ActionCode.ActionPostponed)] = typeof(MeetingUpdate);

            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.RaceUpdateMsg, Enums.ActionCode.ActionOn)] = typeof(RaceUpdate);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.RaceUpdateMsg, Enums.ActionCode.ActionOff)] = typeof(RaceUpdate);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.RaceUpdateMsg, Enums.ActionCode.ActionClosed)] = typeof(RaceUpdate);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.RaceUpdateMsg, Enums.ActionCode.ActionCancelled)] = typeof(RaceUpdate);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.RaceUpdateMsg, Enums.ActionCode.ActionPostponed)] = typeof(RaceUpdate);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.RaceUpdateMsg, Enums.ActionCode.ActionVoid)] = typeof(RaceUpdate);

            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.RunnerUpdateMsg, Enums.ActionCode.ActionRunning)] = typeof(RunnerUpdate);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.RunnerUpdateMsg, Enums.ActionCode.ActionNonRunner)] = typeof(RunnerUpdate);

            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.MeetingPoolUpdate, Enums.ActionCode.ActionOn)] = typeof(MeetingPoolUpdate);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.MeetingPoolUpdate, Enums.ActionCode.ActionCancelled)] = typeof(MeetingPoolUpdate);

            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.RacePoolUpdateMsg, Enums.ActionCode.ActionOn)] = typeof(RacePoolUpdate);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.RacePoolUpdateMsg, Enums.ActionCode.ActionCancelled)] = typeof(RacePoolUpdate);

            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.PoolSubstituteUpdate, Enums.ActionCode.ActionOn)] = typeof(PoolSubstituteUpdate);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.PoolSubstituteUpdate, Enums.ActionCode.ActionOff)] = typeof(PoolSubstituteUpdate);

            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.MeetingSalesUpdate, Enums.ActionCode.ActionSalesOpen)] = typeof(MeetingSalesUpdate);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.MeetingSalesUpdate, Enums.ActionCode.ActionSalesClosed)] = typeof(MeetingSalesUpdate);

            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.RaceSalesUpdate, Enums.ActionCode.ActionSalesOpen)] = typeof(RaceSalesUpdate);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.RaceSalesUpdate, Enums.ActionCode.ActionSalesClosed)] = typeof(RaceSalesUpdate);

            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.MeetingPoolSalesUpdate, Enums.ActionCode.ActionSalesOpen)] = typeof(MeetingPoolSalesUpdate);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.MeetingPoolSalesUpdate, Enums.ActionCode.ActionSalesClosed)] = typeof(MeetingPoolSalesUpdate);

            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.RacePoolSalesUpdate, Enums.ActionCode.ActionSalesOpen)] = typeof(RacePoolSalesUpdate);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.RacePoolSalesUpdate, Enums.ActionCode.ActionSalesClosed)] = typeof(RacePoolSalesUpdate);

            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.ResultUpdateMsg, Enums.ActionCode.ActionPartialResult)] = typeof(ResultUpdate);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.ResultUpdateMsg, Enums.ActionCode.ActionFullResult)] = typeof(ResultUpdate);

            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.SubstituteUpdateMsg, Enums.ActionCode.ActionToteSubstitute)] = typeof(SubstituteUpdate);

            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.WeighedInUpdateMsg, Enums.ActionCode.ActionUnknown)] = typeof(WeighedInUpdate);

            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.RuOkRequestMsg, Enums.ActionCode.ActionUnknown)] = typeof(RuOkRequest);

            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.RacePoolDivUpdateMsg, Enums.ActionCode.ActionUnknown)] = typeof(RacePoolDividendUpdate);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.MeetingPoolDivUpdateMsg, Enums.ActionCode.ActionUnknown)] = typeof(MeetingPoolDividendUpdate);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.SuperComplexPoolDividendUpdate, Enums.ActionCode.ActionUnknown)] = typeof(SuperComplexPoolDividendUpdate);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.MatrixPoolDividendUpdate, Enums.ActionCode.ActionUnknown)] = typeof(MatrixPoolDividendUpdate);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.ComplexRacePoolDividendUpdate, Enums.ActionCode.ActionUnknown)] = typeof(ComplexRacePoolDividendUpdate);

            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.RunnerUpdateMsg, Enums.ActionCode.ActionRunning)] = typeof(RunnerUpdate);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.RunnerUpdateMsg, Enums.ActionCode.ActionNonRunner)] = typeof(RunnerUpdate);

            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.MeetingPayUpdateMsg, Enums.ActionCode.ActionPayOpen)] = typeof(MeetingPayUpdate);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.MeetingPayUpdateMsg, Enums.ActionCode.ActionPayClosed)] = typeof(MeetingPayUpdate);

            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.RacePayUpdateMsg, Enums.ActionCode.ActionPayOpen)] = typeof(RacePayUpdate);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.RacePayUpdateMsg, Enums.ActionCode.ActionPayClosed)] = typeof(RacePayUpdate);

            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.MeetingPoolPayUpdateMsg, Enums.ActionCode.ActionPayOpen)] = typeof(MeetingPoolPayUpdate);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.MeetingPoolPayUpdateMsg, Enums.ActionCode.ActionPayClosed)] = typeof(MeetingPoolPayUpdate);

            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.RacePoolPayUpdateMsg, Enums.ActionCode.ActionPayOpen)] = typeof(RacePoolPayUpdate);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.RacePoolPayUpdateMsg, Enums.ActionCode.ActionPayClosed)] = typeof(RacePoolPayUpdate);

            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.MeetingPoolWillPayUpdateMsg, Enums.ActionCode.ActionUnknown)] = typeof(MeetingPoolWillPayUpdate);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.LegBreakdownUpdateMsg, Enums.ActionCode.ActionUnknown)] = typeof(LegBreakdownUpdate);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.RacePoolWillPayUpdateMsg, Enums.ActionCode.ActionUnknown)] = typeof(RaceWillPayUpdate);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.RacePoolExtendedWillPayUpdateMsg, Enums.ActionCode.ActionUnknown)] = typeof(RaceExtendedWillPayUpdate);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.MeetingPoolTotalUpdate, Enums.ActionCode.ActionUnknown)] = typeof(MeetingPoolTotalUpdate);
            _lookup[new Tuple<Enums.MessageType, Enums.ActionCode>(Enums.MessageType.ComplexRacePoolTotalUpdate, Enums.ActionCode.ActionUnknown)] = typeof(ComplexRacePoolTotalUpdate);

            // set up updates to ignore (gets noisy in the log)

        }

        protected int GetNextBetId(int? betId)
        {
            if (betId.HasValue)
            {
                Interlocked.Exchange(ref _nextBetId, betId.Value + 1);
                return betId.Value;
            }
            else
            {
                return Interlocked.Increment(ref _nextBetId);
            }
        }

        protected void SendRequest(MessageBase message)
        {
            var stream = new MemoryStream();
            _serializer.Serialize(stream, message);
            var networkStream = _tcpClient.GetStream();
            var buffer = stream.ToArray();

            _logger.DebugFormat("Sending {0} bytes: {1}", buffer.Length, string.Join(" ", Array.ConvertAll(buffer, b => b.ToString("X2"))));
            OnRawPacketSent?.Invoke(buffer);

            networkStream.Write(buffer, 0, buffer.Length);
        }

        private void Callback(IAsyncResult asyncResult)
        {
            try
            {
                if (!asyncResult.IsCompleted)
                {
                    _logger.Error("Async operation didn't complete!");
                    return;
                }

                if (!(asyncResult.AsyncState is NetworkStream stream))
                {
                    _logger.Error("Invalid ASyncState on asyncResult");
                    return;
                }

                var bytesRead = stream.EndRead(asyncResult);

                if (bytesRead > 0)
                {
                    // a bit noisy with MSN's, would be nice to turn just this off
                    _logger.DebugFormat("Received {0} bytes: {1}", bytesRead, 
                        string.Join(" ", Array.ConvertAll(_buffer.Take(bytesRead).ToArray(), b => b.ToString("X2"))));

                    _circularBuffer.Put(_buffer, 0, bytesRead);
                    _logger.DebugFormat("{0} bytes read circ buffer[{1}]", bytesRead, _circularBuffer.ToString());
                    ParseBuffer();
                }
                var sizeLeftInBuffer = Math.Min(_circularBuffer.Capacity - _circularBuffer.Size, BufferSize);
                stream.BeginRead(_buffer, 0, sizeLeftInBuffer, Callback, stream); // should be in finally
            }
            catch(IOException ex)
            {
                // don't want the watchdog butting in when we're in orderly shutdown
                _watchdogTimer.Stop();
                _logger.Error("IOException", ex);
                if (!_shuttingDown)
                {
                    Disconnect(ex.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Callback", ex);
            }
            finally
            {

            }
            
        }

        private void ParseBuffer()
        {
            while (true)
            {
                // look for the marker FE DC BA 98
                while (_circularBuffer.Size > 0 && _circularBuffer.Peek() != 0xFE)
                {
                    _circularBuffer.Get();
                }

                if (_circularBuffer.Size < MessageBase.HEADER_LENGTH)
                {
                    break;
                }

                var buf1 = _circularBuffer.Peek(MessageBase.HEADER_LENGTH); 
                var header = _serializer.Deserialize<Header>(buf1);

                if (header.Marker == MessageBase.MARKER)
                {
                    if (_circularBuffer.Size < header.Length)
                    {
                        // wait for the rest of the packet to arrive
                        break;
                    }
                    else
                    {
                        // pull the entire packet from the buffer
                        var packetBuffer = _circularBuffer.Get(header.Length);
                        Watchdog.Kick();  // kick the dog whenever we receive a complete packet
                        OnRawPacketReceived?.Invoke(packetBuffer);
                        ProcessPacket(header, packetBuffer);
                    }
                }
                else
                {
                    // consume the first byte and move on
                    _circularBuffer.Get();
                }
            }
        }

        protected void Publish<T>(T message, Action<T> action)
        {
            action?.Invoke(message);
        }

        private WatchdogTimer Watchdog
        {
            get
            {
                if (_watchdogTimer == null)
                {
                    _watchdogTimer = new WatchdogTimer(_watchdogTimeoutMs);
                    _watchdogTimer.Start();
                    _watchdogTimer.OnTimeout += _watchdogTimer_OnTimeout;
                }
                return _watchdogTimer;
            }
        }

        private void ProcessRuok()
        {
            OnRuOk?.Invoke();
            Watchdog.Kick();
        }

        private void _watchdogTimer_OnTimeout(string obj)
        {
            _logger.DebugFormat("Watchdog timer timeout: {0}", obj);
            OnIdle?.Invoke(obj);
        }

        private void ProcessPacket(Header header, byte[] buffer)
        {
            if (_ignoreUpdates.ContainsKey(header.MessageType))
            {
                return;
            }

            var key = new Tuple<Enums.MessageType, Enums.ActionCode>(header.MessageType, header.ActionCode);
            if (_lookup.ContainsKey(key))
            {
                var pType = _lookup[key];
                var packet = _serializer.Deserialize(buffer, pType);

                if (pType == typeof(RuOkRequest))
                {
                    var reply = new RuOkReply();
                    QueueWork(reply);
                    ProcessRuok();
                }
                else if (pType == typeof(AccountLoginError))
                {
                    OnLoginError?.Invoke(packet as AccountLoginError);
                }
                else if (pType == typeof(AccountLoginSuccess))
                {
                    OnLoginSuccess?.Invoke(packet as AccountLoginSuccess);
                }
                else if (pType == typeof(AccountLogoutError))
                {
                    OnLogoutError?.Invoke(packet as AccountLogoutError);
                }
                else if (pType == typeof(AccountLogoutSuccess))
                {
                    OnLogoutSuccess?.Invoke(packet as AccountLogoutSuccess);
                }
                else if (pType == typeof(MeetingEndDateErrorReply))
                {
                    OnMeetingEndDateError?.Invoke(packet as MeetingEndDateErrorReply);
                }
                else if (pType == typeof(MeetingEndDateReply))
                {
                    OnMeetingEndDate?.Invoke(packet as MeetingEndDateReply);
                }
                else if (pType == typeof(MeetingReply))
                {
                    OnMeeting?.Invoke(packet as MeetingReply);
                }
                else if (pType == typeof(RacecardReply))
                {
                    OnRacecard?.Invoke(packet as RacecardReply);
                }
                else if (pType == typeof(RacePoolReply))
                {
                    OnRacePool?.Invoke(packet as RacePoolReply);
                }
                else if (pType == typeof(RaceReply))
                {
                    OnRace?.Invoke(packet as RaceReply);
                }
                else if (pType == typeof(RunnerReply))
                {
                    OnRunner?.Invoke(packet as RunnerReply);
                }
                else if (pType == typeof(MeetingPoolReply))
                {
                    OnMeetingPool?.Invoke(packet as MeetingPoolReply);
                }
                else if (pType == typeof(SellBetFailed))
                {
                    OnSellBetFailed?.Invoke(packet as SellBetFailed);
                }
                else if (pType == typeof(SellBetSuccess))
                {
                    OnSellBetSuccess?.Invoke(packet as SellBetSuccess);
                }
                else if (pType == typeof(PayEnquirySuccess))
                {
                    OnPayEnquirySuccess?.Invoke(packet as PayEnquirySuccess);
                }
                else if (pType == typeof(PayEnquiryFailed))
                {
                    OnPayEnquiryFailed?.Invoke(packet as PayEnquiryFailed);
                }
                else if (pType == typeof(RaceUpdate))
                {
                    OnRaceUpdate?.Invoke(packet as RaceUpdate);
                }
                else if (pType == typeof(MeetingPoolUpdate))
                {
                    OnMeetingPoolUpdate?.Invoke(packet as MeetingPoolUpdate);
                }
                else if (pType == typeof(RacePoolUpdate))
                {
                    OnRacePoolUpdate?.Invoke(packet as RacePoolUpdate);
                }
                else if (pType == typeof(PoolSubstituteUpdate))
                {
                    OnPoolSubstituteUpdate?.Invoke(packet as PoolSubstituteUpdate);
                }
                else if (pType == typeof(MeetingUpdate))
                {
                    OnMeetingUpdate?.Invoke(packet as MeetingUpdate);
                }
                else if (pType == typeof(MeetingSalesUpdate))
                {
                    OnMeetingSalesUpdate?.Invoke(packet as MeetingSalesUpdate);
                }
                else if (pType == typeof(RaceSalesUpdate))
                {
                    OnRaceSalesUpdate?.Invoke(packet as RaceSalesUpdate);
                }
                else if (pType == typeof(MeetingPoolSalesUpdate))
                {
                    OnMeetingPoolSalesUpdate?.Invoke(packet as MeetingPoolSalesUpdate);
                }
                else if (pType == typeof(RacePoolSalesUpdate))
                {
                    OnRacePoolSalesUpdate?.Invoke(packet as RacePoolSalesUpdate);
                }
                else if (pType == typeof(ResultUpdate))
                {
                    OnResultUpdate?.Invoke(packet as ResultUpdate);
                }
                else if (pType == typeof(SubstituteUpdate))
                {
                    OnSubstituteUpdate?.Invoke(packet as SubstituteUpdate);
                }
                else if (pType == typeof(WeighedInUpdate))
                {
                    OnWeighedInUpdate?.Invoke(packet as WeighedInUpdate);
                }
                else if (pType == typeof(EndOfRacingUpdate))
                {
                    OnEndOfRacingUpdate?.Invoke(packet as EndOfRacingUpdate);
                }
                else if (pType == typeof(MsnReply))
                {
                    OnMsnReply?.Invoke(packet as MsnReply);
                }
                else if (pType == typeof(CurrentMsnReply))
                {
                    OnCurrentMsnReply?.Invoke(packet as CurrentMsnReply);
                }
                else if (pType == typeof(RacePoolDividendUpdate))
                {
                    OnRacePoolDividendUpdate?.Invoke(packet as RacePoolDividendUpdate);
                }
                else if (pType == typeof(MeetingPoolWillPayUpdate))
                {
                    OnMeetingPoolWillPayUpdate?.Invoke(packet as MeetingPoolWillPayUpdate);
                }
                else if (pType == typeof(RaceWillPayUpdate))
                {
                    OnRaceWillPayUpdate?.Invoke(packet as RaceWillPayUpdate);
                }
                else if (pType == typeof(RaceExtendedWillPayUpdate))
                {
                    OnRaceExtendedWillPayUpdate?.Invoke(packet as RaceExtendedWillPayUpdate);
                }
                else if (pType == typeof(LegBreakdownUpdate))
                {
                    OnLegBreakdownUpdate?.Invoke(packet as LegBreakdownUpdate);
                }
                else if (pType == typeof(MeetingPoolTotalUpdate))
                {
                    OnMeetingPoolTotalUpdate?.Invoke(packet as MeetingPoolTotalUpdate);
                }
                else if (pType == typeof(ComplexRacePoolTotalUpdate))
                {
                    OnComplexRacePoolTotalUpdate?.Invoke(packet as ComplexRacePoolTotalUpdate);
                }
                else if (pType == typeof(RunnerUpdate))
                {
                    OnRunnerUpdate?.Invoke(packet as RunnerUpdate);
                }
                else if (pType == typeof(CurrentBalanceReply))
                {
                    OnCurrentBalanceReply?.Invoke(packet as CurrentBalanceReply);
                }
                else if (pType == typeof(MeetingPoolDividendUpdate))
                {
                    OnMeetingPoolDividendUpdate?.Invoke(packet as MeetingPoolDividendUpdate);
                }
                else if (pType == typeof(SuperComplexPoolDividendUpdate))
                {
                    OnSuperComplexPoolDividendUpdate?.Invoke(packet as SuperComplexPoolDividendUpdate);
                }
                else if (pType == typeof(MatrixPoolDividendUpdate))
                {
                    OnMatrixPoolDividendUpdate?.Invoke(packet as MatrixPoolDividendUpdate);
                }
                else if (pType == typeof(ComplexRacePoolDividendUpdate))
                {
                    OnComplexRacePoolDividendUpdate?.Invoke(packet as ComplexRacePoolDividendUpdate);
                }
                else if (pType == typeof(MeetingPayUpdate))
                {
                    OnMeetingPayUpdate?.Invoke(packet as MeetingPayUpdate);
                }
                else if (pType == typeof(RacePayUpdate))
                {
                    OnRacePayUpdate?.Invoke(packet as RacePayUpdate);
                }
                else if (pType == typeof(MeetingPoolPayUpdate))
                {
                    OnMeetingPoolPayUpdate?.Invoke(packet as MeetingPoolPayUpdate);
                }
                else if (pType == typeof(RacePoolPayUpdate))
                {
                    OnRacePoolPayUpdate?.Invoke(packet as RacePoolPayUpdate);
                }
            }
            else
            {
                _logger.WarnFormat("Key not found in map ({0}, {1})", key.Item1.ToString(), key.Item2.ToString());
            }
        }

        public bool Connect(string hostname, int port)
        {
            if (!_tcpClient.Connected)
            {
                try
                {
                    _tcpClient = new TcpClient();
                    _tcpClient.Connect(hostname, port);
                    if (!_tcpClient.Connected)
                    {
                        return false;
                    }
                    
                    var stream = _tcpClient.GetStream();
                    stream.BeginRead(_buffer, 0, BufferSize, new AsyncCallback(Callback), stream);

                    OnConnected?.Invoke();
                    return Start(); 
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    return false;
                }
            }

            return true;
        }

        private string _loggedInUsername;
        protected void LoginAsync(string username, string password)
        {
            var login = new AccountLoginRequest()
            {
                Username = username,
                Password = password
            };

            QueueWork(login);
        }

        protected void LogoutAsync(string username)
        {
            var logout = new AccountLogoutRequest()
            {
                Username = username
            };

            QueueWork(logout);
        }

        protected void GetRacecardAsync(DateTime forDate)
        {
            var req = new RacecardRequest()
            {
                Date = forDate.ToString(DateFormat)
            };

            QueueWork(req);
        }

        protected void GetMeetingAsync(int meetingNumber)
        {
            var req = new MeetingRequest()
            {
                MeetingNumber = (ushort)meetingNumber
            };

            QueueWork(req);
        }

        protected void GetRaceAsync(int meetingNumber, int raceNumber)
        {
            var req = new RaceRequest()
            {
                MeetingNumber = (ushort)meetingNumber,
                RaceNumber = (ushort)raceNumber
            };

            QueueWork(req);
        }

        protected void GetRacePoolAsync(int meetingNumber, int raceNumber, int poolNumber)
        {
            var req = new RacePoolRequest()
            {
                MeetingNumber = (ushort)meetingNumber,
                RaceNumber = (ushort)raceNumber,
                PoolNumber = (ushort)poolNumber
            };

            QueueWork(req);
        }

        protected void GetMeetingPoolAsync(int meetingNumber, int poolNumber)
        {
            var req = new MeetingPoolRequest()
            {
                MeetingNumber = (ushort)meetingNumber,
                PoolNumber = (ushort)poolNumber
            };

            QueueWork(req);
        }

        protected void GetRunnerAsync(int meetingNumber, int raceNumber, int runnerNumber)
        {
            var req = new RunnerRequest()
            {
                MeetingNumber = (ushort)meetingNumber,
                RaceNumber = (ushort)raceNumber,
                RunnerNumber = (ushort)runnerNumber
            };

            QueueWork(req);
        }

        protected int SellBetAsync(DateTime forDate, int meetingNumber,
            int unitStake, int totalStake,
            Enums.BetCode betCode, Enums.BetOption betOption, Selection[] selections, int? useBetId)
        {
            var betId = useBetId??GetNextBetId(useBetId);
            var req = new SellBetRequest()
            {
                RacecardDate = forDate.ToString(DateFormat),
                BetCode = betCode,
                BetId = (uint)betId,
                BetOption = betOption,
                NumberOfSelections = (ushort)selections.Length,
                Selections = selections.ToList(),
                TotalStake = (uint)totalStake,
                UnitStake = (uint)unitStake
            };

            QueueWork(req);
            return betId;
        }

        protected void PayEnquiryAsync(string tsn)
        {
            var req = new PayEnquiryRequest()
            {
                Tsn = tsn
            };

            QueueWork(req);
        }

        protected void CurrentMsnRequestAsync()
        {
            QueueWork(new CurrentMsnRequest());
        }

        protected void CurrentBalanceRequestAsync()
        {
            QueueWork(new CurrentBalanceRequest());
        }

        protected void MsnRequestAsync(ushort sequence)
        {
            QueueWork(new MsnRequest(sequence));
        }

        public void Disconnect(string reason = null)
        {
            try
            {
                if (_tcpClient != null)
                {
                    var stream = _tcpClient.GetStream();
                    if (stream != null)
                    {
                        stream.Close();
                    }
                    _tcpClient.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.DebugFormat("Disconnect", ex);
            }
            finally
            {
                if (!_shuttingDown) OnDisconnected?.Invoke(reason);
            }
        }

        protected override bool OnStart()
        {
            return true;
        }

        protected override bool OnStop()
        {
            _watchdogTimer?.Stop();
            return true;
        }

        protected override void Process(MessageBase t)
        {
            SendRequest(t);
        }

        protected override void OnItemQueued(MessageBase t)
        {
            // do nothing
        }

        public Task<bool> Login(string username, string password)
        {
            var tcs = new TaskCompletionSource<bool>();
            Action<AccountLoginSuccess> successHandler = null;
            Action<AccountLoginError> errorHandler = null;

            successHandler += (reply) =>
            {
                tcs.TrySetResult(true);
                _loggedInUsername = username;   
                OnLoginSuccess -= successHandler;
                OnLoginError -= errorHandler;
            };
            errorHandler += (reply) =>
            {
                tcs.TrySetResult(false);
                OnLoginSuccess -= successHandler;
                OnLoginError -= errorHandler;
            };

            OnLoginSuccess += successHandler;
            OnLoginError += errorHandler;

            LoginAsync(username, password);
            return tcs.Task;
        }

        public Task<bool> Logout(string username)
        {
            var tcs = new TaskCompletionSource<bool>();
            Action<AccountLogoutSuccess> successHandler = null;
            Action<AccountLogoutError> errorHandler = null;

            successHandler += (reply) =>
            {
                tcs.TrySetResult(true);
                OnLogoutSuccess -= successHandler;
                OnLogoutError -= errorHandler;
            };
            errorHandler += (reply) =>
            {
                tcs.TrySetResult(false);
                OnLogoutSuccess -= successHandler;
                OnLogoutError -= errorHandler;
            };

            OnLogoutSuccess += successHandler;
            OnLogoutError += errorHandler;

            LogoutAsync(username);
            return tcs.Task;
        }

        public Task<RacecardReply> GetRacecard(DateTime forDate)
        {
            var tcs = new TaskCompletionSource<RacecardReply>();
            Action<RacecardReply> handler = null;

            handler += (reply) =>
            {
                tcs.TrySetResult(reply);
                OnRacecard -= handler;
            };
            OnRacecard += handler;
            GetRacecardAsync(forDate);
            return tcs.Task;
        }

        public Task<MeetingReply> GetMeeting(int meetingNumber)
        {
            var tcs = new TaskCompletionSource<MeetingReply>();
            Action<MeetingReply> handler = null;

            handler += (reply) =>
            {
                tcs.TrySetResult(reply);
                OnMeeting -= handler;
            };
            OnMeeting += handler;
            GetMeetingAsync(meetingNumber);
            return tcs.Task;
        }

        public Task<RaceReply> GetRace(int meetingNumber, int raceNumber)
        {
            var tcs = new TaskCompletionSource<RaceReply>();
            Action<RaceReply> handler = null;

            handler += (reply) =>
            {
                tcs.TrySetResult(reply);
                OnRace -= handler;
            };
            OnRace += handler;
            GetRaceAsync(meetingNumber, raceNumber);
            return tcs.Task;
        }

        public Task<RunnerReply> GetRunner(int meetingNumber, int raceNumber, int runnerNumber)
        {
            var tcs = new TaskCompletionSource<RunnerReply>();
            Action<RunnerReply> handler = null;

            handler += (reply) =>
            {
                tcs.TrySetResult(reply);
                OnRunner -= handler;
            };
            OnRunner += handler;
            GetRunnerAsync(meetingNumber, raceNumber, runnerNumber);
            return tcs.Task;
        }

        public Task<BetReply> SellBet(DateTime forDate, int meetingNumber, int raceNumber, 
            int unitStake, int totalStake,
            Enums.BetCode betCode, Enums.BetOption betOption, int[] selections, int? betId=null)
        {
            switch (betCode)
            {
                case Enums.BetCode.WIN:
                case Enums.BetCode.PLACE:
                case Enums.BetCode.QUINELLA:
                case Enums.BetCode.EXACTA:
                case Enums.BetCode.SWINGER:
                case Enums.BetCode.TRIFECTA:
                    return SellBet(forDate, meetingNumber, unitStake, totalStake, betCode, betOption, selections.Select(s => (raceNumber, s)).ToArray(), betId);
                default:
                    return SellBet(forDate, meetingNumber, unitStake, totalStake, betCode, betOption, selections.Select(s => (s / 100, s % 100)).ToArray(), betId);
            }
            
        }

        public Task<BetReply> SellBet(DateTime forDate, int meetingNumber, 
            int unitStake, int totalStake,
            Enums.BetCode betCode, Enums.BetOption betOption, (int race, int selection)[] selections, int? betId = null)
        {
            var tcs = new TaskCompletionSource<BetReply>();
            Action<SellBetSuccess> successHandler = null;
            Action<SellBetFailed> failedHandler = null;

            successHandler += (reply) =>
            {
                _logger.DebugFormat("BetId {0} succeeded", reply.BetId);
                tcs.TrySetResult(new BetReply()
                {
                    Tsn = reply.Tsn,
                    BetId = reply.BetId,
                    ErrorCode = Enums.ErrorCode.Success,
                    ErrorText = string.Empty
                });
                OnSellBetSuccess -= successHandler;
                OnSellBetFailed -= failedHandler;
            };
            failedHandler += (reply) =>
            {
                _logger.DebugFormat("BetId {0} failed ErrorCode({1}) ErrorText({2})", reply.BetId, reply.ErrorCode2, reply.ErrorText);
                tcs.TrySetResult(new BetReply()
                {
                    Tsn = string.Empty,
                    BetId = reply.BetId,
                    ErrorCode = reply.ErrorCode2,
                    ErrorText = reply.ErrorText
                });
                OnSellBetSuccess -= successHandler;
                OnSellBetFailed -= failedHandler;
            };

            OnSellBetSuccess += successHandler;
            OnSellBetFailed += failedHandler;

            SellBetAsync(forDate, meetingNumber, unitStake, totalStake, betCode, betOption,
                       Selection.Create((ushort)meetingNumber, selections), betId);

            return tcs.Task;
        }

        public Task<IList<PayEnquiryReply>> PayEnquiryBatch(IList<string> tsnList)
        {
            var tcs = new TaskCompletionSource<IList<PayEnquiryReply>>();
            Action<PayEnquirySuccess> successHandler = null;
            Action<PayEnquiryFailed> failedHandler = null;

            var responses = new ConcurrentDictionary<string, PayEnquiryReply>();
            int responseCount = 0;
            int betCount = tsnList.Count();

            successHandler += (reply) => 
            {
                var tsn = reply.Tsn.Replace("\0", string.Empty);
                _logger.DebugFormat("Pay enquiry {0} succeeded", tsn);
                if (responses.ContainsKey(tsn))
                {
                    responses[tsn] = new PayEnquiryReply()
                    {
                        Tsn = tsn,
                        PayoutAmount = reply.PayoutAmount,
                        VoidAmount = reply.VoidAmount,
                        ErrorCode = Enums.ErrorCode.Success,
                        ErrorText = string.Empty
                    };
                    if (Interlocked.Increment(ref responseCount) >= betCount)
                    {
                        OnPayEnquirySuccess -= successHandler;
                        OnPayEnquiryFailed -= failedHandler;
                        tcs.TrySetResult(responses.Values.OrderBy(v => v.Tsn).ToList());
                    }
                    OnBatchProgress?.Invoke(responseCount, betCount);
                }
            };
            failedHandler += (reply) =>
            {
                var tsn = reply.Tsn.Replace("\0", string.Empty);
                _logger.DebugFormat("Pay enquiry {0} failed", tsn);
                if (responses.ContainsKey(tsn))
                {
                    responses[tsn] = new PayEnquiryReply()
                    {
                        Tsn = tsn,
                        ErrorCode = reply.ErrorCode2,
                        ErrorText = reply.ErrorText.Replace("\0", string.Empty)
                    };
                }
                if (Interlocked.Increment(ref responseCount) >= betCount)
                {
                    OnPayEnquirySuccess -= successHandler;
                    OnPayEnquiryFailed -= failedHandler;
                    tcs.TrySetResult(responses.Values.OrderBy(v => v.Tsn).ToList());
                }
                OnBatchProgress?.Invoke(responseCount, betCount);
            };

            OnPayEnquirySuccess += successHandler;
            OnPayEnquiryFailed += failedHandler;

            foreach (var tsn in tsnList)
            {
                responses[tsn] = null;
                PayEnquiryAsync(tsn);
            }

            return tcs.Task;
        }

        public Task<IList<BetReply>> SellBatch(IList<BetRequest> batch)
        {
            var tcs = new TaskCompletionSource<IList<BetReply>>();
            Action<SellBetSuccess> successHandler = null;
            Action<SellBetFailed> failedHandler = null;

            var responses = new ConcurrentDictionary<int, BetReply>();
            int responseCount = 0;
            int betCount = batch.Count();

            successHandler += (reply) =>
            {
                _logger.DebugFormat("BetId {0} succeeded", reply.BetId);
                if (responses.ContainsKey((int)reply.BetId))
                {
                    responses[(int)reply.BetId] = new BetReply()
                    {
                        Tsn = reply.Tsn,
                        BetId = reply.BetId,
                        ErrorCode = Enums.ErrorCode.Success,
                        ErrorText = string.Empty,
                        Ref = _refLookup.ContainsKey((int)reply.BetId) ? _refLookup[(int)reply.BetId] : (Guid?)null
                    };
                    if (Interlocked.Increment(ref responseCount) >= betCount)
                    {
                        OnSellBetSuccess -= successHandler;
                        OnSellBetFailed -= failedHandler;
                        tcs.TrySetResult(responses.Values.OrderBy(v => v?.BetId).ToList());
                    }
                    OnBatchProgress?.Invoke(responseCount, betCount);
                }
            };
            failedHandler += (reply) =>
            {
                _logger.DebugFormat("BetId {0} failed", reply.BetId);
                if (responses.ContainsKey((int)reply.BetId))
                {
                    responses[(int)reply.BetId] = new BetReply()
                    {
                        Tsn = string.Empty,
                        BetId = reply.BetId,
                        ErrorCode = reply.ErrorCode2,
                        ErrorText = reply.ErrorText,
                        Ref = _refLookup.ContainsKey((int)reply.BetId) ? _refLookup[(int)reply.BetId] : (Guid?)null
                    };
                }
                if (Interlocked.Increment(ref responseCount) >= betCount)
                {
                    OnSellBetSuccess -= successHandler;
                    OnSellBetFailed -= failedHandler;
                    tcs.TrySetResult(responses.Values.OrderBy(v => v?.BetId).ToList());
                }
                OnBatchProgress?.Invoke(responseCount, betCount);
            };

            OnSellBetSuccess += successHandler;
            OnSellBetFailed += failedHandler;

            foreach (var bet in batch)
            {
                bet.BetId = GetNextBetId(bet.BetId);
                responses[bet.BetId.Value] = null;
                _refLookup[bet.BetId.Value] = bet.Ref;

                switch (bet.BetCode)
                {
                    case Enums.BetCode.WIN:
                    case Enums.BetCode.PLACE:
                    case Enums.BetCode.QUINELLA:
                    case Enums.BetCode.EXACTA:
                    case Enums.BetCode.SWINGER:
                    case Enums.BetCode.TRIFECTA:
                        // single race
                        SellBetAsync(bet.ForDate, bet.MeetingNumber, bet.UnitStake, bet.TotalStake, bet.BetCode, bet.BetOption, bet.Selections, bet.BetId);
                        break;
                    default:
                        // multi race
                        SellBetAsync(bet.ForDate, bet.MeetingNumber, bet.UnitStake, bet.TotalStake, bet.BetCode, bet.BetOption, bet.Selections, bet.BetId);
                        break;

                }
            }

            return tcs.Task;
        }

        public Task<PayEnquiryReply> PayEnquiry(string tsn)
        {
            var tcs = new TaskCompletionSource<PayEnquiryReply>();
            Action<PayEnquirySuccess> successHandler = null;
            Action<PayEnquiryFailed> failedHandler = null;

            successHandler += (reply) =>
            {
                tcs.TrySetResult(new PayEnquiryReply()
                {
                    Tsn = reply.Tsn,
                    PayoutAmount = reply.PayoutAmount,
                    VoidAmount = reply.VoidAmount,
                    ErrorCode = Enums.ErrorCode.Success,
                    ErrorText = string.Empty
                });
                OnPayEnquirySuccess -= successHandler;
                OnPayEnquiryFailed -= failedHandler;
            };
            failedHandler += (reply) =>
            {
                tcs.TrySetResult(new PayEnquiryReply()
                {
                    Tsn = string.Empty,
                    ErrorCode = reply.ErrorCode2,
                    ErrorText = reply.ErrorText
                });
                OnPayEnquirySuccess -= successHandler;
                OnPayEnquiryFailed -= failedHandler;
            };

            OnPayEnquirySuccess += successHandler;
            OnPayEnquiryFailed += failedHandler;

            PayEnquiryAsync(tsn);
            return tcs.Task;
        }

        public Task<MsnReply> GetMsn(int sequence)
        {
            var tcs = new TaskCompletionSource<MsnReply>();
            Action<MsnReply> handler = null;

            handler += (reply) =>
            {
                tcs.TrySetResult(reply);
                OnMsnReply -= handler;
            };
            OnMsnReply += handler;
            MsnRequestAsync((ushort)sequence);
            return tcs.Task;
        }

        public Task<CurrentMsnReply> GetCurrentMsn()
        {
            var tcs = new TaskCompletionSource<CurrentMsnReply>();
            Action<CurrentMsnReply> handler = null;

            handler += (reply) =>
            {
                tcs.TrySetResult(reply);
                OnCurrentMsnReply -= handler;
            };
            OnCurrentMsnReply += handler;
            CurrentMsnRequestAsync();
            return tcs.Task;
        }

        public Task<CurrentBalanceReply> GetCurrentBalance()
        {
            var tcs = new TaskCompletionSource<CurrentBalanceReply>();
            Action<CurrentBalanceReply> handler = null;

            handler += (reply) =>
            {
                tcs.TrySetResult(reply);
                OnCurrentBalanceReply -= handler;
            };
            OnCurrentBalanceReply += handler;
            CurrentBalanceRequestAsync();
            return tcs.Task;
        }

        public void Dispose()
        {
            _shuttingDown = true;
            if (!string.IsNullOrEmpty(_loggedInUsername))
            {
                LogoutAsync(_loggedInUsername);
            }
            Disconnect("stopping");
            Stop();
        }
    }
}
