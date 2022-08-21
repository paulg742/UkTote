using BinarySerialization;

namespace UkTote.Message
{
    public class RunnerUpdate : MessageBase, IRaceUpdate
    {
        public RunnerUpdate()
            : base(Enums.MessageType.RunnerUpdateMsg)
        {

        }

        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort RaceNumber { get; set; }

        [FieldOrder(2)]
        public ushort RunnerNumber { get; set; }

        [FieldOrder(3)]
        [FieldLength(20)]
        public byte[]? Reserved { get; set; }

        [FieldOrder(4)]
        [FieldCount(40)]
        public List<ushort>? NonRunnerMap { get; set; }

        //[Ignore]
        //protected override ushort BodyLength => 106;
    }
}
