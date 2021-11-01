using BinarySerialization;

namespace UkTote.Message
{
    public class RacecardRequest : RequestMessage
    {
        [FieldOrder(0)]
        [FieldLength(8)]
        [FieldEncoding("us-ascii")]
        public string Date { get; set; }

        public RacecardRequest()
            : base(Enums.MessageType.RacecardReqMsg, Enums.ActionCode.ActionUnknown)
        {

        }

        [Ignore]
        protected override ushort BodyLength => 8;
    }
}
