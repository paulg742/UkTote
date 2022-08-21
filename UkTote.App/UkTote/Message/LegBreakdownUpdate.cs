using BinarySerialization;
#if EIGHT_BYTE_MONEY
using money_t = System.UInt64;
#else
using money_t = System.UInt32;
#endif

namespace UkTote.Message
{
    public class LegBreakdownUpdate : MessageBase, IPoolUpdate
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort PoolNumber { get; set; }

        [FieldOrder(2)]
        public ushort LegNumber { get; set; }

        [FieldOrder(3)]
        public money_t MainStakeTotal { get; set; }

        [FieldOrder(4)]
        public money_t ConsolationStakeTotal { get; set; }

        [FieldOrder(5)]
        [FieldCount(41)]
        public List<money_t>? MainStakeBreakdowns { get; set; }

        [FieldOrder(6)]
        [FieldCount(41)]
        public List<money_t>? ConsolationStakeBreakdowns { get; set; }

        public LegBreakdownUpdate()
            : base(Enums.MessageType.LegBreakdownUpdateMsg)
        {

        }

        //[Ignore]
        //protected override ushort BodyLength => 342;
    }
}
