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
            : base(Enums.MessageType.MEETING_POOL_REQ_MSG, Enums.ActionCode.ACTION_UNKNOWN)
        {

        }

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
