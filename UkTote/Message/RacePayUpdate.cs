﻿using BinarySerialization;

namespace UkTote.Message
{
    public class RacePayUpdate : MessageBase
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort RaceNumber { get; set; }

        [FieldOrder(2)]
        public ushort Reserved { get; set; }

        public RacePayUpdate()
            : base(Enums.MessageType.RACE_PAY_UPDATE_MSG)
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
