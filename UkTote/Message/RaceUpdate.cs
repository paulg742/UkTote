using BinarySerialization;

namespace UkTote.Message
{
    public class RaceUpdate: MessageBase
    {
        public RaceUpdate()
            : base(Enums.MessageType.RACE_UPDATE_MSG)
        {

        }

        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort RaceNumber { get; set; }

        [FieldOrder(2)]
        public ushort WeighedIn{ get; set; }

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
