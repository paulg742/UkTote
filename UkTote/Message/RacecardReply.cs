using System.Linq;
using BinarySerialization;
using System.Collections.Generic;
using log4net;

namespace UkTote.Message
{
    public class RacecardReply : ReplyMessage
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(RacecardReply));

        [FieldOrder(0)]
        [FieldLength(60)]
        public string Description { get; set; }

        [FieldOrder(1)]
        [FieldLength(8)]
        [FieldEncoding("us-ascii")]
        public string Date { get; set; }

        [FieldOrder(2)]
        public Enums.RacecardStatus RacecardStatus { get; set; }

        [FieldOrder(3)]
        public ushort NumMeetings { get; set; }

        [FieldOrder(4)]
        [FieldLength(2)]
        public byte[] Reserved { get; set; }

        [Ignore]
        protected override ushort BodyLength
        {
            get
            {
                return 26;
            }
        }

        [Ignore]
        public IDictionary<int, MeetingReply> Meetings { get; set; }

        [Ignore]
        public bool IsComplete
        {
            get
            {
                _logger.DebugFormat("Meetings.Count:{0} NumMeetings:{1} #Incomplete:{2}", Meetings?.Count, NumMeetings, Meetings.Count(m => !m.Value.IsComplete));
                return (Meetings != null) && (Meetings.Count == NumMeetings) && Meetings.All(m => m.Value.IsComplete);
            }
        }
    }
}
