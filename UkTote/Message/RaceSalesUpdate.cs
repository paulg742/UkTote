using BinarySerialization;

namespace UkTote.Message
{
    public class RaceSalesUpdate : MessageBase, IRaceUpdate
    {
        public RaceSalesUpdate()
            : base(Enums.MessageType.RaceSalesUpdate)
        {

        }

        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort RaceNumber { get; set; }

        [FieldOrder(2)]
        public ushort Reserved { get; set; }

        //[Ignore]
        //protected override ushort BodyLength => 6;
    }
}
