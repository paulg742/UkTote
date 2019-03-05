using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UkTote;
using UkTote.Message;
using log4net;
using System.Collections.Generic;

namespace TBS.ARK.Tests
{
    [TestClass]
    public class UkToteBetTests
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(UkToteTests));

        const string host = "217.46.202.209";
        const int port = 8032;
        //const int port = 8036;    // HK port
        // const string username = "HKATCentrum02";
        // const string password = "password";
        const string username = "ATCentrum6";
        const string password = "password";
        ToteGateway gateway = new ToteGateway(120000);
        public UkToteBetTests()
        {
        }

        [TestInitialize]
        public void Setup()
        {
            log4net.Config.XmlConfigurator.Configure();
            var connected = gateway.Connect(host, port);
            Assert.IsTrue(connected);
            Login();
        }

        [TestCleanup]
        public void Teardown()
        {
            gateway.Stop();
        }

        private async void Login()
        {
            var loggedIn = await gateway.Login(username, password);
            Assert.IsTrue(loggedIn);
        }

        [TestMethod]
        public async Task TestSellWinBet()
        {
            var reply = await gateway.SellBet(DateTime.UtcNow, 1, 1, 1, 1, Enums.BetCode.WIN, Enums.BetOption.NO_OPTION, new[] { 1 });
            Assert.IsTrue(!string.IsNullOrEmpty(reply.TSN));
        }

        [TestMethod]
        public async Task TestSellToteDouble()
        {
            //var reply = await gateway.SellBet(DateTime.UtcNow, 1, 1, 1, Enums.BetCode.TOTEDOUBLE, Enums.BetOption.NO_OPTION, new[] { (1, 1), (2, 1) });
            var reply = await gateway.SellBet(DateTime.UtcNow, 1,  1, 200, 800, Enums.BetCode.TOTEDOUBLE, Enums.BetOption.NO_OPTION, new[] { 102, 104, 205, 207 }, 2);
            Assert.IsTrue(!string.IsNullOrEmpty(reply.TSN));
        }

        [TestMethod]
        public async Task TestSellPlaceBet()
        {
            var reply = await gateway.SellBet(DateTime.UtcNow, 1, 1, 200, 200, Enums.BetCode.PLACE, Enums.BetOption.NO_OPTION, new[] { 1 });
            Assert.IsTrue(!string.IsNullOrEmpty(reply.TSN));
        }

        [TestMethod]
        public async Task TestEachWayBet()
        {
            var reply = await gateway.SellBet(DateTime.UtcNow, 1, 1, 200, 400, Enums.BetCode.WIN, Enums.BetOption.EACH_WAY, new[] { 1 });
            Assert.IsTrue(!string.IsNullOrEmpty(reply.TSN));
        }

        [TestMethod]
        public async Task TestSwingerBet()
        {
            var reply = await gateway.SellBet(DateTime.UtcNow, 1, 1, 200, 200, Enums.BetCode.SWINGER, Enums.BetOption.STRAIGHT, new[] { 1, 2 });
            Assert.IsTrue(!string.IsNullOrEmpty(reply.TSN));
        }

        [TestMethod]
        public async Task TestPayEnquiry()
        {
            var reply = await gateway.PayEnquiry("0000C8293DD06674");
            Assert.IsTrue(reply.ErrorCode == Enums.ErrorCode.SUCCESS);
        }

        [TestMethod]
        public async Task RunBetfredBetPlacement()
        {
            gateway.NextBetId = 100;

            //Ascot   1   Win 5   5   1   No Option   1
            var betStr = "Ascot   1   Win 5   5   1   No Option   1";
            var reply = await gateway.SellBet(DateTime.UtcNow, 1, 1, 500, 500, Enums.BetCode.WIN, Enums.BetOption.NO_OPTION, new[] { 1 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Ascot   1   Win 2   2   1   No Option   4
            betStr = "Ascot   1   Win 2   2   1   No Option   4";
            reply = await gateway.SellBet(DateTime.UtcNow, 1, 1, 200, 200, Enums.BetCode.WIN, Enums.BetOption.NO_OPTION, new[] { 4 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Ascot   2   Win 10  10  1   No Option   29
            betStr = "//Ascot   2   Win 10  10  1   No Option   29";
            reply = await gateway.SellBet(DateTime.UtcNow, 1, 2, 1000, 1000, Enums.BetCode.WIN, Enums.BetOption.NO_OPTION, new[] { 29 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Ascot   3   Win 5   10  2   No Option   1,5
            betStr = "//Ascot   3   Win 5   10  2   No Option   1,5";
            reply = await gateway.SellBet(DateTime.UtcNow, 1, 3, 500, 1000, Enums.BetCode.WIN, Enums.BetOption.NO_OPTION, new[] { 1, 5 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Ascot   4   Win 2   4   2   No Option   10,17
            betStr = "//Ascot   4   Win 2   4   2   No Option   10,17";
            reply = await gateway.SellBet(DateTime.UtcNow, 1, 4, 200, 400, Enums.BetCode.WIN, Enums.BetOption.NO_OPTION, new[] { 10, 17 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Bath    1   Win 20  20  1   No Option   2
            betStr = "//Bath    1   Win 20  20  1   No Option   2";
            reply = await gateway.SellBet(DateTime.UtcNow, 2, 1, 2000, 2000, Enums.BetCode.WIN, Enums.BetOption.NO_OPTION, new[] { 2 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Bath    1   Place   5   5   1   No Option   9
            betStr = "//Bath    1   Place   5   5   1   No Option   9";
            reply = await gateway.SellBet(DateTime.UtcNow, 2, 1, 500, 500, Enums.BetCode.PLACE, Enums.BetOption.NO_OPTION, new[] { 9 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Ascot   6   Place   10  10  1   No Option   10
            betStr = "//Ascot   6   Place   10  10  1   No Option   10";
            reply = await gateway.SellBet(DateTime.UtcNow, 1, 6, 1000, 1000, Enums.BetCode.PLACE, Enums.BetOption.NO_OPTION, new[] { 10 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Ascot   2   Place   2   4   2   No Option   5,28
            betStr = "//Ascot   2   Place   2   4   2   No Option   5,28";
            reply = await gateway.SellBet(DateTime.UtcNow, 1, 2, 200, 400, Enums.BetCode.PLACE, Enums.BetOption.NO_OPTION, new[] { 5, 28 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Bath    5   Place   25  50  2   No Option   2,3
            betStr = "//Bath    5   Place   25  50  2   No Option   2,3";
            reply = await gateway.SellBet(DateTime.UtcNow, 2, 5, 2500, 5000, Enums.BetCode.PLACE, Enums.BetOption.NO_OPTION, new[] { 2, 3 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Bath    2   Eachway 2   4   2   Eachway 7
            betStr = "//Bath    2   Eachway 2   4   2   Eachway 7";
            reply = await gateway.SellBet(DateTime.UtcNow, 2, 2, 200, 400, Enums.BetCode.WIN, Enums.BetOption.EACH_WAY, new[] { 7 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Bath    3   Eachway 5   10  2   Eachway 10
            betStr = "//Bath    3   Eachway 5   10  2   Eachway 10";
            reply = await gateway.SellBet(DateTime.UtcNow, 2, 3, 500, 1000, Enums.BetCode.WIN, Enums.BetOption.EACH_WAY, new[] { 10 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Ascot   3   Eachway 20  40  2   Eachway 5
            betStr = "//Ascot   3   Eachway 20  40  2   Eachway 5";
            reply = await gateway.SellBet(DateTime.UtcNow, 1, 3, 2000, 4000, Enums.BetCode.WIN, Enums.BetOption.EACH_WAY, new[] { 5 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Ascot   2   Eachway 10  40  4   Eachway 5,27
            betStr = "//Ascot   2   Eachway 10  40  4   Eachway 5,27";
            reply = await gateway.SellBet(DateTime.UtcNow, 1, 2, 1000, 4000, Enums.BetCode.WIN, Enums.BetOption.EACH_WAY, new[] { 5, 27 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Bath    3   Eachway 2   8   4   Eachway 8,11
            betStr = "//Bath    3   Eachway 2   8   4   Eachway 8,11";
            reply = await gateway.SellBet(DateTime.UtcNow, 2, 3, 200, 800, Enums.BetCode.WIN, Enums.BetOption.EACH_WAY, new[] { 8, 11 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Bath    5   Eachway 2.5 5   2   Eachway 3
            betStr = "//Bath    5   Eachway 2.5 5   2   Eachway 3";
            reply = await gateway.SellBet(DateTime.UtcNow, 2, 5, 250, 500, Enums.BetCode.WIN, Enums.BetOption.EACH_WAY, new[] { 3 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Bath    7   Eachway 25  50  2   Eachway 10
            betStr = "//Bath    7   Eachway 25  50  2   Eachway 10";
            reply = await gateway.SellBet(DateTime.UtcNow, 2, 7, 2500, 5000, Enums.BetCode.WIN, Enums.BetOption.EACH_WAY, new[] { 10 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Ascot   6   Eachway 2   8   4   Eachway 2,9
            betStr = "//Ascot   6   Eachway 2   8   4   Eachway 2,9";
            reply = await gateway.SellBet(DateTime.UtcNow, 1, 6, 200, 800, Enums.BetCode.WIN, Enums.BetOption.EACH_WAY, new[] { 2, 9 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Ascot   4   Exacta  2   2   1   Straight    17,18
            betStr = "//Ascot   4   Exacta  2   2   1   Straight    17,18";
            reply = await gateway.SellBet(DateTime.UtcNow, 1, 4, 200, 200, Enums.BetCode.EXACTA, Enums.BetOption.STRAIGHT, new[] { 117, 218 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Bath    2   Exacta  10  10  1   Straight    6,7
            betStr = "//Bath    2   Exacta  10  10  1   Straight    6,7";
            reply = await gateway.SellBet(DateTime.UtcNow, 2, 2, 1000, 1000, Enums.BetCode.EXACTA, Enums.BetOption.STRAIGHT, new[] { 106, 207 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Bath    1   Exacta  8   8   1   Straight    9,10
            betStr = "//Bath    1   Exacta  8   8   1   Straight    9,10";
            reply = await gateway.SellBet(DateTime.UtcNow, 2, 1, 800, 800, Enums.BetCode.EXACTA, Enums.BetOption.STRAIGHT, new[] { 109, 210 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Bath    1   Exacta  5   10  2   Permutation 2,3
            betStr = "//Bath    1   Exacta  5   10  2   Permutation 2,3";
            reply = await gateway.SellBet(DateTime.UtcNow, 2, 1, 500, 1000, Enums.BetCode.EXACTA, Enums.BetOption.PERMUTATION, new[] { 2, 3 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Bath    2   Exacta  20  40  2   Permutation 6,7
            betStr = "//Bath    2   Exacta  20  40  2   Permutation 6,7";
            reply = await gateway.SellBet(DateTime.UtcNow, 2, 2, 2000, 4000, Enums.BetCode.EXACTA, Enums.BetOption.PERMUTATION, new[] { 6, 7 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Ascot   2   Exacta  25  50  2   Permutation 27,28
            betStr = "//Ascot   2   Exacta  25  50  2   Permutation 27,28";
            reply = await gateway.SellBet(DateTime.UtcNow, 1, 2, 2500, 5000, Enums.BetCode.EXACTA, Enums.BetOption.PERMUTATION, new[] { 27, 28 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Bath    1   Exacta  2   12  6   Permutation 2,3,4
            betStr = "//Bath    1   Exacta  2   12  6   Permutation 2,3,4";
            reply = await gateway.SellBet(DateTime.UtcNow, 2, 1, 200, 1200, Enums.BetCode.EXACTA, Enums.BetOption.PERMUTATION, new[] { 2, 3, 4 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Bath    3   Exacta  5   30  6   Permutation 8,9,10
            betStr = "//Bath    3   Exacta  5   30  6   Permutation 8,9,10";
            reply = await gateway.SellBet(DateTime.UtcNow, 2, 3, 500, 3000, Enums.BetCode.EXACTA, Enums.BetOption.PERMUTATION, new[] { 8, 9, 10 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Bath    1   Exacta  15  90  6   Permutation 8,9,10
            betStr = "//Bath    1   Exacta  15  90  6   Permutation 8,9,10";
            reply = await gateway.SellBet(DateTime.UtcNow, 2, 1, 1500, 9000, Enums.BetCode.EXACTA, Enums.BetOption.PERMUTATION, new[] { 8, 9, 10 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Ascot   2   Exacta  10  60  6   Permutation 4,5,28
            betStr = "//Ascot   2   Exacta  10  60  6   Permutation 4,5,28";
            reply = await gateway.SellBet(DateTime.UtcNow, 1, 2, 1000, 6000, Enums.BetCode.EXACTA, Enums.BetOption.PERMUTATION, new[] { 4, 5, 28 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Ascot   1   Exacta  2   4   2   Banker  901,2,3
            betStr = "//Ascot   1   Exacta  2   4   2   Banker  901,2,3";
            reply = await gateway.SellBet(DateTime.UtcNow, 1, 1, 200, 400, Enums.BetCode.EXACTA, Enums.BetOption.BANKER, new[] { 901, 2, 3 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Ascot   1   Exacta  4   8   2   Banker  902,1,3
            betStr = "//Ascot   1   Exacta  4   8   2   Banker  902,1,3";
            reply = await gateway.SellBet(DateTime.UtcNow, 1, 1, 400, 800, Enums.BetCode.EXACTA, Enums.BetOption.BANKER, new[] { 902, 1, 3 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Ascot   4   Exacta  7.5 15  2   Banker  917,17,18,19
            betStr = "//Ascot   4   Exacta  7.5 15  2   Banker  917,17,18,19";
            reply = await gateway.SellBet(DateTime.UtcNow, 1, 4, 750, 1500, Enums.BetCode.EXACTA, Enums.BetOption.BANKER, new[] { 917, 17, 18, 19 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Ascot   1   Trifecta    2   2   1   Straight    1,2,3
            betStr = "//Ascot   1   Trifecta    2   2   1   Straight    1,2,3";
            reply = await gateway.SellBet(DateTime.UtcNow, 1, 1, 200, 200, Enums.BetCode.TRIFECTA, Enums.BetOption.STRAIGHT, new[] { 101, 202, 303 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Ascot   1   Trifecta    5   5   1   Straight    3,4,5
            betStr = "//Ascot   1   Trifecta    5   5   1   Straight    3,4,5";
            reply = await gateway.SellBet(DateTime.UtcNow, 1, 1, 500, 500, Enums.BetCode.TRIFECTA, Enums.BetOption.STRAIGHT, new[] { 103, 204, 305 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Ascot   2   Trifecta    10  10  1   Straight    6,7,27
            betStr = "//Ascot   2   Trifecta    10  10  1   Straight    6,7,27";
            reply = await gateway.SellBet(DateTime.UtcNow, 1, 2, 1000, 1000, Enums.BetCode.TRIFECTA, Enums.BetOption.STRAIGHT, new[] { 106, 207, 327 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Bath    1   Trifecta    20  480 24  Permutation 1,2,3,5
            betStr = "//Bath    1   Trifecta    20  480 24  Permutation 1,2,3,5";
            reply = await gateway.SellBet(DateTime.UtcNow, 2, 1, 2000, 48000, Enums.BetCode.TRIFECTA, Enums.BetOption.PERMUTATION, new[] { 1, 2, 3, 5 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Bath    5   Trifecta    2   48  24  Permutation 2,4,6,8
            betStr = "//Bath    5   Trifecta    2   48  24  Permutation 2,4,6,8";
            reply = await gateway.SellBet(DateTime.UtcNow, 2, 5, 200, 4800, Enums.BetCode.TRIFECTA, Enums.BetOption.PERMUTATION, new[] { 2, 4, 6, 8 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Bath    1   Trifecta    6   36  6   Permutation 2,4,10
            betStr = "//Bath    1   Trifecta    6   36  6   Permutation 2,4,10";
            reply = await gateway.SellBet(DateTime.UtcNow, 2, 1, 600, 3600, Enums.BetCode.TRIFECTA, Enums.BetOption.PERMUTATION, new[] { 2, 4, 10 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Bath    1   Trifecta    10  240 24  Permutation 1,2,3,10
            betStr = "//Bath    1   Trifecta    10  240 24  Permutation 1,2,3,10";
            reply = await gateway.SellBet(DateTime.UtcNow, 2, 1, 1000, 24000, Enums.BetCode.TRIFECTA, Enums.BetOption.PERMUTATION, new[] { 1, 2, 3, 10 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Ascot   6   Trifecta    20  120 6   Banker  901,2,3,4
            betStr = "//Ascot   6   Trifecta    20  120 6   Banker  901,2,3,4";
            reply = await gateway.SellBet(DateTime.UtcNow, 1, 6, 2000, 12000, Enums.BetCode.TRIFECTA, Enums.BetOption.BANKER, new[] { 901, 2, 3, 4 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Ascot   6   Trifecta    5   30  6   Banker  902,1,3,4
            betStr = "//Ascot   6   Trifecta    5   30  6   Banker  902,1,3,4";
            reply = await gateway.SellBet(DateTime.UtcNow, 1, 6, 500, 3000, Enums.BetCode.TRIFECTA, Enums.BetOption.BANKER, new[] { 902, 1, 3, 4 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Bath    1   Trifecta    10  60  6   Banker  903,1,2,3,4
            betStr = "//Bath    1   Trifecta    10  60  6   Banker  903,1,2,3,4";
            reply = await gateway.SellBet(DateTime.UtcNow, 2, 1, 1000, 6000, Enums.BetCode.TRIFECTA, Enums.BetOption.BANKER, new[] { 903, 1, 2, 3, 4 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            // TODO - rod to get back on these trifecta floating banker ones

            //Ascot   1   Trifecta    2   36  18  Floating Banker 901,3,4,5
            betStr = "//Ascot   1   Trifecta    2   36  18  Floating Banker 901,3,4,5";
            reply = await gateway.SellBet(DateTime.UtcNow, 1, 1, 200, 3600, Enums.BetCode.TRIFECTA, Enums.BetOption.FLOATING_BANKER, new[] { 901, 3, 4, 5 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Ascot   1   Trifecta    12.5    225 18  Floating Banker 901,2,4,5
            betStr = "//Ascot   1   Trifecta    12.5    225 18  Floating Banker 901,2,4,5";
            reply = await gateway.SellBet(DateTime.UtcNow, 1, 1, 1250, 22500, Enums.BetCode.TRIFECTA, Enums.BetOption.FLOATING_BANKER, new[] { 901, 2, 4, 5 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Ascot   2   Trifecta    10  180 18  Floating Banker 904,4,5,6,7
            betStr = "//Ascot   2   Trifecta    10  180 18  Floating Banker 904,4,5,6,7";
            reply = await gateway.SellBet(DateTime.UtcNow, 1, 2, 1000, 18000, Enums.BetCode.TRIFECTA, Enums.BetOption.FLOATING_BANKER, new[] { 904, 4, 5, 6, 7 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Ascot   4   Swinger 20  20  1   Straight    17,19
            betStr = "//Ascot   4   Swinger 20  20  1   Straight    17,19";
            reply = await gateway.SellBet(DateTime.UtcNow, 1, 4, 2000, 2000, Enums.BetCode.SWINGER, Enums.BetOption.STRAIGHT, new[] { 17, 19 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Ascot   1   Swinger 10  10  1   Straight    2,6
            betStr = "//Ascot   1   Swinger 10  10  1   Straight    2,6";
            reply = await gateway.SellBet(DateTime.UtcNow, 1, 1, 1000, 1000, Enums.BetCode.SWINGER, Enums.BetOption.STRAIGHT, new[] { 2, 6 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Bath    1   Swinger 20  20  1   Straight    2,10
            betStr = "//Bath    1   Swinger 20  20  1   Straight    2,10";
            reply = await gateway.SellBet(DateTime.UtcNow, 2, 1, 2000, 2000, Enums.BetCode.SWINGER, Enums.BetOption.STRAIGHT, new[] { 2, 10 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Bath    3   Swinger 2   6   3   Permutation 10,11,13
            betStr = "//Bath    3   Swinger 2   6   3   Permutation 10,11,13";
            reply = await gateway.SellBet(DateTime.UtcNow, 2, 3, 200, 600, Enums.BetCode.SWINGER, Enums.BetOption.PERMUTATION, new[] { 10, 11, 13 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Bath    5   Swinger 50  150 3   Permutation 2,4,6
            betStr = "//Bath    5   Swinger 50  150 3   Permutation 2,4,6";
            reply = await gateway.SellBet(DateTime.UtcNow, 2, 5, 5000, 15000, Enums.BetCode.SWINGER, Enums.BetOption.PERMUTATION, new[] { 2, 4, 6 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Ascot   2   Swinger 2   6   3   Permutation 25,26,27
            betStr = "//Ascot   2   Swinger 2   6   3   Permutation 25,26,27";
            reply = await gateway.SellBet(DateTime.UtcNow, 1, 2, 200, 600, Enums.BetCode.SWINGER, Enums.BetOption.PERMUTATION, new[] { 25, 26, 27 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Bath    1   Swinger 20  60  3   Permutation 1,3,10
            betStr = "//Bath    1   Swinger 20  60  3   Permutation 1,3,10";
            reply = await gateway.SellBet(DateTime.UtcNow, 2, 1, 2000, 6000, Enums.BetCode.SWINGER, Enums.BetOption.PERMUTATION, new[] { 1, 3, 10 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Ascot   4   Swinger 2   6   3   Banker  918,16,17,19
            betStr = "//Ascot   4   Swinger 2   6   3   Banker  918,16,17,19";
            reply = await gateway.SellBet(DateTime.UtcNow, 1, 4, 200, 600, Enums.BetCode.SWINGER, Enums.BetOption.BANKER, new[] { 918, 16, 17, 19 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Ascot   4   Swinger 10  30  3   Banker  917,15,16,18
            betStr = "//Ascot   4   Swinger 10  30  3   Banker  917,15,16,18";
            reply = await gateway.SellBet(DateTime.UtcNow, 1, 4, 1000, 3000, Enums.BetCode.SWINGER, Enums.BetOption.BANKER, new[] { 917, 15, 16, 18 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Ascot   2   Swinger 12  24  2   Banker  905,4,5,6
            betStr = "//Ascot   2   Swinger 12  24  2   Banker  905,4,5,6";
            reply = await gateway.SellBet(DateTime.UtcNow, 1, 2, 1200, 2400, Enums.BetCode.SWINGER, Enums.BetOption.BANKER, new[] { 905, 4, 5, 6 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Jackpot 1   Jackpot 2   2   1   No Option   101,203,303,403,501,601
            betStr = "//Jackpot 1   Jackpot 2   2   1   No Option   101,203,303,403,501,601";
            reply = await gateway.SellBet(DateTime.UtcNow, 8, 1, 200, 200, Enums.BetCode.JACKPOT, Enums.BetOption.NO_OPTION, new[] { 101, 203, 303, 403, 501, 601 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Jackpot 1   Jackpot 4   128 32  No Option   101,102,203,205,301,303,402,403,501,502,601
            betStr = "//Jackpot 1   Jackpot 4   128 32  No Option   101,102,203,205,301,303,402,403,501,502,601";
            reply = await gateway.SellBet(DateTime.UtcNow, 8, 1, 400, 12800, Enums.BetCode.JACKPOT, Enums.BetOption.NO_OPTION, new[] { 101, 102, 203, 205, 301, 303, 402, 403, 501, 502, 601 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Jackpot 1   Jackpot 6   6   1   No Option   1FV,203,303,403,501,6FV
            betStr = "//Jackpot 1   Jackpot 6   6   1   No Option   1FV,203,303,403,501,6FV";
            reply = await gateway.SellBet(DateTime.UtcNow, 8, 1, 600, 600, Enums.BetCode.JACKPOT, Enums.BetOption.NO_OPTION, new[] { 141, 203, 303, 403, 501, 641 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Jackpot 1   Jackpot 8   256 32  No Option   101,102,203,2FV,301,303,403,4FV,501,502,6FV
            betStr = "//Jackpot 1   Jackpot 8   256 32  No Option   101,102,203,2FV,301,303,403,4FV,501,502,6FV";
            reply = await gateway.SellBet(DateTime.UtcNow, 8, 1, 800, 25600, Enums.BetCode.JACKPOT, Enums.BetOption.NO_OPTION, new[] { 101, 102, 203, 241, 301, 303, 403, 441, 501, 502, 641 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Bath    1   Placepot    10  10  1   No Option   102,205,311,401,502,601
            betStr = "//Bath    1   Placepot    10  10  1   No Option   102,205,311,401,502,601";
            reply = await gateway.SellBet(DateTime.UtcNow, 2, 1, 1000, 1000, Enums.BetCode.PLACEPOT, Enums.BetOption.NO_OPTION, new[] { 102, 205, 311, 401, 502, 601 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Bath    1   Placepot    2   64  32  No Option   102,104,205,207,311,320,401,402,502,504,601
            betStr = "//Bath    1   Placepot    2   64  32  No Option   102,104,205,207,311,320,401,402,502,504,601";
            reply = await gateway.SellBet(DateTime.UtcNow, 2, 1, 200, 6400, Enums.BetCode.PLACEPOT, Enums.BetOption.NO_OPTION, new[] { 102, 104, 205, 207, 311, 320, 401, 402, 502, 504, 601 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Bath    1   Placepot    4   4   1   No Option   1FV,205,3FV,401,5FV,601
            betStr = "//Bath    1   Placepot    4   4   1   No Option   1FV,205,3FV,401,5FV,601";
            reply = await gateway.SellBet(DateTime.UtcNow, 2, 1, 400, 400, Enums.BetCode.PLACEPOT, Enums.BetOption.NO_OPTION, new[] { 141, 205, 341, 401, 541, 601 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Bath    1   Placepot    6   48  8   No Option   102,1FV,205,311,3FV,401,502,5FV,601
            betStr = "//Bath    1   Placepot    6   48  8   No Option   102,1FV,205,311,3FV,401,502,5FV,601";
            reply = await gateway.SellBet(DateTime.UtcNow, 2, 1, 600, 4800, Enums.BetCode.PLACEPOT, Enums.BetOption.NO_OPTION, new[] { 102, 141, 205, 311, 341, 401, 502, 541, 601 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Goodwood    1   Placepot    8   8   1   No Option   110,211,306,410,508,612
            betStr = "//Goodwood    1   Placepot    8   8   1   No Option   110,211,306,410,508,612";
            reply = await gateway.SellBet(DateTime.UtcNow, 5, 1, 800, 800, Enums.BetCode.PLACEPOT, Enums.BetOption.NO_OPTION, new[] { 110, 211, 306, 410, 508, 612 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Ascot   3   Quadpot 25  25  1   No Option   301,418,501,602
            betStr = "//Ascot   3   Quadpot 25  25  1   No Option   301,418,501,602";
            reply = await gateway.SellBet(DateTime.UtcNow, 1, 3, 2500, 2500, Enums.BetCode.QUADPOT, Enums.BetOption.NO_OPTION, new[] { 301, 418, 501, 602 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Ascot   3   Quadpot 10  80  8   No Option   301,302,417,418,501,601,602
            betStr = "//Ascot   3   Quadpot 10  80  8   No Option   301,302,417,418,501,601,602";
            reply = await gateway.SellBet(DateTime.UtcNow, 1, 3, 1000, 8000, Enums.BetCode.QUADPOT, Enums.BetOption.NO_OPTION, new[] { 301, 302, 417, 418, 501, 601, 602 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Ascot   3   Quadpot 4   4   1   No Option   3FV,418,5FV,602
            betStr = "//Ascot   3   Quadpot 4   4   1   No Option   3FV,418,5FV,602";
            reply = await gateway.SellBet(DateTime.UtcNow, 1, 3, 400, 400, Enums.BetCode.QUADPOT, Enums.BetOption.NO_OPTION, new[] { 341, 418, 541, 602 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Ascot   3   Quadpot 2   16  8   No Option   301,3FV,417,4FV,501,601,6FV
            betStr = "//Ascot   3   Quadpot 2   16  8   No Option   301,3FV,417,4FV,501,601,6FV";
            reply = await gateway.SellBet(DateTime.UtcNow, 1, 3, 200, 1600, Enums.BetCode.QUADPOT, Enums.BetOption.NO_OPTION, new[] { 301, 341, 417, 441, 501, 601, 641 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Goodwood    3   Quadpot 2   2   1   No Option   306,410,508,612
            betStr = "//Goodwood    3   Quadpot 2   2   1   No Option   306,410,508,612";
            reply = await gateway.SellBet(DateTime.UtcNow, 5, 3, 200, 200, Enums.BetCode.QUADPOT, Enums.BetOption.NO_OPTION, new[] { 306, 410, 508, 612 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Scoop 6 1   Scoop6  2   2   1   No Option   104,211,317,401,501,611
            betStr = "//Scoop 6 1   Scoop6  2   2   1   No Option   104,211,317,401,501,611";
            reply = await gateway.SellBet(DateTime.UtcNow, 9, 1, 200, 200, Enums.BetCode.SCOOP6, Enums.BetOption.NO_OPTION, new[] { 104, 211, 317, 401, 501, 611 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Scoop 6 1   Scoop6  2   64  32  No Option   102,104,211,212,310,317,401,403,501,510,611
            betStr = "//Scoop 6 1   Scoop6  2   64  32  No Option   102,104,211,212,310,317,401,403,501,510,611";
            reply = await gateway.SellBet(DateTime.UtcNow, 9, 1, 200, 6400, Enums.BetCode.SCOOP6, Enums.BetOption.NO_OPTION, new[] { 102, 104, 211, 212, 310, 317, 401, 403, 501, 510, 611 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Scoop 6 1   Scoop6  2   2   1   No Option   1FV,211,3FV,401,5FV,611
            betStr = "//Scoop 6 1   Scoop6  2   2   1   No Option   1FV,211,3FV,401,5FV,611";
            reply = await gateway.SellBet(DateTime.UtcNow, 9, 1, 200, 200, Enums.BetCode.SCOOP6, Enums.BetOption.NO_OPTION, new[] { 141, 211, 341, 401, 541, 611 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);

            //Scoop 6 1   Scoop6  2   64  32  No Option   104,1FV,212,2FV,317,3FV,401,403,501,5FV,611
            betStr = "//Scoop 6 1   Scoop6  2   64  32  No Option   104,1FV,212,2FV,317,3FV,401,403,501,5FV,611";
            reply = await gateway.SellBet(DateTime.UtcNow, 9, 1, 200, 6400, Enums.BetCode.SCOOP6, Enums.BetOption.NO_OPTION, new[] { 104, 141, 212, 241, 317, 341, 401, 403, 501, 541, 611 });
            _logger.InfoFormat("{0} : {1} : {2} : {3}", reply.TSN, reply.BetId, betStr, reply.ErrorText);
        }

        [TestMethod]
        public async Task RunHkBatchTest()
        {
            gateway.NextBetId = 400;

            var batch = new List<BetRequest>()
            {
                //HK_Tote039	Happy Valley	1	Double Trio	1	40	40	Permutation	3,10,6,7,8 / 4,2,9,12		Permutation - Void = 0, Paid <> 0 (win)							
                new BetRequest(DateTime.UtcNow, 1, 100, 4000, Enums.BetCode.DOUBLETRIO, Enums.BetOption.PERMUTATION, new[] { (1, 3), (1, 10), (1, 6), (1, 7), (1, 8), (2, 4), (2, 2), (2, 9), (2, 12) }),
                //HK_Tote040	Happy Valley	1	Double Trio	1	24	24	Banker	901,902,7,8,9 / 905,903,4,5,6,7,8,9,10,11	9nn = banker	Losing Bet							
                new BetRequest(DateTime.UtcNow, 1, 100, 2400, Enums.BetCode.DOUBLETRIO, Enums.BetOption.BANKER, new[] { (1, 901), (1, 902), (1, 7), (1, 8), (1, 9), (2, 905), (2, 903), (2, 4), (2, 5), (2, 6), (2, 7), (2, 8), (2, 9), (2, 10), (2, 11) }), 
                //HK_Tote041	Happy Valley	1	Double Trio	10	10	1	Normal	3,6,10 / 2,4,9		Winning bet  							

            };

            var reply = await gateway.SellBatch(batch);

            Assert.AreEqual(reply.Count(), batch.Count);
        }


        [TestMethod]
        public async Task RunBatchTest()
        {
            gateway.NextBetId = 300;

            var batch = new List<BetRequest>();

            batch.Add(new BetRequest(DateTime.UtcNow, 1, 1, 500, 500, Enums.BetCode.WIN, Enums.BetOption.NO_OPTION, new[] { 1 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 1, 1, 200, 200, Enums.BetCode.WIN, Enums.BetOption.NO_OPTION, new[] { 4 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 1, 2, 1000, 1000, Enums.BetCode.WIN, Enums.BetOption.NO_OPTION, new[] { 29 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 1, 3, 500, 1000, Enums.BetCode.WIN, Enums.BetOption.NO_OPTION, new[] { 1, 5 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 1, 4, 200, 400, Enums.BetCode.WIN, Enums.BetOption.NO_OPTION, new[] { 10, 17 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 2, 1, 2000, 2000, Enums.BetCode.WIN, Enums.BetOption.NO_OPTION, new[] { 2 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 2, 1, 500, 500, Enums.BetCode.PLACE, Enums.BetOption.NO_OPTION, new[] { 9 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 1, 6, 1000, 1000, Enums.BetCode.PLACE, Enums.BetOption.NO_OPTION, new[] { 10 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 1, 2, 200, 400, Enums.BetCode.PLACE, Enums.BetOption.NO_OPTION, new[] { 5, 28 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 2, 5, 2500, 5000, Enums.BetCode.PLACE, Enums.BetOption.NO_OPTION, new[] { 2, 3 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 2, 2, 200, 400, Enums.BetCode.WIN, Enums.BetOption.EACH_WAY, new[] { 7 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 2, 3, 500, 1000, Enums.BetCode.WIN, Enums.BetOption.EACH_WAY, new[] { 10 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 1, 3, 2000, 4000, Enums.BetCode.WIN, Enums.BetOption.EACH_WAY, new[] { 5 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 1, 2, 1000, 4000, Enums.BetCode.WIN, Enums.BetOption.EACH_WAY, new[] { 5, 27 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 2, 3, 200, 800, Enums.BetCode.WIN, Enums.BetOption.EACH_WAY, new[] { 8, 11 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 2, 5, 250, 500, Enums.BetCode.WIN, Enums.BetOption.EACH_WAY, new[] { 3 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 2, 7, 2500, 5000, Enums.BetCode.WIN, Enums.BetOption.EACH_WAY, new[] { 10 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 1, 6, 200, 800, Enums.BetCode.WIN, Enums.BetOption.EACH_WAY, new[] { 2, 9 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 1, 4, 200, 200, Enums.BetCode.EXACTA, Enums.BetOption.STRAIGHT, new[] { 117, 218 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 2, 2, 1000, 1000, Enums.BetCode.EXACTA, Enums.BetOption.STRAIGHT, new[] { 106, 207 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 2, 1, 800, 800, Enums.BetCode.EXACTA, Enums.BetOption.STRAIGHT, new[] { 109, 210 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 2, 1, 500, 1000, Enums.BetCode.EXACTA, Enums.BetOption.PERMUTATION, new[] { 2, 3 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 2, 2, 2000, 4000, Enums.BetCode.EXACTA, Enums.BetOption.PERMUTATION, new[] { 6, 7 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 1, 2, 2500, 5000, Enums.BetCode.EXACTA, Enums.BetOption.PERMUTATION, new[] { 27, 28 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 2, 1, 200, 1200, Enums.BetCode.EXACTA, Enums.BetOption.PERMUTATION, new[] { 2, 3, 4 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 2, 3, 500, 3000, Enums.BetCode.EXACTA, Enums.BetOption.PERMUTATION, new[] { 8, 9, 10 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 2, 1, 1500, 9000, Enums.BetCode.EXACTA, Enums.BetOption.PERMUTATION, new[] { 8, 9, 10 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 1, 2, 1000, 6000, Enums.BetCode.EXACTA, Enums.BetOption.PERMUTATION, new[] { 4, 5, 28 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 1, 1, 200, 400, Enums.BetCode.EXACTA, Enums.BetOption.BANKER, new[] { 901, 2, 3 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 1, 1, 400, 800, Enums.BetCode.EXACTA, Enums.BetOption.BANKER, new[] { 902, 1, 3 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 1, 4, 750, 1500, Enums.BetCode.EXACTA, Enums.BetOption.BANKER, new[] { 917, 17, 18, 19 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 1, 1, 200, 200, Enums.BetCode.TRIFECTA, Enums.BetOption.STRAIGHT, new[] { 101, 202, 303 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 1, 1, 500, 500, Enums.BetCode.TRIFECTA, Enums.BetOption.STRAIGHT, new[] { 103, 204, 305 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 1, 2, 1000, 1000, Enums.BetCode.TRIFECTA, Enums.BetOption.STRAIGHT, new[] { 106, 207, 327 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 2, 1, 2000, 48000, Enums.BetCode.TRIFECTA, Enums.BetOption.PERMUTATION, new[] { 1, 2, 3, 5 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 2, 5, 200, 4800, Enums.BetCode.TRIFECTA, Enums.BetOption.PERMUTATION, new[] { 2, 4, 6, 8 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 2, 1, 600, 3600, Enums.BetCode.TRIFECTA, Enums.BetOption.PERMUTATION, new[] { 2, 4, 10 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 2, 1, 1000, 24000, Enums.BetCode.TRIFECTA, Enums.BetOption.PERMUTATION, new[] { 1, 2, 3, 10 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 1, 6, 2000, 12000, Enums.BetCode.TRIFECTA, Enums.BetOption.BANKER, new[] { 901, 2, 3, 4 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 1, 6, 500, 3000, Enums.BetCode.TRIFECTA, Enums.BetOption.BANKER, new[] { 902, 1, 3, 4 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 2, 1, 1000, 6000, Enums.BetCode.TRIFECTA, Enums.BetOption.BANKER, new[] { 903, 1, 2, 3, 4 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 1, 1, 200, 3600, Enums.BetCode.TRIFECTA, Enums.BetOption.FLOATING_BANKER, new[] { 901, 3, 4, 5 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 1, 1, 1250, 22500, Enums.BetCode.TRIFECTA, Enums.BetOption.FLOATING_BANKER, new[] { 901, 2, 4, 5 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 1, 2, 1000, 18000, Enums.BetCode.TRIFECTA, Enums.BetOption.FLOATING_BANKER, new[] { 904, 4, 5, 6, 7 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 1, 4, 2000, 2000, Enums.BetCode.SWINGER, Enums.BetOption.STRAIGHT, new[] { 17, 19 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 1, 1, 1000, 1000, Enums.BetCode.SWINGER, Enums.BetOption.STRAIGHT, new[] { 2, 6 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 2, 1, 2000, 2000, Enums.BetCode.SWINGER, Enums.BetOption.STRAIGHT, new[] { 2, 10 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 2, 3, 200, 600, Enums.BetCode.SWINGER, Enums.BetOption.PERMUTATION, new[] { 10, 11, 13 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 2, 5, 5000, 15000, Enums.BetCode.SWINGER, Enums.BetOption.PERMUTATION, new[] { 2, 4, 6 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 1, 2, 200, 600, Enums.BetCode.SWINGER, Enums.BetOption.PERMUTATION, new[] { 25, 26, 27 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 2, 1, 2000, 6000, Enums.BetCode.SWINGER, Enums.BetOption.PERMUTATION, new[] { 1, 3, 10 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 1, 4, 200, 600, Enums.BetCode.SWINGER, Enums.BetOption.BANKER, new[] { 918, 16, 17, 19 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 1, 4, 1000, 3000, Enums.BetCode.SWINGER, Enums.BetOption.BANKER, new[] { 917, 15, 16, 18 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 1, 2, 1200, 2400, Enums.BetCode.SWINGER, Enums.BetOption.BANKER, new[] { 905, 4, 5, 6 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 8, 1, 200, 200, Enums.BetCode.JACKPOT, Enums.BetOption.NO_OPTION, new[] { 101, 203, 303, 403, 501, 601 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 8, 1, 400, 12800, Enums.BetCode.JACKPOT, Enums.BetOption.NO_OPTION, new[] { 101, 102, 203, 205, 301, 303, 402, 403, 501, 502, 601 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 8, 1, 600, 600, Enums.BetCode.JACKPOT, Enums.BetOption.NO_OPTION, new[] { 141, 203, 303, 403, 501, 641 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 8, 1, 800, 25600, Enums.BetCode.JACKPOT, Enums.BetOption.NO_OPTION, new[] { 101, 102, 203, 241, 301, 303, 403, 441, 501, 502, 641 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 2, 1, 1000, 1000, Enums.BetCode.PLACEPOT, Enums.BetOption.NO_OPTION, new[] { 102, 205, 311, 401, 502, 601 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 2, 1, 200, 6400, Enums.BetCode.PLACEPOT, Enums.BetOption.NO_OPTION, new[] { 102, 104, 205, 207, 311, 320, 401, 402, 502, 504, 601 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 2, 1, 400, 400, Enums.BetCode.PLACEPOT, Enums.BetOption.NO_OPTION, new[] { 141, 205, 341, 401, 541, 601 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 2, 1, 600, 4800, Enums.BetCode.PLACEPOT, Enums.BetOption.NO_OPTION, new[] { 102, 141, 205, 311, 341, 401, 502, 541, 601 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 5, 1, 800, 800, Enums.BetCode.PLACEPOT, Enums.BetOption.NO_OPTION, new[] { 110, 211, 306, 410, 508, 612 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 1, 3, 2500, 2500, Enums.BetCode.QUADPOT, Enums.BetOption.NO_OPTION, new[] { 301, 418, 501, 602 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 1, 3, 1000, 8000, Enums.BetCode.QUADPOT, Enums.BetOption.NO_OPTION, new[] { 301, 302, 417, 418, 501, 601, 602 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 1, 3, 400, 400, Enums.BetCode.QUADPOT, Enums.BetOption.NO_OPTION, new[] { 341, 418, 541, 602 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 1, 3, 200, 1600, Enums.BetCode.QUADPOT, Enums.BetOption.NO_OPTION, new[] { 301, 341, 417, 441, 501, 601, 641 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 5, 3, 200, 200, Enums.BetCode.QUADPOT, Enums.BetOption.NO_OPTION, new[] { 306, 410, 508, 612 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 9, 1, 200, 200, Enums.BetCode.SCOOP6, Enums.BetOption.NO_OPTION, new[] { 104, 211, 317, 401, 501, 611 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 9, 1, 200, 6400, Enums.BetCode.SCOOP6, Enums.BetOption.NO_OPTION, new[] { 102, 104, 211, 212, 310, 317, 401, 403, 501, 510, 611 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 9, 1, 200, 200, Enums.BetCode.SCOOP6, Enums.BetOption.NO_OPTION, new[] { 141, 211, 341, 401, 541, 611 }));
            batch.Add(new BetRequest(DateTime.UtcNow, 9, 1, 200, 6400, Enums.BetCode.SCOOP6, Enums.BetOption.NO_OPTION, new[] { 104, 141, 212, 241, 317, 341, 401, 403, 501, 541, 611 }));

            var reply = await gateway.SellBatch(batch);

            Assert.AreEqual(reply.Count(), batch.Count);
        }

        [TestMethod]
        public async Task RunBetfredPaymentRequest()
        {
            var tsns = new[]
            {
                "0000B82A89B06674",
                "0000AC2A89B86674",
                "0000642A89C06674",
                "0000742A89C86674",
                "00004C2A89D26674",
                "00005C2A89DA6674",
                "0000F82A8A8A6674",
                "0000D42A8A926674",
                "0000D02A8AEE6674",
                "0000E42A8AF86674",
                "0000442A8B286674",
                "0000782A8B306674",
                "00006C2A8B386674",
                "0000982A8B426674",
                "0000B42A8B4A6674",
                "0000842A8B526674",
                "0000902A8B5A6674",
                "0000E02A8B646674",
                "0000F02A8B6C6674",
                "0000C42A8B746674",
                "0000D02A8B7E6674",
                "00003C2A8B866674",
                "0000102A8B906674",
                "0000042A8B986674",
                "0000702A8BA26674",
                "00006C2A8BAA6674",
                "0000542A8BB46674",
                "0000FC2A8BE06674",
                "0000942A8BEA6674",
                "0000A02A8BF26674",
                "0000BC2A8BFC6674",
                "0000402A8B046674",
                "0000782A8C186674",
                "0000D02A8C4E6674",
                "0000FC2A8C586674",
                "0000902A8C606674",
                "0000802A8C6A6674",
                "0000BC2A8C746674",
                "0000A02A8C7C6674",
                "0000542A8C866674",
                "00007C2A8C906674",
                "0000DC2A8D5A6674",
                "0000A42A8D646674",
                "0000A82A8D6C6674",
                "0000942A8D766674",
                "0000802A8D7E6674",
                "0000902A8D886674",
                "0000A42A8D906674",
                "0000302A8DDA6674",
                "00004C2A8DE26674",
                "0000AC2A8D106674",
                "0000A82A8E1C6674",
                "0000D82A8E266674",
                "0000F82A8E306674",
                "0000E02A8E3C6674",
                "0000482A8E6A6674",
                "0000742A8E746674",
                "0000A02A8E806674",
                "0000BC2A8E8A6674",
                "0000842A8E946674",
                "00008C2A8E9E6674",
                "0000E42A8EA86674",
                "0000D02A8EB26674",
                "0000782A8EE06674",
                "0000682A8EEA6674",
                "00002C2A8EF66674"
            };

            foreach (var tsn in tsns)
            {
                var reply = await gateway.PayEnquiry(tsn);
                _logger.InfoFormat("{0}\t{1}\t{2}\t{3}", reply.TSN, reply.PayoutAmount, reply.VoidAmount, reply.ErrorText);
            }
        }
    }
}
