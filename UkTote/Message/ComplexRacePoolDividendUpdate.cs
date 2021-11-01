using System.Collections.Generic;
using BinarySerialization;

namespace UkTote.Message
{
    public class ComplexRacePoolDividendUpdate : MessageBase
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort RaceNumber { get; set; }

        [FieldOrder(2)]
        public ushort PoolNumber { get; set; }

        [FieldOrder(3)]
        public ushort NumberOfDividends { get; set; }

        [FieldOrder(4)]
        [FieldCount(10)]
        public List<uint> Dividends { get; set; }

        [FieldOrder(5)]
        [FieldCount(10)]
        public List<uint> PoolTotals { get; set; }

        [FieldOrder(6)]
        [FieldCount(10)]
        public List<uint> CarriedForwardAmounts { get; set; }

        [FieldOrder(7)]
        public uint BonusPoolAmount { get; set; }

        [FieldOrder(8)]
        public uint BonusPoolCarriedForwardAmount { get; set; }

        [FieldOrder(9)]
        [FieldCount(40)]
        public List<short> CombinationMap { get; set; }

        public ComplexRacePoolDividendUpdate()
            : base(Enums.MessageType.ComplexRacePoolDividendUpdate)
        {

        }

        [Ignore]
        protected override ushort BodyLength => 216;
    }
}
