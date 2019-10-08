using BinarySerialization;

namespace UkTote.Message
{
    public class MeetingPoolPayUpdate : MessageBase
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort PoolNumber { get; set; }

        public MeetingPoolPayUpdate()
            : base(Enums.MessageType.MEETING_POOL_PAY_UPDATE_MSG)
        {

        }

        [Ignore]
        protected override ushort BodyLength
        {
            get
            {
                return 4;
            }
        }

    }
}
