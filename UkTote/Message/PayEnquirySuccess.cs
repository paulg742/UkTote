using System;
using System.Collections.Generic;
using BinarySerialization;

namespace UkTote.Message
{
    public class PayEnquirySuccess : ReplyMessage
    {
        [FieldOrder(0)]
        public uint PayoutAmount { get; set; }

        [FieldOrder(1)]
        public uint VoidAmount { get; set; }

        [FieldOrder(2)]
        [FieldLength(18)]
        [FieldEncoding("us-ascii")]
        public string TSN { get; set; }

        [Ignore]
        protected override ushort BodyLength
        {
            get
            {
                return 26;
            }
        }
    }
}
