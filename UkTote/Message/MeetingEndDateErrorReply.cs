using BinarySerialization;

namespace UkTote.Message
{
    public class MeetingEndDateErrorReply : ReplyMessage
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        [FieldLength(2)]
        [FieldEncoding("us-ascii")]
        public string MeetingCode { get; set; }

        //[Ignore]
        //protected override ushort BodyLength => 4;
    }
}
