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
            : base(Enums.MessageType.RACECARD_REQ_MSG, Enums.ActionCode.ACTION_UNKNOWN)
        {

        }

        [Ignore]
        protected override ushort BodyLength
        {
            get
            {
                return 8;
            }
        }

    }
}
