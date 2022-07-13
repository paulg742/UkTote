using System.Collections.Generic;
using BinarySerialization;

namespace UkTote.Message
{
    public class MatrixPoolDividendUpdate : MessageBase, IPoolUpdate
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort PoolNumber { get; set; }

        [FieldOrder(2)]
        public ushort NumberWinningCombinations { get; set; }

        [FieldOrder(3)]
        [FieldCount(90)]
        public List<MatrixPoolCombination> CombinationMap { get; set; }

        [FieldOrder(4)]
        [FieldCount(10)]
        public List<uint> Dividend { get; set; }

        [FieldOrder(5)]
        public uint PoolTotal { get; set; }

        [FieldOrder(6)]
        public uint CarriedForwardAmount { get; set; }

        public MatrixPoolDividendUpdate()
            : base(Enums.MessageType.MatrixPoolDividendUpdate)
        {

        }

        [Ignore]
        protected override ushort BodyLength => 774;
    }
}
