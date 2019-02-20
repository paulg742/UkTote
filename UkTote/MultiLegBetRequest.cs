using System;

namespace UkTote
{
    using Message;
    public class MultiLegBetRequest
    {
        public MultiLegBetRequest()
        {
            Ref = Guid.NewGuid();
        }

        public MultiLegBetRequest(DateTime forDate, int meetingNumber, int raceNumber, int unitStake, int totalStake, Enums.BetCode betCode, Enums.BetOption betOption, 
            Selection[] selections, int? betId = null)
            : this()
        {
            ForDate = forDate;
            MeetingNumber = meetingNumber;
            RaceNumber = raceNumber;
            UnitStake = unitStake;
            TotalStake = totalStake;
            BetCode = betCode;
            BetOption = betOption;
            Selections = selections;
            BetId = betId;
        }

        public DateTime ForDate { get; set; }
        public int MeetingNumber { get; set; }
        public int RaceNumber { get; set; }
        public int UnitStake { get; set; }
        public int TotalStake { get; set; }
        public Enums.BetCode BetCode { get; set; }
        public Enums.BetOption BetOption { get; set; }
        public Selection[] Selections { get; set; }
        public int? BetId { get; set; }
        public Guid Ref { get; set; }
    }
}
