using BinarySerialization;

namespace UkTote.Message
{
    public abstract class RequestMessage : MessageBase
    {
        public RequestMessage(Enums.MessageType messageType, Enums.ActionCode actionCode) 
            : base(messageType, actionCode)
        {
        }

        public RequestMessage(Enums.MessageType messageType, Enums.ActionCode actionCode, ushort sequence)
            : base(messageType, actionCode, sequence)
        {
        }

        [Ignore]
        public override ushort Length
        {
            get => (ushort)(HEADER_LENGTH + BodyLength);
            set => base.Length = value;
        }
    }
}
