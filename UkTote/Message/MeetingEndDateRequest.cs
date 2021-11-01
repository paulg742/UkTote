using System;
using BinarySerialization;

namespace UkTote.Message
{
    public class MeetingEndDateRequest : RequestMessage
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        public MeetingEndDateRequest()
            : base(Enums.MessageType.MeetingEndDateReqMsg, Enums.ActionCode.ActionUnknown)
        {

        }

        [Ignore]
        protected override ushort BodyLength => 2;
    }
}
