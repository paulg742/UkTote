namespace UkTote.Message
{
    public static class Enums
    {
        public enum MessageType : ushort
        {
            RACECARD_REQ_MSG = 0x02,
            MEETING_REQ_MSG = 0x03,
            MEETING_POOL_REQ_MSG = 0x05,
            RACE_REQ_MSG = 0x06,
            POOL_REQ_MSG = 0x07,
            RUNNER_REQ_MSG = 0x09,
            MEETING_UPDATE_MSG = 0x0A,
            RACE_UPDATE_MSG = 0x0B,
            RACE_POOL_UPDATE_MSG = 0x0C,
            MEETING_POOL_UPDATE = 0x0E,
            RUNNER_UPDATE_MSG = 0x0F,

            MEETING_SALES_UPDATE = 0x12,
            RACE_SALES_UPDATE = 0x13,
            RACE_POOL_SALES_UPDATE = 0x14,
            MEETING_POOL_SALES_UPDATE = 0x15,
            RESULT_UPDATE_MSG = 0x17,
            RACE_POOL_DIV_UPDATE_MSG = 0x1D,
            MEETING_POOL_DIV_UPDATE_MSG = 0x1F,

            MEETING_PAY_UPDATE_MSG = 0x25,
            RACE_PAY_UPDATE_MSG = 0x26,
            RACE_POOL_PAY_UPDATE_MSG = 0x27,
            MEETING_POOL_PAY_UPDATE_MSG = 0x28,
            LEG_BREAKDOWN_UPDATE_MSG = 0x40,
            RACE_POOL_WILL_PAY_UPDATE_MSG = 0x2D,
            MEETING_POOL_WILL_PAY_UPDATE_MSG = 0x2E,

            SUBSTITUTE_UPDATE_MSG = 0x39,
            TIME_SYNC_MSG = 0x3B,

            MEETING_POOL_TOTAL_UPDATE = 0x43,
            WEIGHED_IN_UPDATE_MSG = 0x44,
            POOL_SUBSTITUTE_UPDATE = 0x45,
            COMPLEX_RACE_POOL_TOTAL_UPDATE = 0x46,
            COMPLEX_RACE_POOL_DIVIDEND_UPDATE = 0x47,
            MATRIX_POOL_DIVIDEND_UPDATE = 0x48,

            ACCOUNT_LOGIN = 0x1000,
            ACCOUNT_LOGOUT = 0x1001,
            SELL_BET_REQ_MSG = 0x1002,
            PAY_BET_REQ_MSG = 0x1003,
            CANCEL_BET_REQ_MSG = 0x1004,
            RUOk_REQUEST_MSG = 0x1005,
            RUOk_REPLY_MSG = 0x1006,
            MSN_REQ_MSG = 0x1007,
            CURRENT_MSN_REQ_MSG = 0x1008,
            SINGLE_MSN_REQ_MSG = 0x1009,
            END_RACING_UPDATE_MSG = 0x100A,
            CANCEL_BET_ID_REQ_MSG = 0x100B,
            MEETING_END_DATE_REQ_MSG = 0x100E,
            CURRENT_BALANCE_REQ_MSG = 0x100F,

            SUPER_COMPLEX_POOL_DIVIDEND_UPDATE = 0x2000,
        }

        public enum ActionCode : ushort
        {
            ACTION_FAIL = 0x00,
            ACTION_SUCCESS = 0x01,
            ACTION_ON = 0x03,
            ACTION_ABANDONED = 0x04,
            ACTION_CANCELLED = 0x05,
            ACTION_DELAYED = 0x06,
            ACTION_POSTPONED = 0x07,
            ACTION_RESCHEDULED = 0x08,
            ACTION_VOID = 0x09,
            ACTION_SALES_OPEN = 0x0A,
            ACTION_SALES_CLOSED = 0x0B,
            ACTION_NORMAL = 0x0C,
            ACTION_RUNNING = 0x0D,
            ACTION_BROUGHT_DOWN = 0x0E,
            ACTION_FALLEN = 0x0F,
            ACTION_LEFT_AT_START = 0x10,
            ACTION_NON_RUNNER = 0x11,
            ACTION_RODE_OVER = 0x12,
            ACTION_PULLED_UP = 0x13,
            ACTION_REFUSED = 0x14,
            ACTION_SLIPPED_UP = 0x15,
            ACTION_UNSEATED_RIDER = 0x16,
            ACTION_WITHDRAWN = 0x17,
            ACTION_BOOK_OPEN = 0x18,
            ACTION_BOOK_CLOSED = 0x19,
            ACTION_PARTIAL_RESULT = 0x1A,
            ACTION_FULL_RESULT = 0x1B,
            ACTION_OFF = 0x1F,
            ACTION_CLOSED = 0x20,
            ACTION_PAY_OPEN = 0x21,
            ACTION_PAY_CLOSED = 0x22,
            ACTION_TOTE_SUBSTITUTE = 0x27,
            ACTION_UNKNOWN = 0x99,
            ACTION_LOGIN = 0x1000,
            ACTION_LOGOUT = 0x1001,
            ACTION_END_RACING_LOGOUT = 0x1002
        }

        public enum ErrorCode : ushort
        {
            SUCCESS = 0x0000,
            INTERNAL_ERROR = 0x0001,
            COMMS_FAILURE = 0x0002,
            INVALID_MESSAGE = 0x0003,
            INVALID_MESSAGE_TYPE = 0x0004,
            INVALID_DATE = 0x000A,
            INVALID_COURSE = 0x000E,
            INVALID_MEETING = 0x000F,
            INVALID_RACE = 0x0011,
            INVALID_POOL = 0x0012,
            INVALID_RUNNER = 0x0013,
            INVALID_RACECARD_NOT_READY = 0x0023,
            INVALID_MESSAGE_LENGTH = 0x0024,
            INVALID_NOF_BETS = 0x03E8,
            INVALID_UNIT_STAKE = 0x03E9,
            INVALID_TOTAL_UNIT_STAKE = 0x03EA,
            INVALID_TOTAL_TAX = 0x03EB,
            INVALID_BET_SELECTION = 0x03F3,
            INVALID_DUPLICATE_SELECTION = 0x03F4,
            INVALID_RACE_SELECTION = 0x03F9,
            INVALID_BET_TYPE = 0x03FC,
            INVALID_TSN = 0x03FF,
            INVALID_BET_OPTION = 0x0402,
            INVALID_BET_ID = 0x0410,
            INVALID_FAVOURITE = 0x0BB8,
            INVALID_MEETING_CANCELLED = 0x0BBA,
            INVALID_RACE_CANCELLED = 0x0BBB,
            INVALID_RACE_VOID = 0x0BBC,
            INVALID_RACE_OFF = 0x0BBD,
            INVALID_RACE_CLOSED = 0x0BBE,
            INVALID_POOL_CANCELLED = 0x0BBF,
            MULTIPLE_MEETINGS = 0x0BC4,
            MULTIPLE_RACES = 0x0BC5,
            NO_SELECTIONS = 0x0BC6,
            MEETING_UNAVAILABLE = 0x0BC7,
            RACE_UNAVAILABLE = 0x0BC8,
            POOL_UNAVAILABLE = 0x0BC9,
            INVALID_RUNNER_UNAVAILABLE = 0x0BCA,
            INVALID_NON_RUNNER = 0x0BCB,
            INVALID_SALES_NOT_OPENED = 0x0BCC,
            CANCEL_NOT_TODAY = 0x0BD2,
            SELL_BET_FAILED = 0x1389,
            GET_BET_FAILED = 0x138B,
            BET_ALREADY_CANCELLED = 0x138E,
            BET_STATUS_FAILED = 0x1391,
            INVALID_BET_NOT_SETTLED = 0x1394,
            INVALID_ACCOUNT_NOT_LOGGED_ON = 0x1B59,
            INSUFFICIENT_FUNDS = 0x1B5D,
            INVALID_MEETING_NUMBER = 0x234E
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
            WIN = 0x001,
            PLACE = 0x002,
            EXACTA = 0x003,
            TRIFECTA = 0x004,
            JACKPOT = 0x005,
            PLACEPOT = 0x006,
            QUADPOT = 0x007,
            SCOOP6 = 0x008,
            SWINGER = 0x009,
            TOTEDOUBLE = 0x0A,
            TOTETREBLE = 0x0B,
            SUPER7 = 0x0C,
            SUPERJACKPOT7 = 0x0E,
            PLACEPOT7 = 0x0F,
            SUPERJACKPOT8 = 0x10,
            PLACEPOT8 = 0x11,
            QUADDIE = 0x12,
            THURSDAY_MILLIONS = 0x13,
            QUINELLA = 0x14,
            TRIO = 0x15,
            DOUBLETRIO = 0x16,
            TRIPLETRIO = 0x17,
            SIXUP = 0x18
        }

        public enum BetOption : ushort
        {
            UNKNOWN_OPTION = 0x000,
            NO_OPTION = 0x001,
            STRAIGHT = 0x002,
            PERMUTATION = 0x004,
            BANKER = 0x008,
            FLOATING_BANKER = 0x010,
            EACH_WAY = 0x020
        }
    }
}
