using BinarySerialization;

namespace UkTote.Message
{
    public class AccountLoginSuccess : ReplyMessage
    {
        [FieldOrder(0)]
        [FieldLength(20)]
        [FieldEncoding("us-ascii")]
        public string Username { get; set; }

        //[Ignore]
        //protected override ushort BodyLength => 20;
    }
}
