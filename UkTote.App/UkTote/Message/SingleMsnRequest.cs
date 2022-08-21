namespace UkTote.Message
{
    public class SingleMsnRequest : RequestMessage
    {
        public SingleMsnRequest()
            : base(Enums.MessageType.SingleMsnReqMsg, Enums.ActionCode.ActionUnknown)
        {

        }

        //[Ignore]
        //protected override ushort BodyLength => 0;
    }
}
