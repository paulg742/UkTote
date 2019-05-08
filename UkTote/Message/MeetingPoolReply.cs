using System.Collections.Generic;
using System.Linq;
using BinarySerialization;


namespace UkTote.Message
{
    public class MeetingPoolReply : ReplyMessage
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort MeetingPoolNumber { get; set; }

        [FieldOrder(2)]
        public ushort Reserved1 { get; set; }

        [FieldOrder(3)]
        [FieldLength(16)]
        [FieldEncoding("us-ascii")]
        public string PoolName { get; set; }

        [FieldOrder(4)]
        public ushort NumberOfLegs { get; set; }

        [FieldOrder(5)]
        [FieldCount(15)]
        public List<ushort> RaceMap { get; set; }

        [FieldOrder(6)]
        [FieldLength(18)]
        public byte[] Reserved2 { get; set; }

        [Ignore]
        protected override ushort BodyLength
        {
            get
            {
                return 72;
            }
        }

        public string GetRaces()
        {
            var ret = "";
            for (var i=0; i < RaceMap.Count; ++i)
            {
                if (RaceMap[i] > 0)
                {
                    if (ret.Length == 0) ret = (i + 1).ToString();
                    else ret += "," + (i + 1).ToString();
                }
            }
            return ret;
        }

        [Ignore]
        public int[] Races
        {
            get
            {
                var ret = new List<int>();
                for (var i = 0; i < RaceMap.Count; ++i)
                {
                    if (RaceMap[i] > 0)
                    {
                        ret.Add(i + 1);
                    }
                }
                return ret.ToArray();
            }
        }
    }
}
