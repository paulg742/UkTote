using BinarySerialization;

namespace UkTote.Message
{
    public class CurrentBalanceRequest : RequestMessage
    {
        public CurrentBalanceRequest()
            : base(Enums.MessageType.CURRENT_BALANCE_REQ_MSG, Enums.ActionCode.ACTION_UNKNOWN)
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
