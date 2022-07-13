using BinarySerialization;

namespace UkTote.Message
{
    public class RacePoolUpdate : MessageBase, IRacePoolUpdate
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort RaceNumber { get; set; }

        [FieldOrder(2)]
        public ushort PoolNumber { get; set; }

        public RacePoolUpdate()
            : base(Enums.MessageType.RacePoolUpdateMsg)
        {

        }

        [Ignore]
        protected override ushort BodyLength => 6;
    }
}
