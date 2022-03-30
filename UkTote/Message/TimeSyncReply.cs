using BinarySerialization;

namespace UkTote.Message
{
    public class TimeSyncReply: ReplyMessage
    {
        [FieldOrder(0)]
        [FieldLength(8)]
        [FieldEncoding("us-ascii")]
        public string Date { get; set; }

        [FieldOrder(1)]
        [FieldLength(6)]
        [FieldEncoding("us-ascii")]
        public string Time { get; set; }

        [Ignore]
        protected override ushort BodyLength => 14;
    }
}
