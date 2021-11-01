using System.Collections.Generic;
using BinarySerialization;

namespace UkTote.Message
{
    public class MeetingPoolWillPayUpdate : MessageBase
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort PoolNumber { get; set; }

        [FieldOrder(2)]
        public ushort Reserved { get; set; }

        [FieldOrder(3)]
        public uint TotalInvestment { get; set; }

        public MeetingPoolWillPayUpdate()
            : base(Enums.MessageType.MeetingPoolWillPayUpdateMsg)
        {

        }

        [Ignore]
        protected override ushort BodyLength => 10;
    }
}
