using BinarySerialization;
#if EIGHT_BYTE_MONEY
using money_t = System.UInt64;
#else
using money_t = System.UInt32;
#endif

namespace UkTote.Message
{
    public class RacePoolReply : ReplyMessage
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort RaceNumber { get; set; }

        [FieldOrder(2)]
        public ushort PoolNumber { get; set; }

        [FieldOrder(3)]
        public ushort Reserved1 { get; set; }

        [FieldOrder(4)]
        [FieldLength(16)]
        [FieldEncoding("us-ascii")]
        public string?PoolName { get; set; }

        [FieldOrder(5)]
        [FieldCount(2)]
        public money_t[]? Reserved2 { get; set; }

        [FieldOrder(6)]
        [FieldLength(10)]
        public byte[]? Reserved3 { get; set; }

        //[Ignore]
        //protected override ushort BodyLength => 42;
    }
}
