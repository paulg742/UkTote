using System.Collections.Generic;
using BinarySerialization;

namespace UkTote.Message
{
    public class RacePoolDividendUpdate : MessageBase, IRaceUpdate
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort RaceNumber { get; set; }

        [FieldOrder(2)]
        public ushort PoolNumber { get; set; }

        [FieldOrder(3)]
        public ushort NumberWinningCombinations { get; set; }

        [FieldOrder(4)]
        [FieldCount(20)]
        public List<Combination> CombinationMap { get; set; }

        [FieldOrder(5)]
        [FieldLength(40)]
        public byte[] Reserved { get; set; }

        [FieldOrder(6)]
        [FieldCount(10)]
        public List<uint> Dividend { get; set; }

        [FieldOrder(7)]
        public uint CarriedForwardAmount { get; set; }

        [FieldOrder(8)]
        [FieldLength(48)]
        public byte[] Reserved2 { get; set; }

        public RacePoolDividendUpdate()
            : base(Enums.MessageType.RacePoolDivUpdateMsg)
        {

        }

        [Ignore]
        protected override ushort BodyLength => 274;
    }
}
