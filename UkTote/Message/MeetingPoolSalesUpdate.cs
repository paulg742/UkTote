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
            : base(Enums.MessageType.MeetingPoolSalesUpdate)
        {

        }

        [Ignore]
        protected override ushort BodyLength => 4;
    }
}
