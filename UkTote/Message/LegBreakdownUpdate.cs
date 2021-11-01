using System.Collections.Generic;
using BinarySerialization;

namespace UkTote.Message
{
    public class LegBreakdownUpdate : MessageBase
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort PoolNumber { get; set; }

        [FieldOrder(2)]
        public ushort LegNumber { get; set; }

        [FieldOrder(3)]
        public uint MainStakeTotal { get; set; }

        [FieldOrder(4)]
        public uint ConsolationStakeTotal { get; set; }

        [FieldOrder(5)]
        [FieldCount(41)]
        public List<uint> MainStakeBreakdowns { get; set; }

        [FieldOrder(6)]
        [FieldCount(41)]
        public List<uint> ConsolationStakeBreakdowns { get; set; }

        public LegBreakdownUpdate()
            : base(Enums.MessageType.LegBreakdownUpdateMsg)
        {

        }

        [Ignore]
        protected override ushort BodyLength => 342;
    }
}
