using BinarySerialization;
using log4net;

namespace UkTote.Message
{
    public class CurrentBalanceReply : ReplyMessage
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(CurrentBalanceReply));

        [FieldOrder(0)]
        public ushort Active { get; set; }

        [FieldOrder(1)]
        public uint Limit { get; set; }

        [FieldOrder(2)]
        public uint StakeLimit { get; set; }

        [FieldOrder(3)]
        public uint Balance { get; set; }

        [FieldOrder(4)]
        public uint SessionBalanceHighWatermark { get; set; }

        [FieldOrder(5)]
        public uint LifeBalanceHighWatermark { get; set; }

        [FieldOrder(6)]
        public ushort BalanceResetDays { get; set; }

        [FieldOrder(7)]
        public ushort BalanceLimitReached { get; set; }

        [FieldOrder(8)]
        public uint RemainingBalance { get; set; }

        [Ignore]
        protected override ushort BodyLength => 30;
    }
}
