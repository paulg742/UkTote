using System.Collections.Generic;
using BinarySerialization;

namespace UkTote.Message
{
    public class SellBetRequest : RequestMessage
    {
        public SellBetRequest()
            : base(Enums.MessageType.SELL_BET_REQ_MSG, Enums.ActionCode.ACTION_UNKNOWN)
        {

        }

        [FieldOrder(0)]
        [FieldLength(8)]
        [FieldEncoding("us-ascii")]
        public string RacecardDate { get; set; }

        [FieldOrder(1)]
        public uint UnitStake { get; set; }

        [FieldOrder(2)]
        public uint TotalStake { get; set; }

        [FieldOrder(3)]
        public ushort Reserved { get; set; }

        [FieldOrder(4)]
        public Enums.BetCode BetCode { get; set; }

        [FieldOrder(5)]
        public Enums.BetOption BetOption { get; set; }

        [FieldOrder(6)]
        public ushort Reserved2 { get; set; }

        [FieldOrder(7)]
        public ushort NumberOfSelections { get; set; }

        [FieldOrder(8)]
        public uint BetId { get; set; }

        [FieldOrder(9)]
        [FieldCount("NumberOfSelections")]
        public List<Selection> Selections { get; set; }

        [Ignore]
        protected override ushort BodyLength
        {
            get
            {
                return (ushort) (30 + NumberOfSelections * 13);
            }
        }

    }
}
