using System.Collections.Generic;
using System.Linq;
using BinarySerialization;
using log4net;

namespace UkTote.Message
{
    public class MeetingReply : ReplyMessage
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(MeetingReply));

        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        [FieldLength(20)]
        [FieldEncoding("us-ascii")]
        public string MeetingName { get; set; }

        [FieldOrder(2)]
        [FieldLength(2)]
        [FieldEncoding("us-ascii")]
        public string MeetingCode { get; set; }

        [FieldOrder(3)]
        [FieldLength(7)]
        public byte[] Reserved { get; set; }

        [FieldOrder(4)]
        public ushort NumberOfRaces { get; set; }

        [FieldOrder(5)]
        public ushort NumberOfMultiLegPools { get; set; }

        [FieldOrder(6)]
        [FieldLength(4)]
        public byte[] Reserved2 { get; set; }

        [FieldOrder(7)]
        public Enums.Going Going { get; set; }

        [Ignore]
        protected override ushort BodyLength
        {
            get
            {
                return 41;
            }
        }

        [Ignore]
        public IDictionary<int, RaceReply> Races { get; set; }

        [Ignore]
        public bool IsComplete
        {
            get
            {
                _logger.DebugFormat("MeetingNumber:{0} Races.Count:{1} NumberOfRaces:{2} #Incomplete:{3}", MeetingNumber, Races?.Count, NumberOfRaces, Races.Count(m => !m.Value.IsComplete));
                return (Races != null) && (Races.Count == NumberOfRaces) && Races.All(m => m.Value.IsComplete);
            }
        }
    }
}
