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
            : base(Enums.MessageType.ACCOUNT_LOGOUT, Enums.ActionCode.ACTION_LOGOUT)
        {

        }

        [Ignore]
protected override ushort BodyLength
        {
            get
            {
                return 20;
            }
        }

    }
}
