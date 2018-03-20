using System.Collections.Generic;
using BinarySerialization;

namespace UkTote.Message
{
    public class PayEnquiryRequest : RequestMessage
    {
        public PayEnquiryRequest()
            : base(Enums.MessageType.PAY_BET_REQ_MSG, Enums.ActionCode.ACTION_UNKNOWN)
        {

        }

        [FieldOrder(0)]
        [FieldLength(18)]
        [FieldEncoding("us-ascii")]
        public string TSN { get; set; }

        [Ignore]
        protected override ushort BodyLength
        {
            get
            {
                return 18;
            }
        }

    }
}
