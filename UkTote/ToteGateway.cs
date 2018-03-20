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
    public partial class ToteGateway : CancellableQueueWorker<Message.MessageBase>, IDisposable, IToteGateway
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(ToteGateway));

        const string DateFormat = "ddMMyyyy";
        const int BUFFER_SIZE = 16384;

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
        public event Action<SellBetFailed> OnSellBetFailed;
        public event Action<SellBetSuccess> OnSellBetSuccess;
        public event Action<PayEnquirySuccess> OnPayEnquirySuccess;
        public event Action<PayEnquiryFailed> OnPayEnquiryFailed;
        public event Action<MsnReply> OnMsnReply;
        public event Action<CurrentMsnReply> OnCurrentMsnReply;
        public event Action OnConnected;
        public event Action<string> OnDisconnected;
        public event Action<string> OnIdle;

        // update events
        public event Action<MeetingSalesUpdate> OnMeetingSalesUpdate;
        public event Action<RaceUpdate> OnRaceUpdate;
        public event Action<RacePoolUpdate> OnRacePoolUpdate;
        public event Action<RaceSalesUpdate> OnRaceSalesUpdate;
        public event Action<RacePoolSalesUpdate> OnRacePoolSalesUpdate;
        public event Action<RacePoolDividendUpdate> OnRacePoolDividendUpdate;
        public event Action<RaceWillPayUpdate> OnRaceWillPayUpdate;
        public event Action<RunnerUpdate> OnRunnerUpdate;

        private bool _shuttingDown = false;
        private int _nextBetId = 0; // TODO - this gets set to 0 at start of day and persisted
        private TcpClient _tcpClient = new TcpClient();
        private BinarySerializer _serializer;
        private CircularBuffer<byte> _circularBuffer;
        private byte[] _buffer = new byte[BUFFER_SIZE];
        private Dictionary<Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>, Type> _lookup = new Dictionary<Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>, Type>();
        private Dictionary<Message.Enums.MessageType, bool> _ignoreUpdates = new Dictionary<Message.Enums.MessageType, bool>();
        private WatchdogTimer _watchdogTimer;
        private readonly int _watchdogTimeoutMs;

        public int NextBetId
        {
            set { _nextBetId = value; }
        }

        public bool IsConnected
        {
            get
            {
                return _tcpClient != null && _tcpClient.Connected;
            }
        }

        public ToteGateway(int watchdogTimeoutMs)
            : base(0, 1)
        {
            _watchdogTimeoutMs = watchdogTimeoutMs;
            _serializer = new BinarySerializer();
            _serializer.Endianness = Endianness.Big;
            _circularBuffer = new CircularBuffer<byte>(BUFFER_SIZE * 4);

            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.ACCOUNT_LOGIN, Message.Enums.ActionCode.ACTION_SUCCESS)] = typeof(AccountLoginSuccess);
            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.ACCOUNT_LOGIN, Message.Enums.ActionCode.ACTION_FAIL)] = typeof(AccountLoginError);

            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.ACCOUNT_LOGOUT, Message.Enums.ActionCode.ACTION_LOGOUT)] = typeof(AccountLogoutSuccess);
            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.ACCOUNT_LOGOUT, Message.Enums.ActionCode.ACTION_FAIL)] = typeof(AccountLogoutError);

            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.RACECARD_REQ_MSG, Message.Enums.ActionCode.ACTION_UNKNOWN)] = typeof(RacecardReply);
            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.RACECARD_REQ_MSG, Message.Enums.ActionCode.ACTION_FAIL)] = typeof(RacecardReply);

            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.MEETING_REQ_MSG, Message.Enums.ActionCode.ACTION_UNKNOWN)] = typeof(MeetingReply);
            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.MEETING_REQ_MSG, Message.Enums.ActionCode.ACTION_FAIL)] = typeof(MeetingReply);
            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.MEETING_REQ_MSG, Message.Enums.ActionCode.ACTION_ON)] = typeof(MeetingReply);

            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.RACE_REQ_MSG, Message.Enums.ActionCode.ACTION_UNKNOWN)] = typeof(RaceReply);
            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.RACE_REQ_MSG, Message.Enums.ActionCode.ACTION_FAIL)] = typeof(RaceReply);
            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.RACE_REQ_MSG, Message.Enums.ActionCode.ACTION_ON)] = typeof(RaceReply);

            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.RUNNER_REQ_MSG, Message.Enums.ActionCode.ACTION_UNKNOWN)] = typeof(RunnerReply);
            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.RUNNER_REQ_MSG, Message.Enums.ActionCode.ACTION_FAIL)] = typeof(RunnerReply);
            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.RUNNER_REQ_MSG, Message.Enums.ActionCode.ACTION_RUNNING)] = typeof(RunnerReply);
            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.RUNNER_REQ_MSG, Message.Enums.ActionCode.ACTION_NON_RUNNER)] = typeof(RunnerReply);

            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.POOL_REQ_MSG, Message.Enums.ActionCode.ACTION_UNKNOWN)] = typeof(RacePoolReply);
            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.POOL_REQ_MSG, Message.Enums.ActionCode.ACTION_FAIL)] = typeof(RacePoolReply);
            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.POOL_REQ_MSG, Message.Enums.ActionCode.ACTION_NORMAL)] = typeof(RacePoolReply);

            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.SELL_BET_REQ_MSG, Message.Enums.ActionCode.ACTION_SUCCESS)] = typeof(SellBetSuccess);
            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.SELL_BET_REQ_MSG, Message.Enums.ActionCode.ACTION_FAIL)] = typeof(SellBetFailed);

            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.PAY_BET_REQ_MSG, Message.Enums.ActionCode.ACTION_SUCCESS)] = typeof(PayEnquirySuccess);
            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.PAY_BET_REQ_MSG, Message.Enums.ActionCode.ACTION_FAIL)] = typeof(PayEnquiryFailed);

            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.MSN_REQ_MSG, Message.Enums.ActionCode.ACTION_SUCCESS)] = typeof(MsnReply);
            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.MSN_REQ_MSG, Message.Enums.ActionCode.ACTION_FAIL)] = typeof(MsnReply);

            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.CURRENT_MSN_REQ_MSG, Message.Enums.ActionCode.ACTION_SUCCESS)] = typeof(CurrentMsnReply);
            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.CURRENT_MSN_REQ_MSG, Message.Enums.ActionCode.ACTION_FAIL)] = typeof(CurrentMsnReply);

            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.MEETING_UPDATE_MSG, Message.Enums.ActionCode.ACTION_ON)] = typeof(MeetingUpdate);
            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.MEETING_UPDATE_MSG, Message.Enums.ActionCode.ACTION_CANCELLED)] = typeof(MeetingUpdate);
            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.MEETING_UPDATE_MSG, Message.Enums.ActionCode.ACTION_POSTPONED)] = typeof(MeetingUpdate);

            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.RACE_UPDATE_MSG, Message.Enums.ActionCode.ACTION_ON)] = typeof(RaceUpdate);
            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.RACE_UPDATE_MSG, Message.Enums.ActionCode.ACTION_OFF)] = typeof(RaceUpdate);
            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.RACE_UPDATE_MSG, Message.Enums.ActionCode.ACTION_CLOSED)] = typeof(RaceUpdate);
            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.RACE_UPDATE_MSG, Message.Enums.ActionCode.ACTION_CANCELLED)] = typeof(RaceUpdate);
            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.RACE_UPDATE_MSG, Message.Enums.ActionCode.ACTION_POSTPONED)] = typeof(RaceUpdate);
            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.RACE_UPDATE_MSG, Message.Enums.ActionCode.ACTION_VOID)] = typeof(RaceUpdate);

            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.RUNNER_UPDATE_MSG, Message.Enums.ActionCode.ACTION_RUNNING)] = typeof(Message.RunnerUpdate);
            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.RUNNER_UPDATE_MSG, Message.Enums.ActionCode.ACTION_NON_RUNNER)] = typeof(Message.RunnerUpdate);

            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.RACE_POOL_UPDATE_MSG, Message.Enums.ActionCode.ACTION_ON)] = typeof(Message.RacePoolUpdate);
            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.RACE_POOL_UPDATE_MSG, Message.Enums.ActionCode.ACTION_CANCELLED)] = typeof(Message.RacePoolUpdate);

            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.MEETING_SALES_UPDATE, Message.Enums.ActionCode.ACTION_SALES_OPEN)] = typeof(Message.MeetingSalesUpdate);
            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.MEETING_SALES_UPDATE, Message.Enums.ActionCode.ACTION_SALES_CLOSED)] = typeof(Message.MeetingSalesUpdate);

            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.RACE_SALES_UPDATE, Message.Enums.ActionCode.ACTION_SALES_OPEN)] = typeof(Message.RaceSalesUpdate);
            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.RACE_SALES_UPDATE, Message.Enums.ActionCode.ACTION_SALES_CLOSED)] = typeof(Message.RaceSalesUpdate);

            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.RACE_POOL_SALES_UPDATE, Message.Enums.ActionCode.ACTION_SALES_OPEN)] = typeof(Message.RacePoolSalesUpdate);
            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.RACE_POOL_SALES_UPDATE, Message.Enums.ActionCode.ACTION_SALES_CLOSED)] = typeof(Message.RacePoolSalesUpdate);

            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.RUOk_REQUEST_MSG, Message.Enums.ActionCode.ACTION_UNKNOWN)] = typeof(Message.RuOkRequest);

            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.RACE_POOL_DIV_UPDATE_MSG, Message.Enums.ActionCode.ACTION_UNKNOWN)] = typeof(Message.RacePoolDividendUpdate);
            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.RACE_POOL_WILL_PAY_UPDATE_MSG, Message.Enums.ActionCode.ACTION_UNKNOWN)] = typeof(Message.RaceWillPayUpdate);

            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.RUNNER_UPDATE_MSG, Message.Enums.ActionCode.ACTION_RUNNING)] = typeof(Message.RunnerUpdate);
            _lookup[new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(Message.Enums.MessageType.RUNNER_UPDATE_MSG, Message.Enums.ActionCode.ACTION_NON_RUNNER)] = typeof(Message.RunnerUpdate);

            // set up updates to ignore (gets noisy in the log)
            _ignoreUpdates[Message.Enums.MessageType.MEET_POOL_WILL_PAY_UPDATE_MSG] = true;
            //_ignoreUpdates[Message.Enums.MessageType.RACE_POOL_WILL_PAY_UPDATE_MSG] = true;
            _ignoreUpdates[Message.Enums.MessageType.RESULT_UPDATE_MSG] = true;
            _ignoreUpdates[Message.Enums.MessageType.WEIGHED_IN_MSG] = true;

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

        protected void SendRequest(Message.MessageBase message)
        {
            var stream = new MemoryStream();
            _serializer.Serialize(stream, message);
            var networkStream = _tcpClient.GetStream();
            var buffer = stream.ToArray();

            _logger.DebugFormat("Sending {0} bytes: {1}", buffer.Length, string.Join(" ", Array.ConvertAll(buffer, b => b.ToString("X2"))));

            networkStream.Write(buffer, 0, buffer.Length);
        }

        void Callback(IAsyncResult asyncResult)
        {
            try
            {
                if (!asyncResult.IsCompleted)
                {
                    _logger.Error("Async operation didn't complete!");
                    return;
                }
                var stream = asyncResult.AsyncState as NetworkStream;

                if (stream == null)
                {
                    _logger.Error("Invalid ASyncState on asyncResult");
                    return;
                }

                var bytesRead = stream.EndRead(asyncResult);

                if (bytesRead > 0)
                {
                    // a bit noisy with MSN's, would be nice to turn just this off
                    //_logger.DebugFormat("Received {0} bytes: {1}", bytesRead, 
                    //    string.Join(" ", Array.ConvertAll(_buffer.Take(bytesRead).ToArray(), b => b.ToString("X2"))));

                    _circularBuffer.Put(_buffer, 0, bytesRead);
                    _logger.DebugFormat("{0} bytes read circ buffer[{1}]", bytesRead, _circularBuffer.ToString());
                    ParseBuffer();
                }
                stream.BeginRead(_buffer, 0, BUFFER_SIZE, Callback, stream); // should be in finally
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

        void ParseBuffer()
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
                var header = _serializer.Deserialize<Message.Header>(buf1);

                if (header.Marker == MessageBase.MARKER)
                {
                    if (_circularBuffer.Size < (header.Length - MessageBase.HEADER_LENGTH))
                    {
                        // wait for the rest of the packet to arrive
                        break;
                    }
                    else
                    {
                        // pull the entire packet from the buffer
                        var packetBuffer = _circularBuffer.Get(header.Length);
                        _watchdogTimer?.Kick();  // kick the dog whenever we receive a complete packet
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

        void ProcessRuok()
        {
            if (_watchdogTimer == null)
            {
                _watchdogTimer = new WatchdogTimer(_watchdogTimeoutMs);
                _watchdogTimer.Start();
                _watchdogTimer.OnTimeout += _watchdogTimer_OnTimeout;
            }
            else
            {
                _watchdogTimer.Kick();
            }
        }

        private void _watchdogTimer_OnTimeout(string obj)
        {
            _logger.DebugFormat("Watchdog timer timeout: {0}", obj);
            OnIdle?.Invoke(obj);
        }

        void ProcessPacket(Message.Header header, byte[] buffer)
        {
            if (_ignoreUpdates.ContainsKey(header.MessageType))
            {
                return;
            }

            var key = new Tuple<Message.Enums.MessageType, Message.Enums.ActionCode>(header.MessageType, header.ActionCode);
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
                else if (pType == typeof(RacePoolUpdate))
                {
                    OnRacePoolUpdate?.Invoke(packet as RacePoolUpdate);
                }
                else if (pType == typeof(MeetingSalesUpdate))
                {
                    OnMeetingSalesUpdate?.Invoke(packet as MeetingSalesUpdate);
                }
                else if (pType == typeof(RaceSalesUpdate))
                {
                    OnRaceSalesUpdate?.Invoke(packet as RaceSalesUpdate);
                }
                else if (pType == typeof(RacePoolSalesUpdate))
                {
                    OnRacePoolSalesUpdate?.Invoke(packet as RacePoolSalesUpdate);
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
                else if (pType == typeof(RaceWillPayUpdate))
                {
                    OnRaceWillPayUpdate?.Invoke(packet as RaceWillPayUpdate);
                }
                else if (pType == typeof(RunnerUpdate))
                {
                    OnRunnerUpdate?.Invoke(packet as RunnerUpdate);
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
                    stream.BeginRead(_buffer, 0, BUFFER_SIZE, new AsyncCallback(Callback), stream);

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

        string _loggedInUsername;
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

        protected int SellBetAsync(DateTime forDate, int meetingNumber, int raceNumber,
            int unitStake, int totalStake,
            Message.Enums.BetCode betCode, Message.Enums.BetOption betOption, Selection[] selections, int? useBetId)
        {
            var betId = GetNextBetId(useBetId);
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
                TSN = tsn
            };

            QueueWork(req);
        }

        protected void CurrentMsnRequestAsync()
        {
            QueueWork(new CurrentMsnRequest());
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
            Message.Enums.BetCode betCode, Message.Enums.BetOption betOption, int[] selections)
        {
            switch(betCode)
            {
                case Message.Enums.BetCode.EXACTA:
                case Message.Enums.BetCode.PLACE:
                case Message.Enums.BetCode.SWINGER:
                case Message.Enums.BetCode.QUINELLA:
                case Message.Enums.BetCode.TRIFECTA:
                case Message.Enums.BetCode.WIN:
                    // single leg bet
                    return SellBet(forDate, meetingNumber, raceNumber, unitStake, totalStake, betCode, betOption,
                        Selection.Create((ushort)meetingNumber, (ushort)raceNumber, selections));

                default:
                    // multi leg bet
                    return SellBet(forDate, meetingNumber, raceNumber, unitStake, totalStake, betCode, betOption,
                        Selection.Create((ushort)meetingNumber, selections));
            }
        }

        public Task<BetReply> SellBet(DateTime forDate, int meetingNumber, int raceNumber, 
            int unitStake, int totalStake, 
            Message.Enums.BetCode betCode, Message.Enums.BetOption betOption, Selection[] selections, int? betId=null)
        {
            var tcs = new TaskCompletionSource<BetReply>();
            Action<SellBetSuccess> successHandler = null;
            Action<SellBetFailed> failedHandler = null;

            successHandler += (reply) =>
            {
                _logger.DebugFormat("BetId {0} succeeded", reply.BetId);
                tcs.TrySetResult(new BetReply()
                {
                    TSN = reply.TSN,
                    BetId = reply.BetId,
                    ErrorCode = Message.Enums.ErrorCode.SUCCESS,
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
                    TSN = string.Empty,
                    BetId = reply.BetId,
                    ErrorCode = reply.ErrorCode2,
                    ErrorText = reply.ErrorText
                });
                OnSellBetSuccess -= successHandler;
                OnSellBetFailed -= failedHandler;
            };

            OnSellBetSuccess += successHandler;
            OnSellBetFailed += failedHandler;

            SellBetAsync(forDate, meetingNumber, raceNumber, unitStake, totalStake, betCode, betOption, selections, betId);
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
                        TSN = reply.TSN,
                        BetId = reply.BetId,
                        ErrorCode = Message.Enums.ErrorCode.SUCCESS,
                        ErrorText = string.Empty
                    };
                    if (Interlocked.Increment(ref responseCount) >= betCount)
                    {
                        OnSellBetSuccess -= successHandler;
                        OnSellBetFailed -= failedHandler;
                        tcs.TrySetResult(responses.Values.OrderBy(v => v.BetId).ToList());
                    }
                }
            };
            failedHandler += (reply) =>
            {
                _logger.DebugFormat("BetId {0} failed", reply.BetId);
                if (responses.ContainsKey((int)reply.BetId))
                {
                    responses[(int)reply.BetId] = new BetReply()
                    {
                        TSN = string.Empty,
                        BetId = reply.BetId,
                        ErrorCode = reply.ErrorCode2,
                        ErrorText = reply.ErrorText
                    };
                }
                if (Interlocked.Increment(ref responseCount) >= betCount)
                {
                    OnSellBetSuccess -= successHandler;
                    OnSellBetFailed -= failedHandler;
                    tcs.TrySetResult(responses.Values.OrderBy(v => v.BetId).ToList());
                }
            };

            OnSellBetSuccess += successHandler;
            OnSellBetFailed += failedHandler;

            foreach (var bet in batch)
            {
                var betId = SellBetAsync(bet.ForDate, bet.MeetingNumber, bet.RaceNumber, bet.UnitStake, bet.TotalStake, bet.BetCode, bet.BetOption, 
                    Selection.Create((ushort)bet.MeetingNumber, (ushort)bet.RaceNumber, bet.Selections), bet.BetId);

                responses[betId] = null;
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
                    TSN = reply.TSN,
                    PayoutAmount = reply.PayoutAmount,
                    VoidAmount = reply.VoidAmount,
                    ErrorCode = Message.Enums.ErrorCode.SUCCESS,
                    ErrorText = string.Empty
                });
                OnPayEnquirySuccess -= successHandler;
                OnPayEnquiryFailed -= failedHandler;
            };
            failedHandler += (reply) =>
            {
                tcs.TrySetResult(new PayEnquiryReply()
                {
                    TSN = string.Empty,
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
