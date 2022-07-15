using System.Collections.Generic;
using BinarySerialization;

namespace UkTote.Message
{
    public class SuperComplexPoolDividendUpdate : MessageBase, IUpdate
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort PoolNumber { get; set; }

        [FieldOrder(2)]
        public ushort NumberOfLegs { get; set; }

        [FieldOrder(3)]
        public ushort NumberOfDividends { get; set; }

        [FieldOrder(5)]
        [FieldCount(10)]
        public List<uint> Dividends { get; set; }

        [FieldOrder(6)]
        [FieldCount(10)]
        public List<uint> PoolTotals { get; set; }

        [FieldOrder(7)]
        public uint CarriedForwardAmount { get; set; }

        [FieldOrder(8)]
        [FieldCount("NumberOfLegs")]
        public List<MeetingPoolCombination> CombinationMap { get; set; }

        public SuperComplexPoolDividendUpdate()
            : base(Enums.MessageType.SuperComplexPoolDividendUpdate)
        {

        }

        protected override ushort BodyLength => (ushort)(base.BodyLength + NumberOfLegs * Size.Of(typeof(MeetingPoolCombination)));
        //[Ignore]
        //protected override ushort BodyLength => 1322;
    }
}
