using System.Collections.Generic;
using BinarySerialization;

namespace UkTote.Message
{
    public class SubstituteUpdate : MessageBase
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort RaceNumber { get; set; }

        [FieldOrder(2)]
        public ushort Substitute { get; set; }

        public SubstituteUpdate()
            : base(Enums.MessageType.SubstituteUpdateMsg)
        {

        }

        [Ignore]
        protected override ushort BodyLength => 6;
    }
}
