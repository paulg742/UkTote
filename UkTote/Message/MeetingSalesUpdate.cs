using BinarySerialization;

namespace UkTote.Message
{
    public class MeetingSalesUpdate : MessageBase, IUpdate
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort Reserved { get; set; }

        public MeetingSalesUpdate()
            : base(Enums.MessageType.MeetingSalesUpdate)
        {

        }

        //[Ignore]
        //protected override ushort BodyLength => 4;
    }
}
