using BinarySerialization;

namespace UkTote.Message
{
    public abstract class MessageBase
    {
        public const ushort HEADER_LENGTH = 14;
        public const uint MARKER = 4275878552;

        protected MessageBase()
        {
            Marker = MARKER;
        }

        protected MessageBase(Enums.MessageType messageType)
            : this()
        {
            MessageType = messageType;
        }

        protected MessageBase(Enums.MessageType messageType, Enums.ActionCode actionCode)
            :this(messageType)
        {
            ActionCode = actionCode;
        }

        protected MessageBase(Enums.MessageType messageType, Enums.ActionCode actionCode, ushort sequence)
            :this(messageType, actionCode)
        {
            Sequence = sequence;
        }

        [FieldOrder(0)]
        public uint Marker { get; set; }

        [FieldOrder(1)]
        public ushort Sequence { get; set; }

        [FieldOrder(2)]
        public Enums.MessageType MessageType { get; set; }

        [FieldOrder(3)]
        public virtual ushort Length { get; set; }

        [FieldOrder(4)]
        public Enums.ActionCode ActionCode { get; set; }

        [FieldOrder(5)]
        public Enums.ErrorCode ErrorCode { get; set; }

        [Ignore]
        protected abstract ushort BodyLength { get; }
    }
}
