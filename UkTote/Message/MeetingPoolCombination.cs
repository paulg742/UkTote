using System.Collections.Generic;
using BinarySerialization;

namespace UkTote.Message
{
    public class MeetingPoolCombination
    {
        [FieldOrder(0)]
        [FieldCount(41)]
        public List<short> Runners { get; set; }
    }
}
