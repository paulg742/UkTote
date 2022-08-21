namespace UkTote.Message
{
    public abstract class ReplyMessage : MessageBase
    {
        public ReplyMessage()
            : base()
        {

        }

        public ReplyMessage(Enums.MessageType messageType, Enums.ActionCode actionCode)
            : base(messageType, actionCode)
        {

        }
    }
}
