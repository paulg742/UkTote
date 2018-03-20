using System;
using BinarySerialization;

namespace UkTote.Message
{
    public class MeetingRequest : RequestMessage
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        public MeetingRequest()
            : base(Enums.MessageType.MEETING_REQ_MSG, Enums.ActionCode.ACTION_UNKNOWN)
        {

        }

        [Ignore]
        protected override ushort BodyLength
        {
            get
            {
                return 2;
            }
        }

    }
}
