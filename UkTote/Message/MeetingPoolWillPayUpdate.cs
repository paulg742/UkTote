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
            : base(Enums.MessageType.MEETING_POOL_WILL_PAY_UPDATE_MSG)
        {

        }

        [Ignore]
        protected override ushort BodyLength
        {
            get
            {
                return 10;
            }
        }

    }
}
