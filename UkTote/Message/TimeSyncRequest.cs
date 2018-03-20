using BinarySerialization;

namespace UkTote.Message
{
    public class TimeSyncRequest : RequestMessage
    {
        public TimeSyncRequest()
            : base(Enums.MessageType.TIME_SYNC_MSG, Enums.ActionCode.ACTION_UNKNOWN)
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
