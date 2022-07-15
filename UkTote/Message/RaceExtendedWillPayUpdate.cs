using System.Collections.Generic;
using BinarySerialization;
using Newtonsoft.Json;
#if EIGHT_BYTE_MONEY
using money_t = System.UInt64;
#else
using money_t = System.UInt32;
#endif

namespace UkTote.Message
{
    public class RaceExtendedWillPayUpdate : MessageBase, IRacePoolUpdate
    {
        public class Combination
        {
            public int N1 { get; set; }
            public int N2 { get; set; }
            public uint Total { get; set; }
        }
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort RaceNumber { get; set; }

        [FieldOrder(2)]
        public ushort PoolNumber { get; set; }

        [FieldOrder(3)]
        public money_t TotalInvestment { get; set; }

        [FieldOrder(4)]
        public ushort NumberOfCombinations { get; set; }

        [FieldOrder(5)]
        [FieldCount("NumberOfCombinations")]
        [JsonIgnore]
        public List<uint> Declarations { get; set; }

        [FieldOrder(6)]
        [FieldCount("NumberOfCombinations")]
        [JsonIgnore]
        public List<uint> CombinationTotal { get; set; }

        [Ignore]
        public List<Combination> CombinationTotals
        {
            get
            {
                var ret = new List<Combination>();
                for (var i=0; i < NumberOfCombinations; ++i)
                {
                    var r1 = ((Declarations[i] & 0xFFFF0000) >> 16);
                    var r2 = (Declarations[i] & 0x0000FFFF);

                    ret.Add(new Combination
                    {
                        N1 = (int)r1,
                        N2 = (int)r2,
                        Total = CombinationTotal[i]
                    });
                }
                return ret; 
            }
        }

        public RaceExtendedWillPayUpdate()
            : base(Enums.MessageType.RacePoolExtendedWillPayUpdateMsg)
        {

        }

        protected override ushort BodyLength => (ushort)(base.BodyLength + NumberOfCombinations * 8); // declarations + CombinationTotal = 8 (two x uints)
        //[Ignore]
        //protected override ushort BodyLength => ((ushort)(12 + NumberOfCombinations * 8));
    }
}
