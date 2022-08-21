namespace UkTote.Message
{
    public class TimeSyncRequest : RequestMessage
    {
        public TimeSyncRequest()
            : base(Enums.MessageType.TimeSyncMsg, Enums.ActionCode.ActionUnknown)
        {

        }

        //[Ignore]
        //protected override ushort BodyLength => 0;
    }
}
