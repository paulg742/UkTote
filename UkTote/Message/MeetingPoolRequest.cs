using System;
using BinarySerialization;

namespace UkTote.Message
{
    public class MeetingPoolRequest : RequestMessage
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort PoolNumber { get; set; }

        public MeetingPoolRequest()
            : base(Enums.MessageType.MeetingPoolReqMsg, Enums.ActionCode.ActionUnknown)
        {

        }

        [Ignore]
        protected override ushort BodyLength => 4;
    }
}
