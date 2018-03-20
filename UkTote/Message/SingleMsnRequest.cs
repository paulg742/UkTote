using BinarySerialization;

namespace UkTote.Message
{
    public class SingleMsnRequest : RequestMessage
    {
        public SingleMsnRequest()
            : base(Enums.MessageType.SINGLE_MSN_REQ_MSG, Enums.ActionCode.ACTION_UNKNOWN)
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
