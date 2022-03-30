using BinarySerialization;

namespace UkTote.Message
{
    public class PayEnquiryRequest : RequestMessage
    {
        public PayEnquiryRequest()
            : base(Enums.MessageType.PayBetReqMsg, Enums.ActionCode.ActionUnknown)
        {

        }

        [FieldOrder(0)]
        [FieldLength(18)]
        [FieldEncoding("us-ascii")]
        public string Tsn { get; set; }

        [Ignore]
        protected override ushort BodyLength => 18;
    }
}
