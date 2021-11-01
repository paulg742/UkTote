using BinarySerialization;

namespace UkTote.Message
{
    public class AccountLogoutRequest : RequestMessage
    {
        [FieldOrder(0)]
        [FieldLength(20)]
        [FieldEncoding("us-ascii")]
        public string Username { get; set; }

        public AccountLogoutRequest()
            : base(Enums.MessageType.AccountLogout, Enums.ActionCode.ActionLogout)
        {

        }

        [Ignore]
protected override ushort BodyLength => 20;
    }
}
