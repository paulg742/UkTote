using BinarySerialization;

namespace UkTote.Message
{
    public class AccountLoginRequest : RequestMessage
    {
        [FieldOrder(0)]
        [FieldLength(20)]
        [FieldEncoding("us-ascii")]
        public string Username { get; set; }

        [FieldOrder(1)]
        [FieldLength(20)]
        [FieldEncoding("us-ascii")]
        public string Password { get; set; }

        public AccountLoginRequest()
            : base(Enums.MessageType.AccountLogin, Enums.ActionCode.ActionLogin)
        {
        }

        [Ignore]
        protected override ushort BodyLength => 40;
    }
}
