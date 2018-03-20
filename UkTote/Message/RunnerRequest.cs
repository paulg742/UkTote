using BinarySerialization;

namespace UkTote.Message
{
    public class RunnerRequest : RequestMessage
    {
        public RunnerRequest()
            : base(Enums.MessageType.RUNNER_REQ_MSG, Enums.ActionCode.ACTION_UNKNOWN)
        {

        }
        
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort RaceNumber { get; set; }

        [FieldOrder(2)]
        public ushort RunnerNumber { get; set; }

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
