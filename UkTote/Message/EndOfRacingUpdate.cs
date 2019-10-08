using System.Collections.Generic;
using BinarySerialization;

namespace UkTote.Message
{
    public class EndOfRacingUpdate : MessageBase
    {
        public EndOfRacingUpdate()
            : base(Enums.MessageType.END_RACING_UPDATE_MSG)
        {

        }

        [Ignore]
        protected override ushort BodyLength
        {
            get
            {
                return 0;
            }
        }
    }
}
