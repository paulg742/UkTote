using System;
using BinarySerialization;

namespace UkTote.Message
{
    public class MeetingRequest : RequestMessage
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        public MeetingRequest()
            : base(Enums.MessageType.MeetingReqMsg, Enums.ActionCode.ActionUnknown)
        {

        }

        [Ignore]
        protected override ushort BodyLength => 2;
    }
}
