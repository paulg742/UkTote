using BinarySerialization;
#if EIGHT_BYTE_MONEY
using money_t = System.UInt64;
#else
using money_t = System.UInt32;
#endif

namespace UkTote.Message
{
    public class MeetingPoolWillPayUpdate : MessageBase, IPoolUpdate
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort PoolNumber { get; set; }

        [FieldOrder(2)]
        public ushort Reserved { get; set; }

        [FieldOrder(3)]
        public money_t TotalInvestment { get; set; }

        public MeetingPoolWillPayUpdate()
            : base(Enums.MessageType.MeetingPoolWillPayUpdateMsg)
        {

        }

        //[Ignore]
        //protected override ushort BodyLength => 10;
    }
}
