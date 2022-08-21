namespace UkTote.Message
{
    public class RuOkReply : ReplyMessage
    {
        public RuOkReply()
            : base(Enums.MessageType.RuOkReplyMsg, Enums.ActionCode.ActionUnknown)
        {

        }

        //[Ignore]
        //protected override ushort BodyLength => 0;
    }
}
