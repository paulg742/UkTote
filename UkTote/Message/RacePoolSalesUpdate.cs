using BinarySerialization;

namespace UkTote.Message
{
    public class RacePoolSalesUpdate : MessageBase
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort RaceNumber { get; set; }

        [FieldOrder(2)]
        public ushort PoolNumber { get; set; }

        public RacePoolSalesUpdate()
            : base(Enums.MessageType.RACE_POOL_SALES_UPDATE)
        {

        }

        [Ignore]
        protected override ushort BodyLength
        {
            get
            {
                return 6;
            }
        }

    }
}
