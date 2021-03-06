﻿using BinarySerialization;

namespace UkTote.Message
{
    public class RacePoolPayUpdate : MessageBase
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort RaceNumber { get; set; }

        [FieldOrder(2)]
        public ushort PoolNumber { get; set; }

        public RacePoolPayUpdate()
            : base(Enums.MessageType.RACE_POOL_PAY_UPDATE_MSG)
        {

        }

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
