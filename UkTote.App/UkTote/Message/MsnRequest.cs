namespace UkTote.Message
{
    public class MsnRequest : RequestMessage
    {
        public MsnRequest(ushort sequence)
            : base(Enums.MessageType.MsnReqMsg, Enums.ActionCode.ActionUnknown, sequence)
        {
        }

        //[Ignore]
        //protected override ushort BodyLength => 0;
    }
}
