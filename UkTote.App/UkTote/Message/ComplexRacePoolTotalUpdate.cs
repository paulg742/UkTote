using BinarySerialization;
#if EIGHT_BYTE_MONEY
using money_t = System.UInt64;
#else
using money_t = System.UInt32;
#endif

namespace UkTote.Message
{
    public class ComplexRacePoolTotalUpdate : MessageBase, IRacePoolUpdate
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort RaceNumber { get; set; }

        [FieldOrder(2)]
        public ushort PoolNumber { get; set; }

        [FieldOrder(3)]
        public ushort NumberOfTotals { get; set; }

        [FieldOrder(4)]
        [FieldCount(10)]
        public List<money_t>? Totals { get; set; }

        [FieldOrder(5)]
        public money_t BonusPoolTotal { get; set; }

        public ComplexRacePoolTotalUpdate()
            : base(Enums.MessageType.ComplexRacePoolTotalUpdate)
        {

        }

        //[Ignore]
        //protected override ushort BodyLength => 52;
    }
}
