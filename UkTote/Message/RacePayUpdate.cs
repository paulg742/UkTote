using BinarySerialization;

namespace UkTote.Message
{
    public class RacePayUpdate : MessageBase, IRaceUpdate
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort RaceNumber { get; set; }

        [FieldOrder(2)]
        public ushort Reserved { get; set; }

        public RacePayUpdate()
            : base(Enums.MessageType.RacePayUpdateMsg)
        {

        }

        //[Ignore]
        //protected override ushort BodyLength => 6;
    }
}
