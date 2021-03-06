﻿using System.Collections.Generic;
using BinarySerialization;

namespace UkTote.Message
{
    public class ComplexRacePoolTotalUpdate : MessageBase
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort RaceNumber { get; set; }

        [FieldOrder(2)]
        public ushort PoolNumber { get; set; }

        [FieldOrder(3)]
        public ushort NumberOfTotals { get; set; }

        [FieldOrder(4)]
        [FieldCount(10)]
        public List<uint> Totals { get; set; }

        [FieldOrder(5)]
        public uint BonusPoolTotal { get; set; }

        public ComplexRacePoolTotalUpdate()
            : base(Enums.MessageType.COMPLEX_RACE_POOL_TOTAL_UPDATE)
        {

        }

        [Ignore]
        protected override ushort BodyLength
        {
            get
            {
                return 52;
            }
        }

    }
}
