using BinarySerialization;

namespace UkTote.Message
{
    public class AccountLoginError : ReplyMessage
    {
        [FieldOrder(0)]
        [FieldLength(20)]
        [FieldEncoding("us-ascii")]
        public string? ErrorText { get; set; }

        //[Ignore]
        //protected override ushort BodyLength => 20;
    }
}
