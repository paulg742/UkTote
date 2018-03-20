using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UkTote.Message;

namespace UkTote
{
    public interface IToteGateway
    {
        bool IsConnected { get; }
        int NextBetId { set; }

        event Action OnConnected;
        event Action<CurrentMsnReply> OnCurrentMsnReply;
        event Action<string> OnDisconnected;
        event Action<string> OnIdle;
        event Action<AccountLoginError> OnLoginError;
        event Action<AccountLoginSuccess> OnLoginSuccess;
        event Action<AccountLogoutError> OnLogoutError;
        event Action<AccountLogoutSuccess> OnLogoutSuccess;
        event Action<MeetingReply> OnMeeting;
        event Action<MeetingEndDateReply> OnMeetingEndDate;
        event Action<MeetingEndDateErrorReply> OnMeetingEndDateError;
        event Action<MeetingSalesUpdate> OnMeetingSalesUpdate;
        event Action<MsnReply> OnMsnReply;
        event Action<PayEnquiryFailed> OnPayEnquiryFailed;
        event Action<PayEnquirySuccess> OnPayEnquirySuccess;
        event Action<RaceReply> OnRace;
        event Action<RacecardReply> OnRacecard;
        event Action<RacePoolReply> OnRacePool;
        event Action<RacePoolDividendUpdate> OnRacePoolDividendUpdate;
        event Action<RacePoolSalesUpdate> OnRacePoolSalesUpdate;
        event Action<RacePoolUpdate> OnRacePoolUpdate;
        event Action<RaceSalesUpdate> OnRaceSalesUpdate;
        event Action<RaceUpdate> OnRaceUpdate;
        event Action<RaceWillPayUpdate> OnRaceWillPayUpdate;
        event Action<RunnerReply> OnRunner;
        event Action<SellBetFailed> OnSellBetFailed;
        event Action<SellBetSuccess> OnSellBetSuccess;

        bool Connect(string hostname, int port);
        void Disconnect(string reason = null);
        void Dispose();
        Task<CurrentMsnReply> GetCurrentMsn();
        Task<MeetingReply> GetMeeting(int meetingNumber);
        Task<IList<MeetingReply>> GetMeetings(int numMeetings);
        Task<MsnReply> GetMsn(int sequence);
        Task<RaceReply> GetRace(int meetingNumber, int raceNumber);
        Task<RacecardReply> GetRacecard(DateTime forDate);
        Task<RacecardReply> GetRacecardFast(DateTime forDate, bool includePools);
        Task<IList<RaceReply>> GetRaces(int meetingNumber, int numRaces);
        Task<RunnerReply> GetRunner(int meetingNumber, int raceNumber, int runnerNumber);
        Task<IList<RunnerReply>> GetRunners(int meetingNumber, int raceNumber, int numRunners);
        Task<bool> Login(string username, string password);
        Task<bool> Logout(string username);
        Task<PayEnquiryReply> PayEnquiry(string tsn);
        Task<IList<BetReply>> SellBatch(IList<BetRequest> batch);
        Task<BetReply> SellBet(DateTime forDate, int meetingNumber, int raceNumber, int unitStake, int totalStake, Enums.BetCode betCode, Enums.BetOption betOption, int[] selections);
        Task<BetReply> SellBet(DateTime forDate, int meetingNumber, int raceNumber, int unitStake, int totalStake, Enums.BetCode betCode, Enums.BetOption betOption, Selection[] selections, int? betId = null);
    }
}