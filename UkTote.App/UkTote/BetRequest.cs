namespace UkTote
{
    using Message;
    public class BetRequest
    {
        public BetRequest()
        {
            Ref = Guid.NewGuid();
        }

        public BetRequest(DateTime forDate, ushort meetingNumber, ushort raceNumber, int unitStake, int totalStake, Enums.BetCode betCode, Enums.BetOption betOption, int[] selections, int? betId = null)
            : this()
        {
            ForDate = forDate;
            MeetingNumber = meetingNumber;
            UnitStake = unitStake;
            TotalStake = totalStake;
            BetCode = betCode;
            BetOption = betOption;
            Selections = Selection.Create(meetingNumber, raceNumber, selections);
            BetId = betId;
        }

        public BetRequest(DateTime forDate, ushort meetingNumber, int unitStake, int totalStake, Enums.BetCode betCode, Enums.BetOption betOption, (int race, int selection)[] selections, int? betId = null)
            : this()
        {
            ForDate = forDate;
            MeetingNumber = meetingNumber;
            UnitStake = unitStake;
            TotalStake = totalStake;
            BetCode = betCode;
            BetOption = betOption;
            Selections = Selection.Create(meetingNumber, selections);
            BetId = betId;
        }

        public DateTime ForDate { get; set; }
        public int MeetingNumber { get; set; }
        public int UnitStake { get; set; }
        public int TotalStake { get; set; }
        public Enums.BetCode BetCode { get; set; }
        public Enums.BetOption BetOption { get; set; }
        public Selection[]? Selections { get; set; }
        public int? BetId { get; set; }
        public Guid Ref { get; set; }
    }
}
