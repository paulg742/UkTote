﻿using BinarySerialization;
#if EIGHT_BYTE_MONEY
using money_t = System.UInt64;
#else
using money_t = System.UInt32;
#endif

namespace UkTote.Message
{
    public class MeetingPoolTotalUpdate : MessageBase, IPoolUpdate
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort PoolNumber { get; set; }

        [FieldOrder(2)]
        public money_t MainPoolTotal { get; set; }

        [FieldOrder(3)]
        public uint Reserved1 { get; set; }

        [FieldOrder(4)]
        public uint Reserved2 { get; set; }

        [FieldOrder(5)]
        public uint Reserved3 { get; set; }

        [FieldOrder(6)]
        public money_t ConsolationPoolTotal { get; set; }

        [FieldOrder(7)]
        public uint Reserved4 { get; set; }

        [FieldOrder(8)]
        public uint Reserved5 { get; set; }

        [FieldOrder(9)]
        public uint Reserved6 { get; set; }

        [FieldOrder(10)]
        public money_t BonusPoolTotal { get; set; }

        public MeetingPoolTotalUpdate()
            : base(Enums.MessageType.MeetingPoolTotalUpdate)
        {

        }

        //[Ignore]
        //protected override ushort BodyLength => 40;
    }
}
