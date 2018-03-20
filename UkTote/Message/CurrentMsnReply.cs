using BinarySerialization;

namespace UkTote.Message
{
    public class CurrentMsnReply : ReplyMessage
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
