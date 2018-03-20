using BinarySerialization;

namespace UkTote.Message
{
    public class RacePoolUpdate : MessageBase
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort RaceNumber { get; set; }

        [FieldOrder(2)]
        public ushort PoolNumber { get; set; }

        public RacePoolUpdate()
            : base(Enums.MessageType.RACE_POOL_UPDATE_MSG)
        {

        }

        [Ignore]
        protected override ushort BodyLength
        {
            get
            {
                return 6;
            }
        }

    }
}
