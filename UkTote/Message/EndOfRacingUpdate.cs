using BinarySerialization;

namespace UkTote.Message
{
    public class EndOfRacingUpdate : MessageBase, IUpdate
    {
        public EndOfRacingUpdate()
            : base(Enums.MessageType.EndRacingUpdateMsg)
        {

        }

        public ushort MeetingNumber { get => 0; }

        [Ignore]
        protected override ushort BodyLength => 0;
    }
}
