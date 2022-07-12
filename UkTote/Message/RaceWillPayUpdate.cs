using System.Collections.Generic;
using BinarySerialization;

namespace UkTote.Message
{
    public class RaceWillPayUpdate : MessageBase, IRaceUpdate
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort RaceNumber { get; set; }

        [FieldOrder(2)]
        public ushort PoolNumber { get; set; }

        [FieldOrder(3)]
        public ushort Reserved { get; set; }

        [FieldOrder(4)]
        public uint TotalInvestment { get; set; }

        [FieldOrder(5)]
        public ushort NumberOfCombinations { get; set; }

        [FieldOrder(6)]
        [FieldCount(100)]
        public List<ushort> CombinationNumber { get; set; }

        [FieldOrder(7)]
        [FieldCount(100)]
        public List<ushort> DeclarationNumber { get; set; }

        [FieldOrder(8)]
        [FieldCount(50)]
        public List<uint> CombinationTotal { get; set; }

        public RaceWillPayUpdate()
            : base(Enums.MessageType.RacePoolWillPayUpdateMsg)
        {

        }

        [Ignore]
        protected override ushort BodyLength => 628;
    }
}
