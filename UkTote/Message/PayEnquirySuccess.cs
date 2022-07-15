using BinarySerialization;
#if EIGHT_BYTE_MONEY
using money_t = System.UInt64;
#else
using money_t = System.UInt32;
#endif

namespace UkTote.Message
{
    public class PayEnquirySuccess : ReplyMessage
    {
        [FieldOrder(0)]
        public money_t PayoutAmount { get; set; }

        [FieldOrder(1)]
        public money_t VoidAmount { get; set; }

        [FieldOrder(2)]
        [FieldLength(18)]
        [FieldEncoding("us-ascii")]
        public string Tsn { get; set; }

        //[Ignore]
        //protected override ushort BodyLength => 26;
    }
}
