using BinarySerialization;

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
        public string PoolName { get; set; }

        [FieldOrder(5)]
        [FieldLength(18)]
        public byte[] Reserved2 { get; set; }

        [Ignore]
        protected override ushort BodyLength => 42;
    }
}
