using BinarySerialization;

namespace UkTote.Message
{
    public class SingleMsnReply : ReplyMessage
    {
        [Ignore]
        protected override ushort BodyLength => 0;
    }
}
