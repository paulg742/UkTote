using BinarySerialization;

namespace UkTote.Message
{
    public class RaceRequest : RequestMessage
    {
        public RaceRequest()
            : base(Enums.MessageType.RACE_REQ_MSG, Enums.ActionCode.ACTION_UNKNOWN)
        {

        }

        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort RaceNumber { get; set; }

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
