using BinarySerialization;

namespace UkTote.Message
{
    public class MeetingPoolSalesUpdate : MessageBase
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort PoolNumber { get; set; }

        public MeetingPoolSalesUpdate()
            : base(Enums.MessageType.MEETING_POOL_SALES_UPDATE)
        {

        }

        [Ignore]
        protected override ushort BodyLength
        {
            get
            {
                return 4;
            }
        }

    }
}
