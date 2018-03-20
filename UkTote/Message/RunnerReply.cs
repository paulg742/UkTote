using BinarySerialization;

namespace UkTote.Message
{
    public class RunnerReply : ReplyMessage
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort RaceNumber { get; set; }

        [FieldOrder(2)]
        public ushort RunnerNumber { get; set; }

        [FieldOrder(3)]
        [FieldLength(26)]
        [FieldEncoding("us-ascii")]
        public string RunnerName { get; set; }

        [FieldOrder(4)]
        [FieldLength(16)]
        public byte[] Reserved { get; set; }

        [Ignore]
        protected override ushort BodyLength
        {
            get
            {
                return 48;
            }
        }
    }
}
