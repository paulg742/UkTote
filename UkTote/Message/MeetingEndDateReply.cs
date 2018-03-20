using BinarySerialization;

namespace UkTote.Message
{
    public class MeetingEndDateReply : ReplyMessage
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        [FieldLength(20)]
        [FieldEncoding("us-ascii")]
        public string MeetingName { get; set; }

        [FieldOrder(2)]
        [FieldLength(2)]
        [FieldEncoding("us-ascii")]
        public string MeetingCode { get; set; }

        [FieldOrder(3)]
        [FieldLength(8)]
        [FieldEncoding("us-ascii")]
        public string StartDate { get; set; }

        [FieldOrder(4)]
        [FieldLength(8)]
        [FieldEncoding("us-ascii")]
        public string EndDate { get; set; }

        [Ignore]
        protected override ushort BodyLength
        {
            get
            {
                return 40;
            }
        }
    }
}
