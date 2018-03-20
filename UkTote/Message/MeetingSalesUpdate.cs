using BinarySerialization;

namespace UkTote.Message
{
    public class MeetingSalesUpdate : MessageBase
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort Reserved { get; set; }

        public MeetingSalesUpdate()
            : base(Enums.MessageType.MEETING_SALES_UPDATE)
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
