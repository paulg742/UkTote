using BinarySerialization;

namespace UkTote.Message
{
    public class RacePoolRequest : RequestMessage
    {
        public RacePoolRequest()
            : base(Enums.MessageType.PoolReqMsg, Enums.ActionCode.ActionUnknown)
        {

        }

        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort RaceNumber { get; set; }

        [FieldOrder(2)]
        public ushort PoolNumber { get; set; }

        [Ignore]
        protected override ushort BodyLength => 6;
    }
}
