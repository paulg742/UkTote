using System.Collections.Generic;
using BinarySerialization;
#if EIGHT_BYTE_MONEY
using money_t = System.UInt64;
#else
using money_t = System.UInt32;
#endif

namespace UkTote.Message
{
    public class ComplexRacePoolDividendUpdate : MessageBase, IRacePoolUpdate
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
        public List<money_t> PoolTotals { get; set; }

        [FieldOrder(6)]
        [FieldCount(10)]
        public List<money_t> CarriedForwardAmounts { get; set; }

        [FieldOrder(7)]
        public money_t BonusPoolAmount { get; set; }

        [FieldOrder(8)]
        public money_t BonusPoolCarriedForwardAmount { get; set; }

        [FieldOrder(9)]
        [FieldCount(40)]
        public List<short> CombinationMap { get; set; }

        public ComplexRacePoolDividendUpdate()
            : base(Enums.MessageType.ComplexRacePoolDividendUpdate)
        {

        }

        //[Ignore]
        //protected override ushort BodyLength => 216;
    }
}
