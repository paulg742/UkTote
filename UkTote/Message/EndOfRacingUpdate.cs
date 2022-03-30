using BinarySerialization;

namespace UkTote.Message
{
    public class EndOfRacingUpdate : MessageBase
    {
        public EndOfRacingUpdate()
            : base(Enums.MessageType.EndRacingUpdateMsg)
        {

        }

        [Ignore]
        protected override ushort BodyLength => 0;
    }
}
