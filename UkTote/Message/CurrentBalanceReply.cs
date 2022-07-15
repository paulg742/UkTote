using BinarySerialization;
using log4net;
#if EIGHT_BYTE_MONEY
using money_t = System.UInt64;
#else
using money_t = System.UInt32;
#endif

namespace UkTote.Message
{
    public class CurrentBalanceReply : ReplyMessage
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(CurrentBalanceReply));

        [FieldOrder(0)]
        public ushort Active { get; set; }

        [FieldOrder(1)]
        public money_t Limit {get;set;}

        [FieldOrder(2)]
        public uint StakeLimit { get; set; }

        [FieldOrder(3)]
        public money_t Balance { get; set; }

        [FieldOrder(4)]
        public money_t SessionBalanceHighWatermark {get;set;}

        [FieldOrder(5)]
        public money_t LifeBalanceHighWatermark { get; set; }

        [FieldOrder(6)]
        public ushort BalanceResetDays { get; set; }

        [FieldOrder(7)]
        public ushort BalanceLimitReached { get; set; }

        [FieldOrder(8)]
        public money_t RemainingBalance { get; set; }

        //[Ignore]
        //protected override ushort BodyLength => (ushort) (Size.Of(Active) + Size.Of(Limit) + Size.Of(StakeLimit) + Size.Of(Balance));
    }
}
