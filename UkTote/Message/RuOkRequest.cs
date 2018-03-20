using BinarySerialization;

namespace UkTote.Message
{
    public class RuOkRequest : RequestMessage
    {
        public RuOkRequest()
            : base(Enums.MessageType.RUOk_REQUEST_MSG, Enums.ActionCode.ACTION_UNKNOWN)
        {

        }

        [FieldOrder(0)]
        [FieldLength(20)]
        [FieldEncoding("us-ascii")]
        public string Username { get; set; }

        [FieldOrder(1)]
        [FieldLength(20)]
        [FieldEncoding("us-ascii")]
        public string Password { get; set; }

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
