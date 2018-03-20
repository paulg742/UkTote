using System.Collections.Generic;
using BinarySerialization;
using log4net;

namespace UkTote.Message
{
    public class RaceReply : ReplyMessage
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(RaceReply));

        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort RaceNumber { get; set; }

        [FieldOrder(2)]
        [FieldLength(6)]
        [FieldEncoding("us-ascii")]
        public string RaceTime { get; set; }

        [FieldOrder(3)]
        [FieldLength(60)]
        [FieldEncoding("us-ascii")]
        public string RaceName { get; set; }

        [FieldOrder(4)]
        public ushort HandicapFlag { get; set; }

        [FieldOrder(5)]
        public Enums.RaceType RaceType { get; set; }

        [FieldOrder(6)]
        public ushort NumberOfDeclaredRunners { get; set; }

        [FieldOrder(7)]
        public ushort NumberOfRacePools { get; set; }

        [FieldOrder(8)]
        public ushort Reserved { get; set; }

        [FieldOrder(9)]
        public ushort DistanceMiles { get; set; }

        [FieldOrder(10)]
        public ushort DistanceFurlongs { get; set; }

        [FieldOrder(11)]
        public ushort DistanceYards { get; set; }

        [FieldOrder(12)]
        public ushort DistanceMeters { get; set; }

        [Ignore]
        protected override ushort BodyLength
        {
            get
            {
                return 88;
            }
        }

        [Ignore]
        public IDictionary<int, RunnerReply> Runners { get; set; }

        [Ignore]
        public IDictionary<int, RacePoolReply> RacePools { get; set; }

        [Ignore]
        public bool IsComplete
        {
            get
            {
                _logger.DebugFormat("MeetingNumber:{0} RaceNumber:{1} Runners.Count:{2} NumberOfDeclaredRunners:{3} NumberOfRacePools:{4} RacePools.Count:{5}", 
                    MeetingNumber, RaceNumber, Runners?.Count, NumberOfDeclaredRunners, NumberOfRacePools, RacePools?.Count);

                return ((NumberOfDeclaredRunners == 0) || (Runners != null) && (Runners.Count == NumberOfDeclaredRunners))
                    && ((NumberOfRacePools == 0) || ((RacePools != null) && (RacePools.Count == NumberOfRacePools)));
            }
        }

    }
}
