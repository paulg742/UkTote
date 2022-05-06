using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using UkTote.Message;

namespace UkTote.Tests
{
    [TestClass]
    public class SerializationTests
    {
        [TestMethod]
        public void TestExtendedWillPaySerialization()
        {
            var packet = new RaceExtendedWillPayUpdate
            {
                NumberOfCombinations = 3,
                Declarations = new List<uint>(),
                CombinationTotal = new List<uint>()
            };
            var c1 = 1 << 16 | 2;
            var c2 = 1 << 16 | 3;
            var c3 = 1 << 16 | 4;
            packet.Declarations.Add((uint)c1);
            packet.Declarations.Add((uint)c2);
            packet.Declarations.Add((uint)c3);
            packet.CombinationTotal.Add(100);
            packet.CombinationTotal.Add(200);
            packet.CombinationTotal.Add(300);

            Assert.IsNotNull(packet);
        }
    }
}
