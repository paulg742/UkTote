using BinarySerialization;

namespace UkTote.Message
{
    public class MeetingPoolPayUpdate : MessageBase, IPoolUpdate
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort PoolNumber { get; set; }

        public MeetingPoolPayUpdate()
            : base(Enums.MessageType.MeetingPoolPayUpdateMsg)
        {

        }

        //[Ignore]
        //protected override ushort BodyLength => 4;
    }
}
