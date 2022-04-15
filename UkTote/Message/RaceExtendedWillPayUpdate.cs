using System.Collections.Generic;
using BinarySerialization;

namespace UkTote.Message
{
    public class RaceExtendedWillPayUpdate : MessageBase
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort RaceNumber { get; set; }

        [FieldOrder(2)]
        public ushort PoolNumber { get; set; }

        [FieldOrder(3)]
        public uint TotalInvestment { get; set; }

        [FieldOrder(4)]
        public ushort NumberOfCombinations { get; set; }

        [FieldOrder(5)]
        [FieldCount("NumberOfCombinations")]
        public List<ushort> Declarations { get; set; }

        [FieldOrder(6)]
        [FieldCount("NumberOfCombinations")]
        public List<uint> CombinationTotal { get; set; }

        public RaceExtendedWillPayUpdate()
            : base(Enums.MessageType.RacePoolExtendedWillPayUpdateMsg)
        {

        }

        [Ignore]
        protected override ushort BodyLength => ((ushort)(12 + NumberOfCombinations * 8));
    }
}
