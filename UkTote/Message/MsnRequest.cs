using BinarySerialization;

namespace UkTote.Message
{
    public class MsnRequest : RequestMessage
    {
        public MsnRequest(ushort sequence)
            : base(Enums.MessageType.MSN_REQ_MSG, Enums.ActionCode.ACTION_UNKNOWN, sequence)
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
