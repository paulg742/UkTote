using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using UkTote.Message;

namespace UkTote
{
    public partial class ToteGateway
    {
        public Task<RacecardReply> GetRacecardFast(DateTime forDate, bool includePools)
        {
            var tcs = new TaskCompletionSource<RacecardReply>();
            Action<RacecardReply> racecardHandler = null;
            Action<MeetingReply> meetingHandler = null;
            Action<RaceReply> raceHandler = null;
            Action<RacePoolReply> racePoolHandler = null;
            Action<RunnerReply> runnerHandler = null;
            Action<MeetingPoolReply> meetingPoolHandler = null;
            RacecardReply racecard = null;

            racecardHandler += (reply) =>
            {
                racecard = reply;
                for (int i = 1; i <= racecard.NumMeetings; ++i)
                {
                    GetMeetingAsync(i);
                }
            };

            meetingHandler += (reply) =>
            {
                if (racecard.Meetings == null) racecard.Meetings = new Dictionary<int, MeetingReply>();

                if (!racecard.Meetings.ContainsKey(reply.MeetingNumber))
                {
                    racecard.Meetings[reply.MeetingNumber] = reply.ActionCode == Enums.ActionCode.ACTION_FAIL
                        ? null
                        : reply;
                    for (int i = 1; i <= reply.NumberOfRaces; ++i)
                    {
                        GetRaceAsync(reply.MeetingNumber, i);
                    }

                    if (includePools)
                    {
                        for (int i = 1; i <= reply.NumberOfMultiLegPools; ++i)
                        {
                            GetMeetingPoolAsync(reply.MeetingNumber, i);
                        }
                    }
                }
            };

            raceHandler += (reply) =>
            {
                if (!racecard.Meetings.ContainsKey(reply.MeetingNumber)) return; // this is an error, we should have the meeting reply before the race reply
                if (racecard.Meetings[reply.MeetingNumber].Races == null) racecard.Meetings[reply.MeetingNumber].Races = new Dictionary<int, RaceReply>();
                if (!racecard.Meetings[reply.MeetingNumber].Races.ContainsKey(reply.RaceNumber))
                {
                    racecard.Meetings[reply.MeetingNumber].Races[reply.RaceNumber] = reply.ActionCode == Enums.ActionCode.ACTION_FAIL
                        ? null
                        : reply;

                    for (int i = 1; i <= reply.NumberOfDeclaredRunners; ++i)
                    {
                        GetRunnerAsync(reply.MeetingNumber, reply.RaceNumber, i);
                    }

                    if (includePools)
                    {
                        for (int i = 1; i <= reply.NumberOfRacePools; ++i)
                        {
                            GetRacePoolAsync(reply.MeetingNumber, reply.RaceNumber, i);
                        }
                    }
                }
            };

            runnerHandler += (reply) =>
            {
                if (!racecard.Meetings.ContainsKey(reply.MeetingNumber)) return; // this is an error, we should have the meeting reply before the race reply
                if (!racecard.Meetings[reply.MeetingNumber].Races.ContainsKey(reply.RaceNumber)) return; // this is an error, we should have the race reply before the runner reply
                if (racecard.Meetings[reply.MeetingNumber].Races[reply.RaceNumber].Runners == null) racecard.Meetings[reply.MeetingNumber].Races[reply.RaceNumber].Runners = new Dictionary<int, RunnerReply>();
                if (!racecard.Meetings[reply.MeetingNumber].Races[reply.RaceNumber].Runners.ContainsKey(reply.RunnerNumber))
                {
                    racecard.Meetings[reply.MeetingNumber].Races[reply.RaceNumber].Runners[reply.RunnerNumber] = reply.ActionCode == Enums.ActionCode.ACTION_FAIL
                        ? null
                        : reply;
                }

                _logger.DebugFormat("Meeting:{0} Race:{1} RunnerNumber:{2} NumberOfDeclaredRunners:{3} RunnersReceived:{4}",
                    reply.MeetingNumber,
                    reply.RaceNumber,
                    reply.RunnerNumber,
                    racecard.Meetings[reply.MeetingNumber].Races[reply.RaceNumber].NumberOfDeclaredRunners,
                    racecard.Meetings[reply.MeetingNumber].Races[reply.RaceNumber].Runners.Count);

                if (racecard.Meetings[reply.MeetingNumber].Races[reply.RaceNumber].IsComplete)
                {
                    _logger.DebugFormat("Meeting:{0} RaceNumber:{1} complete", reply.MeetingNumber, reply.RaceNumber);
                    if (racecard.IsComplete)
                    {
                        _logger.DebugFormat("Racecard complete!");
                        OnRacecard -= racecardHandler;
                        OnMeeting -= meetingHandler;
                        OnRace -= raceHandler;
                        OnRunner -= runnerHandler;
                        OnRacePool -= racePoolHandler;
                        OnMeetingPool -= meetingPoolHandler;
                        tcs.TrySetResult(racecard);
                    }
                    else
                    {
                        _logger.DebugFormat("Racecard not complete :(");
                    }
                }
            };

            racePoolHandler += (reply) =>
            {
                if (!racecard.Meetings.ContainsKey(reply.MeetingNumber))
                {
                    _logger.ErrorFormat("Pool received for meeting:{0} before meeting", reply.MeetingNumber);
                    return; // this is an error, we should have the meeting reply before the race reply
                }

                if (!racecard.Meetings[reply.MeetingNumber].Races.ContainsKey(reply.RaceNumber))
                {
                    _logger.ErrorFormat("Pool received for meeting:{0} race:{1} before race", reply.MeetingNumber, reply.RaceNumber);
                    return; // this is an error, we should have the race reply before the racepool reply
                }

                if (racecard.Meetings[reply.MeetingNumber].Races[reply.RaceNumber].RacePools == null)
                {
                    racecard.Meetings[reply.MeetingNumber].Races[reply.RaceNumber].RacePools = new Dictionary<int, RacePoolReply>();
                }
                if (!racecard.Meetings[reply.MeetingNumber].Races[reply.RaceNumber].RacePools.ContainsKey(reply.PoolNumber))
                {
                    racecard.Meetings[reply.MeetingNumber].Races[reply.RaceNumber].RacePools[reply.PoolNumber] = reply.ActionCode == Enums.ActionCode.ACTION_FAIL
                        ? null
                        : reply;
                }

                _logger.DebugFormat("Meeting:{0} Race:{1} PoolNumber:{2} NumPools:{3} PoolsReceived:{4}",
                    reply.MeetingNumber,
                    reply.RaceNumber,
                    reply.PoolNumber,
                    racecard.Meetings[reply.MeetingNumber].Races[reply.RaceNumber].NumberOfRacePools,
                    racecard.Meetings[reply.MeetingNumber].Races[reply.RaceNumber].RacePools.Count);

                if (racecard.Meetings[reply.MeetingNumber].Races[reply.RaceNumber].IsComplete)
                {
                    if (racecard.IsComplete)
                    {
                        OnRacecard -= racecardHandler;
                        OnMeeting -= meetingHandler;
                        OnRace -= raceHandler;
                        OnRunner -= runnerHandler;
                        OnRacePool -= racePoolHandler;
                        OnMeetingPool -= meetingPoolHandler;
                        tcs.TrySetResult(racecard);
                    }
                }

            };

            meetingPoolHandler += (reply) =>
            {
                if (!racecard.Meetings.ContainsKey(reply.MeetingNumber)) return; // this is an error, we should have the meeting reply before the meeting pool reply
                if (racecard.Meetings[reply.MeetingNumber].MeetingPools == null) racecard.Meetings[reply.MeetingNumber].MeetingPools = new Dictionary<int, MeetingPoolReply>();
                if (!racecard.Meetings[reply.MeetingNumber].MeetingPools.ContainsKey(reply.MeetingPoolNumber))
                {
                    racecard.Meetings[reply.MeetingNumber].MeetingPools[reply.MeetingPoolNumber] = reply.ActionCode == Enums.ActionCode.ACTION_FAIL
                        ? null
                        : reply;
                }
                if (racecard.Meetings[reply.MeetingNumber].IsComplete)
                {
                    _logger.DebugFormat("Meeting:{0} complete", reply.MeetingNumber);
                    if (racecard.IsComplete)
                    {
                        _logger.DebugFormat("Racecard complete!");
                        OnRacecard -= racecardHandler;
                        OnMeeting -= meetingHandler;
                        OnRace -= raceHandler;
                        OnRunner -= runnerHandler;
                        OnRacePool -= racePoolHandler;
                        OnMeetingPool -= meetingPoolHandler;
                        tcs.TrySetResult(racecard);
                    }
                    else
                    {
                        _logger.DebugFormat("Racecard not complete :(");
                    }
                }
            };

            OnRacecard += racecardHandler;
            OnMeeting += meetingHandler;
            OnRace += raceHandler;
            OnRunner += runnerHandler;
            OnRacePool += racePoolHandler;
            OnMeetingPool += meetingPoolHandler;

            GetRacecardAsync(forDate);
            return tcs.Task;
        }

        public Task<IList<MeetingReply>> GetMeetings(int numMeetings)
        {
            var tcs = new TaskCompletionSource<IList<MeetingReply>>();
            Action<MeetingReply> handler = null;
            var responses = new ConcurrentDictionary<int, MeetingReply>();

            handler += (reply) =>
            {
				if (!responses.ContainsKey(reply.MeetingNumber))
                {
                    responses[reply.MeetingNumber] = reply.ActionCode == Enums.ActionCode.ACTION_FAIL
						? null
						: reply;
                }
                
				if (responses.Count >= numMeetings)
                {
                    tcs.TrySetResult(responses.Values.ToList());
                    OnMeeting -= handler;
                }
            };

            OnMeeting += handler;
            for (int i = 1; i <= numMeetings; ++i)
            {
                GetMeetingAsync(i);
            }
            return tcs.Task;
        }

		public Task<IList<RaceReply>> GetRaces(int meetingNumber, int numRaces)
        {
            var tcs = new TaskCompletionSource<IList<RaceReply>>();
            Action<RaceReply> handler = null;
            var responses = new ConcurrentDictionary<int, RaceReply>();

            handler += (reply) =>
            {
                if (!responses.ContainsKey(reply.RaceNumber))
                {
                    responses[reply.RaceNumber] = reply.ActionCode == Enums.ActionCode.ACTION_FAIL
						? null
						: reply;
                }

                if (responses.Count >= numRaces)
                {
                    tcs.TrySetResult(responses.Values.ToList());
                    OnRace -= handler;
                }
            };

            OnRace += handler;
            for (int i = 1; i <= numRaces; ++i)
            {
                GetRaceAsync(meetingNumber, i);
            }
            return tcs.Task;
        }

        public Task<IList<RunnerReply>> GetRunners(int meetingNumber, int raceNumber, int numRunners)
        {
            var tcs = new TaskCompletionSource<IList<RunnerReply>>();
            Action<RunnerReply> handler = null;
            var responses = new ConcurrentDictionary<int, RunnerReply>();

            handler += (reply) =>
            {
                if (!responses.ContainsKey(reply.RunnerNumber))
                {
                    responses[reply.RunnerNumber] = reply.ActionCode == Enums.ActionCode.ACTION_FAIL
                        ? null
                        : reply;
                }

                if (responses.Count >= numRunners)
                {
                    tcs.TrySetResult(responses.Values.ToList());
                    OnRunner -= handler;
                }
            };

            OnRunner += handler;
            for (int i = 1; i <= numRunners; ++i)
            {
                GetRunnerAsync(meetingNumber, raceNumber, i);
            }
            return tcs.Task;
        }
    }
}
