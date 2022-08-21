namespace UkTote.Message
{
    public class CurrentMsnRequest : RequestMessage
    {
        public CurrentMsnRequest()
            : base(Enums.MessageType.CurrentMsnReqMsg, Enums.ActionCode.ActionFail)
        {

        }

        //[Ignore]
        //protected override ushort BodyLength => 0;
    }
}
