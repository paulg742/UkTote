using BinarySerialization;

namespace UkTote.Message
{
    public class ResultUpdate : MessageBase, IRaceUpdate
    {
        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort RaceNumber { get; set; }

        [FieldOrder(2)]
        public ushort Reserved { get; set; }

        [FieldOrder(3)]
        [FieldCount(40)]
        public List<ushort>? NonRunnerMap { get; set; }

        [FieldOrder(4)]
        public ushort FrameSize { get; set; }

        [FieldOrder(5)]
        public ushort NumberPlacesDeclared { get; set; }

        [FieldOrder(6)]
        [FieldCount(10)]
        public List<ushort>? Position { get; set; }

        [FieldOrder(7)]
        [FieldCount(10)]
        public List<ushort>? RunnerNumber { get; set; }

        public ResultUpdate()
            : base(Enums.MessageType.ResultUpdateMsg)
        {

        }

        //[Ignore]
        //protected override ushort BodyLength => 130;
    }
}
