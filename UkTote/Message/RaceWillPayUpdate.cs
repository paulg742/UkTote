using System.Collections.Generic;
using BinarySerialization;
#if EIGHT_BYTE_MONEY
using money_t = System.UInt64;
#else
using money_t = System.UInt32;
#endif

namespace UkTote.Message
{
    public class RaceWillPayUpdate : MessageBase, IRacePoolUpdate
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
        public money_t TotalInvestment { get; set; }

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

        //[Ignore]
        //protected override ushort BodyLength => 628;
    }
}
