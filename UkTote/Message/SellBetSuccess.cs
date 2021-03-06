﻿using System;
using System.Collections.Generic;
using BinarySerialization;

namespace UkTote.Message
{
    public class SellBetSuccess : ReplyMessage
    {
        [FieldOrder(0)]
        [FieldLength(18)]
        [FieldEncoding("us-ascii")]
        public string TSN { get; set; }

        [FieldOrder(1)]
        public uint BetId { get; set; }

        [Ignore]
        protected override ushort BodyLength
        {
            get
            {
                return 22;
            }
        }
    }
}
