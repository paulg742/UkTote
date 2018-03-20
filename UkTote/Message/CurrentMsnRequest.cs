using BinarySerialization;

namespace UkTote.Message
{
    public class CurrentMsnRequest : RequestMessage
    {
        public CurrentMsnRequest()
            : base(Enums.MessageType.CURRENT_MSN_REQ_MSG, Enums.ActionCode.ACTION_FAIL)
        {

        }

        [Ignore]
        protected override ushort BodyLength
        {
            get
            {
                return 0;
            }
        }

    }
}
