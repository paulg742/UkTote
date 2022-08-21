namespace UkTote.Message
{
    public class CurrentBalanceRequest : RequestMessage
    {
        public CurrentBalanceRequest()
            : base(Enums.MessageType.CurrentBalanceReqMsg, Enums.ActionCode.ActionUnknown)
        {

        }

        //[Ignore]
        //protected override ushort BodyLength => 0;
    }
}
