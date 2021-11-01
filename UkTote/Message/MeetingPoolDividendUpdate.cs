﻿using System.Collections.Generic;
using BinarySerialization;

namespace UkTote.Message
{
    public class MeetingPoolDividendUpdate : MessageBase
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort PoolNumber { get; set; }

        [FieldOrder(2)]
        public uint MainDividend { get; set; }

        [FieldOrder(3)]
        public uint ConsolationDividend{ get; set; }

        [FieldOrder(4)]
        public uint Reserved1 { get; set; }

        [FieldOrder(5)]
        public uint Reserved2 { get; set; }

        [FieldOrder(6)]
        public uint CarriedForwardAmount { get; set; }

        [FieldOrder(7)]
        public uint BonusPoolAmount { get; set; }

        [FieldOrder(8)]
        public uint Reserved3 { get; set; }

        [FieldOrder(9)]
        public uint Reserved4 { get; set; }

        [FieldOrder(10)]
        public uint Reserved5 { get; set; }

        [FieldOrder(11)]
        public uint Reserved6 { get; set; }

        [FieldOrder(12)]
        public ushort NumberLegs { get; set; }

        [FieldOrder(13)]
        [FieldCount("NumberLegs")]
        public List<MeetingPoolCombination> CombinationMap { get; set; }

        public MeetingPoolDividendUpdate()
            : base(Enums.MessageType.MeetingPoolDivUpdateMsg)
        {

        }

        [Ignore]
        protected override ushort BodyLength => 1276;
    }
}
