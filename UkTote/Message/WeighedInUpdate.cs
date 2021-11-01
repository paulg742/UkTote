using System.Collections.Generic;
using BinarySerialization;

namespace UkTote.Message
{
    public class WeighedInUpdate : MessageBase
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort RaceNumber { get; set; }

        [FieldOrder(2)]
        public ushort WeighedIn { get; set; }

        public WeighedInUpdate()
            : base(Enums.MessageType.WeighedInUpdateMsg)
        {

        }

        [Ignore]
        protected override ushort BodyLength => 6;
    }
}
