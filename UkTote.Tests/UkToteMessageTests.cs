using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UkTote;
using UkTote.Message;

namespace UkTote.Tests
{
    [TestClass]
    public class UkToteMessageTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var x = new CurrentBalanceReply();
            Assert.AreEqual(Size.Of(x), 4);
        }

        [TestMethod]
        public void TestAllMessagesSizes()
        {
            var typeSizeMap = new Dictionary<string, int>()
            {
                {"AccountLoginError", 20},
                {"AccountLoginRequest", 40},
                {"AccountLoginSuccess", 20},
                {"AccountLogoutError", 20},
                {"AccountLogoutRequest", 20},
                {"AccountLogoutSuccess", 20},
                {"ComplexRacePoolDividendUpdate", 216},
                {"ComplexRacePoolTotalUpdate", 52},
                {"CurrentBalanceReply", 30},
                {"CurrentBalanceRequest", 0},
                {"CurrentMsnReply", 0},
                {"CurrentMsnRequest", 0},
                {"EndOfRacingUpdate", 0},
                {"Header", 0},
                {"LegBreakdownUpdate", 342},
                {"MatrixPoolDividendUpdate", 774},
                {"MeetingEndDateErrorReply", 4},
                {"MeetingEndDateReply", 40},
                {"MeetingEndDateRequest", 2},
                {"MeetingPayUpdate", 4},
                {"MeetingPoolDividendUpdate", 46},
                {"MeetingPoolPayUpdate", 4},
                {"MeetingPoolReply", 72},
                {"MeetingPoolRequest", 4},
                {"MeetingPoolSalesUpdate", 4},
                {"MeetingPoolTotalUpdate", 40},
                {"MeetingPoolUpdate", 4},
                {"MeetingPoolWillPayUpdate", 10},
                {"MeetingReply", 41},
                {"MeetingRequest", 2},
                {"MeetingSalesUpdate", 4},
                {"MeetingUpdate", 4},
                {"MsnReply", 0},
                {"MsnRequest", 0},
                {"PayEnquiryFailed", 80},
                {"PayEnquiryRequest", 18},
                {"PayEnquirySuccess", 26},
                {"PoolSubstituteUpdate", 88},
                {"RacecardReply", 74},
                {"RacecardRequest", 8},
                //{"RaceExtendedWillPayUpdate", ((ushort)(12 + NumberOfCombinations * 8))},
                {"RacePayUpdate", 6},
                {"RacePoolDividendUpdate", 260},
                {"RacePoolPayUpdate", 6},
                {"RacePoolReply", 42},
                {"RacePoolRequest", 6},
                {"RacePoolSalesUpdate", 6},
                {"RacePoolUpdate", 6},
                {"RaceReply", 88},
                {"RaceRequest", 4},
                {"RaceSalesUpdate", 6},
                {"RaceUpdate", 6},
                {"RaceWillPayUpdate", 614},
                {"ReplyMessage", 0},
                {"ResultUpdate", 130},
                {"RunnerReply", 48},
                {"RunnerRequest", 6},
                {"RunnerUpdate", 106},
                {"RuOkReply", 0},
                {"RuOkRequest", 40},
                {"SellBetFailed", 66},
                //{"SellBetRequest", (ushort) (30 + NumberOfSelections * 13)},
                {"SellBetSuccess", 22},
                {"SingleMsnReply", 0},
                {"SingleMsnRequest", 0},
                {"SubstituteUpdate", 6},
                {"SuperComplexPoolDividendUpdate", 92},
                {"TimeSyncReply", 14},
                {"TimeSyncRequest", 0},
                {"WeighedInUpdate", 6}
            };
            
            var executingAssembly = Assembly.GetExecutingAssembly();
            foreach (var referencedAssembly in executingAssembly.GetReferencedAssemblies())
            {
                var assembly = Assembly.Load(referencedAssembly);
                foreach (var type in assembly.GetTypes().Where(t => t.IsClass && t.IsSubclassOf(typeof(MessageBase))))
                {
                    var size = Size.Of(type);

                    if (typeSizeMap.ContainsKey(type.Name))
                    {
                        Assert.AreEqual(typeSizeMap[type.Name], size);
                    }
                }
            }
        }
    }
}
