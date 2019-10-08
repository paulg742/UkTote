using BinarySerialization;

namespace UkTote.Message
{
    public class MeetingPayUpdate : MessageBase
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort Reserved { get; set; }

        public MeetingPayUpdate()
            : base(Enums.MessageType.MEETING_PAY_UPDATE_MSG)
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
