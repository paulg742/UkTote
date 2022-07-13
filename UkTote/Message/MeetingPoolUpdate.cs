using BinarySerialization;

namespace UkTote.Message
{
    public class MeetingPoolUpdate : MessageBase, IPoolUpdate
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort PoolNumber { get; set; }

        public MeetingPoolUpdate()
            : base(Enums.MessageType.MeetingPoolUpdate)
        {

        }

        [Ignore]
        protected override ushort BodyLength => 4;
    }
}
