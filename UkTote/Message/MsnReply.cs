using BinarySerialization;

namespace UkTote.Message
{
    public class MsnReply : ReplyMessage
    {
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
