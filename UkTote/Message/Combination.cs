using BinarySerialization;

namespace UkTote.Message
{
    public class Combination
    {
        [FieldOrder(0)]
        public ushort CombinationNumber { get; set; }

        [FieldOrder(1)]
        public ushort RunnerNumber { get; set; }

        [FieldOrder(2)]
        public ushort Position { get; set; }
    }
}
