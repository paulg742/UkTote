using BinarySerialization;

namespace UkTote.Message
{
    public class RaceSalesUpdate : MessageBase
    {
        public RaceSalesUpdate()
            : base(Enums.MessageType.RACE_SALES_UPDATE)
        {

        }

        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort RaceNumber { get; set; }

        [FieldOrder(2)]
        public ushort Reserved { get; set; }

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
