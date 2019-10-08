using BinarySerialization;

namespace UkTote.Message
{
    public class MatrixPoolCombination
    {
        [FieldOrder(0)]
        public ushort CombinationNmber { get; set; }

        [FieldOrder(1)]
        public ushort RunnerNumber { get; set; }

        [FieldOrder(2)]
        public ushort LegNumber { get; set; }

        [FieldOrder(3)]
        public ushort Position { get; set; }
    }
}
