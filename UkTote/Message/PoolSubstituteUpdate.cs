using System.Collections.Generic;
using BinarySerialization;

namespace UkTote.Message
{
    public class PoolSubstituteUpdate : MessageBase, IRacePoolUpdate
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort RaceNumber { get; set; }

        [FieldOrder(2)]
        public ushort PoolNumber { get; set; }

        [FieldOrder(3)]
        public ushort HorseNumber { get; set; }

        [FieldOrder(4)]
        [FieldCount(40)]
        public List<ushort> SubstitutionsMap { get; set; }

        public PoolSubstituteUpdate()
            : base(Enums.MessageType.PoolSubstituteUpdate)
        {

        }

        [Ignore]
        protected override ushort BodyLength => 88;
    }
}
