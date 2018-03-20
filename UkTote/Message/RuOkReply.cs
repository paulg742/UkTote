using BinarySerialization;

namespace UkTote.Message
{
    public class RuOkReply : ReplyMessage
    {
        public RuOkReply()
            : base(Enums.MessageType.RUOk_REPLY_MSG, Enums.ActionCode.ACTION_UNKNOWN)
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
