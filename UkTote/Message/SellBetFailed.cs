using BinarySerialization;

namespace UkTote.Message
{
    public class SellBetFailed : ReplyMessage
    {
        [FieldOrder(0)]
        public Enums.ErrorCode ErrorCode2 { get; set; }

        [FieldOrder(1)]
        [FieldLength(60)]
        [FieldEncoding("us-ascii")]
        public string ErrorText { get; set; }

        [FieldOrder(2)]
        public uint BetId { get; set; }

        //[Ignore]
        //protected override ushort BodyLength => 66;
    }
}
