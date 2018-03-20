using System;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UkTote;
using UkTote.Message;
using log4net;

namespace TBS.ARK.Tests
{
    [TestClass]
    public class UkToteTests
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(UkToteTests));

        const string host = "217.46.202.209";
        const int port = 8032;
        const string username = "ATCentrum9";
        const string password = "at9password";
        ToteGateway gateway = new ToteGateway(120000);

        public UkToteTests()
        {
            log4net.Config.XmlConfigurator.Configure();
            //gateway.OnReply += Gateway_OnReply;
            //gateway.OnUpdate += Gateway_OnUpdate;
        }

        [TestMethod]
        public async Task TestLogin()
        {
            var connected = gateway.Connect(host, port);
            Assert.IsTrue(connected);
            var loggedIn = await gateway.Login(username, password);
            Assert.IsTrue(loggedIn);
            gateway.Disconnect();
        }

        [TestMethod]
        public async Task GetRaceday()
        {
            var connected = gateway.Connect(host, port);
            Assert.IsTrue(connected);
            var loggedIn = await gateway.Login(username, password);
            Assert.IsTrue(loggedIn);

            var raceCard = await gateway.GetRacecard(DateTime.UtcNow);
            Assert.IsTrue(raceCard.NumMeetings > 0);

            for (var i = 1; i <= raceCard.NumMeetings; ++i)
            {
                var meeting = await gateway.GetMeeting(i);

                Assert.IsTrue(meeting.NumberOfRaces > 0);

                for (var j = 1; j <= meeting.NumberOfRaces; ++j)
                {
                    var race = await gateway.GetRace(i, j);

                    Assert.IsTrue(race.NumberOfDeclaredRunners > 0);
                    for (var k = 1; k <= race.NumberOfDeclaredRunners; ++k)
                    {
                        var runner = await gateway.GetRunner(i, j, k);
                    }
                }
            }
            gateway.Disconnect();
        }

        [TestMethod]
        public async Task TestGetRacecardFast()
        {
            var connected = gateway.Connect(host, port);
            Assert.IsTrue(connected);
            var loggedIn = await gateway.Login(username, password);
            Assert.IsTrue(loggedIn);
            var racecard = await gateway.GetRacecardFast(DateTime.UtcNow, true);
        }

        [TestMethod]
        public async Task TestGetFastMeetings()
        {
            var connected = gateway.Connect(host, port);
            Assert.IsTrue(connected);
            var loggedIn = await gateway.Login(username, password);
            Assert.IsTrue(loggedIn);

            var raceCard = await gateway.GetRacecard(DateTime.UtcNow);
            Assert.IsTrue(raceCard.NumMeetings > 0);

            var meetings = await gateway.GetMeetings(raceCard.NumMeetings);
            Assert.AreEqual(meetings.Count, raceCard.NumMeetings);

            gateway.Disconnect();
        }

        [TestMethod]
        public async Task TestMsn()
        {
            var connected = gateway.Connect(host, port);
            Assert.IsTrue(connected);
            var loggedIn = await gateway.Login(username, password);
            Assert.IsTrue(loggedIn);

            var currentMsnReply = await gateway.GetCurrentMsn();
            if (currentMsnReply.ActionCode == Enums.ActionCode.ACTION_FAIL)
            {
                Assert.Fail();
            }

            var msnReply = await gateway.GetMsn(currentMsnReply.Sequence);
            if (msnReply.ActionCode == Enums.ActionCode.ACTION_FAIL)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestSellBetSerialization()
        {
            int[] selections = { 1 };
            var serializer = new BinarySerialization.BinarySerializer
            {
                Endianness = BinarySerialization.Endianness.Big
            };

            var req = new SellBetRequest()
            {
                RacecardDate = DateTime.UtcNow.ToString("ddMMyyyy"),
                BetCode = Enums.BetCode.WIN,
                BetId = 1,
                BetOption = Enums.BetOption.STRAIGHT,
                NumberOfSelections = (ushort)selections.Length,
                Selections = selections.Select(s => new Selection()
                {
                    MeetingNumber = 1,
                    RaceNumber = 1,
                    HorseNumber = (ushort)s,
                    IsBanker = 0
                }).ToList(),
                TotalStake = 100,
                UnitStake = 100
            };

            var stream = new MemoryStream();
            serializer.Serialize(stream, req);
            var buffer = stream.ToArray();
            Assert.AreEqual(buffer.Length, 57);
        }


        private void Gateway_OnReply(ReplyMessage obj)
        {
            //if (obj.GetType().IsAssignableFrom(typeof(AccountLoginSuccess)))
            //{
            //    gateway.GetRacecard(DateTime.UtcNow);
            //}
            //else if (obj.GetType().IsAssignableFrom(typeof(RacecardReply)))
            //{
            //    var reply = obj as RacecardReply;
            //    for (var i=1; i <= reply.NumMeetings; ++i)
            //    {
            //        gateway.GetMeeting(i);
            //    }
            //}
            //else if (obj.GetType().IsAssignableFrom(typeof(MeetingReply)))
            //{
            //    var reply = obj as MeetingReply;
            //    _logger.DebugFormat("Meeting #{0} - {1} [{2}] {3} raceas", reply.MeetingNumber, reply.MeetingName, reply.MeetingCode, reply.NumberOfRaces);
            //    for (var i=1; i <= reply.NumberOfRaces; ++i)
            //    {
            //        gateway.GetRace(reply.MeetingNumber, i);
            //    }
            //}
            //else if (obj.GetType().IsAssignableFrom(typeof(RaceReply)))
            //{
            //    var reply = obj as RaceReply;
            //    _logger.DebugFormat("M{0} R{1} @{2} {3} runners", reply.MeetingNumber, reply.RaceNumber, reply.RaceTime, reply.NumberOfDeclaredRunners);
            //    for (var i=1; i <= reply.NumberOfDeclaredRunners; ++i)
            //    {
            //        gateway.GetRunner(reply.MeetingNumber, reply.RaceNumber, i);
            //    }

            //}
            //else if (obj.GetType().IsAssignableFrom(typeof(RunnerReply)))
            //{
            //    var reply = obj as RunnerReply;

            //    _logger.DebugFormat("M{0} R{1} {2}.{3}", reply.MeetingNumber, reply.RaceNumber, reply.RunnerNumber, reply.RunnerName);
            //}
        }

        private void Gateway_OnUpdate(MessageBase obj)
        {
            _logger.DebugFormat("{0} received", obj.MessageType.ToString());
        }

        [TestMethod]
        public void TestCircularBuffer()
        {
            var cb = new CircularBuffer<byte>(65536);
            var randomStuff = new byte[61720];
            cb.Put(randomStuff);
            cb.Get(randomStuff.Length);
            cb.Put(new byte[3840]);

            while (cb.Size > 0)
            {
                cb.Peek(14);
                cb.Get(14);
            }

        }

        [TestMethod]
        public void TestLargeSerialization()
        {
            int[] selections = { 1 };
            var serializer = new BinarySerialization.BinarySerializer();
            serializer.Endianness = BinarySerialization.Endianness.Big;

            var req = new SellBetRequest()
            {
                RacecardDate = DateTime.UtcNow.ToString("ddMMyyyy"),
                BetCode = Enums.BetCode.WIN,
                BetId = 1,
                BetOption = Enums.BetOption.STRAIGHT,
                NumberOfSelections = (ushort)selections.Length,
                Selections = selections.Select(s => new Selection()
                {
                    MeetingNumber = 1,
                    RaceNumber = 1,
                    HorseNumber = (ushort)s,
                    IsBanker = 0
                }).ToList(),
                TotalStake = 500000,
                UnitStake = 500000
            };

            var stream = new MemoryStream();
            serializer.Serialize(stream, req);
            var buffer = stream.ToArray();
            Assert.AreEqual(buffer.Length, 57);
        }

    }
}
