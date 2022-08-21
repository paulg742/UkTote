using BinarySerialization;
#if EIGHT_BYTE_MONEY
using money_t = System.UInt64;
#else
using money_t = System.UInt32;
#endif

namespace UkTote.Message
{
    public class MeetingPoolDividendUpdate : MessageBase, IPoolUpdate
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort PoolNumber { get; set; }

        [FieldOrder(2)]
        public uint MainDividend { get; set; }

        [FieldOrder(3)]
        public uint ConsolationDividend{ get; set; }

        [FieldOrder(4)]
        public uint Reserved1 { get; set; }

        [FieldOrder(5)]
        public uint Reserved2 { get; set; }

        [FieldOrder(6)]
        public money_t CarriedForwardAmount { get; set; }

        [FieldOrder(7)]
        public money_t BonusPoolAmount { get; set; }

        [FieldOrder(8)]
        public money_t Reserved3 { get; set; }

        [FieldOrder(9)]
        public money_t Reserved4 { get; set; }

        [FieldOrder(10)]
        public money_t Reserved5 { get; set; }

        [FieldOrder(11)]
        public money_t Reserved6 { get; set; }

        [FieldOrder(12)]
        public ushort NumberLegs { get; set; }

        [FieldOrder(13)]
        [FieldCount("NumberLegs")]
        public List<MeetingPoolCombination>? CombinationMap { get; set; }

        public MeetingPoolDividendUpdate()
            : base(Enums.MessageType.MeetingPoolDivUpdateMsg)
        {

        }

        protected override ushort BodyLength => (ushort)(base.BodyLength + NumberLegs * Size.Of(typeof(MeetingPoolCombination)));
        //[Ignore]
        //protected override ushort BodyLength => 1276;
    }
}
