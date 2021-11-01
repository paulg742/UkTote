using BinarySerialization;

namespace UkTote.Message
{
    public class MeetingUpdate: MessageBase
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public Enums.Going Going { get; set; }

        public MeetingUpdate()
            : base(Enums.MessageType.MeetingUpdateMsg)
        {

        }

        [Ignore]
        protected override ushort BodyLength => 4;
    }
}
