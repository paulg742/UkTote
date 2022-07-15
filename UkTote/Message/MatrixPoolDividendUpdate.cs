using System.Collections.Generic;
using BinarySerialization;
#if EIGHT_BYTE_MONEY
using money_t = System.UInt64;
#else
using money_t = System.UInt32;
#endif

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
        public money_t PoolTotal { get; set; }

        [FieldOrder(6)]
        public money_t CarriedForwardAmount { get; set; }

        public MatrixPoolDividendUpdate()
            : base(Enums.MessageType.MatrixPoolDividendUpdate)
        {

        }

        //[Ignore]
        //protected override ushort BodyLength => 774;
    }
}
