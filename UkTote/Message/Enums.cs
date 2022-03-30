namespace UkTote.Message
{
    public static class Enums
    {
        public enum MessageType : ushort
        {
            RacecardReqMsg = 0x02,
            MeetingReqMsg = 0x03,
            MeetingPoolReqMsg = 0x05,
            RaceReqMsg = 0x06,
            PoolReqMsg = 0x07,
            RunnerReqMsg = 0x09,
            MeetingUpdateMsg = 0x0A,
            RaceUpdateMsg = 0x0B,
            RacePoolUpdateMsg = 0x0C,
            MeetingPoolUpdate = 0x0E,
            RunnerUpdateMsg = 0x0F,

            MeetingSalesUpdate = 0x12,
            RaceSalesUpdate = 0x13,
            RacePoolSalesUpdate = 0x14,
            MeetingPoolSalesUpdate = 0x15,
            ResultUpdateMsg = 0x17,
            RacePoolDivUpdateMsg = 0x1D,
            MeetingPoolDivUpdateMsg = 0x1F,

            MeetingPayUpdateMsg = 0x25,
            RacePayUpdateMsg = 0x26,
            RacePoolPayUpdateMsg = 0x27,
            MeetingPoolPayUpdateMsg = 0x28,
            LegBreakdownUpdateMsg = 0x40,
            RacePoolWillPayUpdateMsg = 0x2D,
            MeetingPoolWillPayUpdateMsg = 0x2E,
            RacePoolExtendedWillPayUpdateMsg = 0x2F,

            SubstituteUpdateMsg = 0x39,
            TimeSyncMsg = 0x3B,

            MeetingPoolTotalUpdate = 0x43,
            WeighedInUpdateMsg = 0x44,
            PoolSubstituteUpdate = 0x45,
            ComplexRacePoolTotalUpdate = 0x46,
            ComplexRacePoolDividendUpdate = 0x47,
            MatrixPoolDividendUpdate = 0x48,

            AccountLogin = 0x1000,
            AccountLogout = 0x1001,
            SellBetReqMsg = 0x1002,
            PayBetReqMsg = 0x1003,
            CancelBetReqMsg = 0x1004,
            RuOkRequestMsg = 0x1005,
            RuOkReplyMsg = 0x1006,
            MsnReqMsg = 0x1007,
            CurrentMsnReqMsg = 0x1008,
            SingleMsnReqMsg = 0x1009,
            EndRacingUpdateMsg = 0x100A,
            CancelBetIdReqMsg = 0x100B,
            MeetingEndDateReqMsg = 0x100E,
            CurrentBalanceReqMsg = 0x100F,

            SuperComplexPoolDividendUpdate = 0x2000,
        }

        public enum ActionCode : ushort
        {
            ActionFail = 0x00,
            ActionSuccess = 0x01,
            ActionOn = 0x03,
            ActionAbandoned = 0x04,
            ActionCancelled = 0x05,
            ActionDelayed = 0x06,
            ActionPostponed = 0x07,
            ActionRescheduled = 0x08,
            ActionVoid = 0x09,
            ActionSalesOpen = 0x0A,
            ActionSalesClosed = 0x0B,
            ActionNormal = 0x0C,
            ActionRunning = 0x0D,
            ActionBroughtDown = 0x0E,
            ActionFallen = 0x0F,
            ActionLeftAtStart = 0x10,
            ActionNonRunner = 0x11,
            ActionRodeOver = 0x12,
            ActionPulledUp = 0x13,
            ActionRefused = 0x14,
            ActionSlippedUp = 0x15,
            ActionUnseatedRider = 0x16,
            ActionWithdrawn = 0x17,
            ActionBookOpen = 0x18,
            ActionBookClosed = 0x19,
            ActionPartialResult = 0x1A,
            ActionFullResult = 0x1B,
            ActionOff = 0x1F,
            ActionClosed = 0x20,
            ActionPayOpen = 0x21,
            ActionPayClosed = 0x22,
            ActionToteSubstitute = 0x27,
            ActionUnknown = 0x99,
            ActionLogin = 0x1000,
            ActionLogout = 0x1001,
            ActionEndRacingLogout = 0x1002
        }

        public enum ErrorCode : ushort
        {
            Success = 0x0000,
            InternalError = 0x0001,
            CommsFailure = 0x0002,
            InvalidMessage = 0x0003,
            InvalidMessageType = 0x0004,
            InvalidDate = 0x000A,
            InvalidCourse = 0x000E,
            InvalidMeeting = 0x000F,
            InvalidRace = 0x0011,
            InvalidPool = 0x0012,
            InvalidRunner = 0x0013,
            InvalidRacecardNotReady = 0x0023,
            InvalidMessageLength = 0x0024,
            InvalidNofBets = 0x03E8,
            InvalidUnitStake = 0x03E9,
            InvalidTotalUnitStake = 0x03EA,
            InvalidTotalTax = 0x03EB,
            InvalidBetSelection = 0x03F3,
            InvalidDuplicateSelection = 0x03F4,
            InvalidRaceSelection = 0x03F9,
            InvalidBetType = 0x03FC,
            InvalidTsn = 0x03FF,
            InvalidBetOption = 0x0402,
            InvalidBetId = 0x0410,
            InvalidFavourite = 0x0BB8,
            InvalidMeetingCancelled = 0x0BBA,
            InvalidRaceCancelled = 0x0BBB,
            InvalidRaceVoid = 0x0BBC,
            InvalidRaceOff = 0x0BBD,
            InvalidRaceClosed = 0x0BBE,
            InvalidPoolCancelled = 0x0BBF,
            MultipleMeetings = 0x0BC4,
            MultipleRaces = 0x0BC5,
            NoSelections = 0x0BC6,
            MeetingUnavailable = 0x0BC7,
            RaceUnavailable = 0x0BC8,
            PoolUnavailable = 0x0BC9,
            InvalidRunnerUnavailable = 0x0BCA,
            InvalidNonRunner = 0x0BCB,
            InvalidSalesNotOpened = 0x0BCC,
            CancelNotToday = 0x0BD2,
            SellBetFailed = 0x1389,
            GetBetFailed = 0x138B,
            BetAlreadyCancelled = 0x138E,
            BetStatusFailed = 0x1391,
            InvalidBetNotSettled = 0x1394,
            InvalidAccountNotLoggedOn = 0x1B59,
            InsufficientFunds = 0x1B5D,
            InvalidMeetingNumber = 0x234E
        }

        public enum RacecardStatus : ushort
        {
            Initialised = 1,
            Unconfirmed = 2,
            Committed = 3,
            Closedown = 4,
            Secured = 5
        }

        public enum Going : ushort
        {
            Equitrack = 0,
            Firm = 1,
            GoodToFirm = 2,
            Good = 3,
            GoodToYielding = 4,
            Soft = 5,
            SoftToYielding = 6,
            Yielding = 7,
            Heavy = 8,
            YieldingToGood = 9,
            Unknown = 10,
            GoodToSoft = 11
        }

        public enum RaceType : ushort
        {
            Flat = 1,
            NationalHunt = 2,
            Trottint = 3
        }

        public enum BetCode : ushort
        {
            Win = 0x001,
            Place = 0x002,
            Exacta = 0x003,
            Trifecta = 0x004,
            Jackpot = 0x005,
            Placepot = 0x006,
            Quadpot = 0x007,
            Scoop6 = 0x008,
            Swinger = 0x009,
            Totedouble = 0x0A,
            Totetreble = 0x0B,
            Super7 = 0x0C,
            Superjackpot7 = 0x0E,
            Placepot7 = 0x0F,
            Superjackpot8 = 0x10,
            Placepot8 = 0x11,
            Quaddie = 0x12,
            ThursdayMillions = 0x13,
            Quinella = 0x14,
            Trio = 0x15,
            Doubletrio = 0x16,
            Tripletrio = 0x17,
            Sixup = 0x18
        }

        public enum BetOption : ushort
        {
            UnknownOption = 0x000,
            NoOption = 0x001,
            Straight = 0x002,
            Permutation = 0x004,
            Banker = 0x008,
            FloatingBanker = 0x010,
            EachWay = 0x020
        }
    }
}
