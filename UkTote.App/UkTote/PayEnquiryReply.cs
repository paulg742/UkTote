#if EIGHT_BYTE_MONEY
using money_t = System.UInt64;
#else
using money_t = System.UInt32;
#endif

namespace UkTote
{
    using Message;
    public class PayEnquiryReply
    {
        public string? Tsn { get; set; }
        public money_t PayoutAmount { get; set; }
        public money_t VoidAmount { get; set; }
        public Enums.ErrorCode ErrorCode { get; set; }
        public string? ErrorText { get; set; }
    }
}
