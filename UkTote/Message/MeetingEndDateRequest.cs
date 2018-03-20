using System;
using BinarySerialization;

namespace UkTote.Message
{
    public class MeetingEndDateRequest : RequestMessage
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        public MeetingEndDateRequest()
            : base(Enums.MessageType.MEETING_END_DATE_REQ_MSG, Enums.ActionCode.ACTION_UNKNOWN)
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
