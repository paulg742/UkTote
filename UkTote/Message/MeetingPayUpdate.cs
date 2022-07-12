﻿using BinarySerialization;

namespace UkTote.Message
{
    public class MeetingPayUpdate : MessageBase, IUpdate
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort Reserved { get; set; }

        public MeetingPayUpdate()
            : base(Enums.MessageType.MeetingPayUpdateMsg)
        {

        }

        [Ignore]
        protected override ushort BodyLength => 4;
    }
}
