using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UkTote.Message;

namespace UkTote.UI.Model
{
    public class FileBet
    {
        public string Raw { get; set; }
        public bool IsValid { get; set; }
        public string Error { get; set; }
        public BetRequest Request { get; set; }
        public BetReply Result { get; set; }

        public static FileBet Parse(string text)
        {
            var fileBet = new FileBet() { Raw = text, IsValid = true };
            var request = new BetRequest();

            var fields = text.Split('|');
            if (fields.Length < 8)
            {
                fileBet.IsValid = false;
                fileBet.Error = "Invalid format";
            }

            if (fileBet.IsValid)
            {
                if (DateTime.TryParseExact(fields[0], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
                {
                    request.ForDate = dateTime;
                }
                else
                {
                    fileBet.IsValid = false;
                    fileBet.Error = $"Invalid date format: {fields[0]}";
                }
            }

            if (fileBet.IsValid)
            {
                if (int.TryParse(fields[1], out int meetingNumber))
                {
                    request.MeetingNumber = meetingNumber;
                }
                else
                {
                    fileBet.IsValid = false;
                    fileBet.Error = $"Invalid meeting number: {fields[1]}";
                }
            }

            int raceNumber = 0;
            if (fileBet.IsValid)
            {
                if (int.TryParse(fields[2], out raceNumber))
                {
                    //request.RaceNumber = raceNumber;
                }
                else
                {
                    fileBet.IsValid = false;
                    fileBet.Error = $"Invalid race number: {fields[2]}";
                }
            }

            if (fileBet.IsValid)
            {
                if (int.TryParse(fields[3], out int unitStake))
                {
                    request.UnitStake = unitStake;
                }
                else
                {
                    fileBet.IsValid = false;
                    fileBet.Error = $"Invalid unit stake: {fields[3]}";
                }
            }

            if (fileBet.IsValid)
            {
                if (int.TryParse(fields[4], out int totalStake))
                {
                    request.TotalStake = totalStake;
                }
                else
                {
                    fileBet.IsValid = false;
                    fileBet.Error = $"Invalid total stake: {fields[4]}";
                }
            }

            if (fileBet.IsValid)
            {
                if (Enum.TryParse(fields[5], true, out Enums.BetCode betCode))
                {
                    request.BetCode = betCode;
                }
                else
                {
                    fileBet.IsValid = false;
                    fileBet.Error = $"Invalid bet code: {fields[5]}";
                }
            }

            if (fileBet.IsValid)
            {
                if (Enum.TryParse(fields[6], true, out Enums.BetOption betOption))
                {
                    request.BetOption = betOption;
                }
                else
                {
                    fileBet.IsValid = false;
                    fileBet.Error = $"Invalid bet option: {fields[6]}";
                }
            }

            if (fileBet.IsValid)
            {
                //var selections = fields[7].Split(',');
                request.Selections = ParseSelections(request, raceNumber, fields[7]);
                fileBet.Request = request;
            }

            return fileBet;
        }

        public static Selection[] ParseSelections(BetRequest betRequest, int raceNumber, int[] selections, int raceOffset = 0)
        {
            return selections.Select(s => new Selection()
            {
                MeetingNumber = (ushort)betRequest.MeetingNumber,
                RaceNumber = (ushort)(raceNumber + raceOffset),
                HorseNumber = (ushort)(s > 900 ? s - 900 : s),
                IsBanker = (byte) (s > 900 ? 1 : 0)
            }).ToArray();
        }

        public static Selection[] ParseMultiRaceSelections(BetRequest betRequest, int[] selections)
        {
            return selections.Select(s => new Selection()
            {
                MeetingNumber = (ushort)betRequest.MeetingNumber,
                RaceNumber = (ushort)(s / 100),
                HorseNumber = (ushort)(s % 100),
                IsBanker = 0
            }).ToArray();
        }

        public static Selection[] ParseSelections(BetRequest betRequest, int raceNumber, string selections)
        {
            var ret = new List<Selection>();
            switch(betRequest.BetCode)
            {
                // multi-race, multi-selection
                case Enums.BetCode.Trio:
                case Enums.BetCode.Doubletrio:
                case Enums.BetCode.Tripletrio:
                case Enums.BetCode.Sixup:
                    var legs = selections.Split('/');
                    for (var i=0; i<legs.Length; ++i)
                    {
                        ret.AddRange(ParseSelections(betRequest, raceNumber, Array.ConvertAll(legs[i].Split(','), int.Parse), i));
                    }
                    break;

                // multi-race:
                case Enums.BetCode.Scoop6:
                case Enums.BetCode.Super7:
                case Enums.BetCode.Jackpot:
                case Enums.BetCode.Placepot:
                case Enums.BetCode.Quadpot:
                case Enums.BetCode.Quaddie:
                case Enums.BetCode.Totedouble:
                case Enums.BetCode.Totetreble:
                case Enums.BetCode.Superjackpot7:
                case Enums.BetCode.Placepot7:
                case Enums.BetCode.Superjackpot8:
                case Enums.BetCode.Placepot8:
                    ret.AddRange(ParseMultiRaceSelections(betRequest, Array.ConvertAll(selections.Split(','), int.Parse)));
                    break;

                default:
                    ret.AddRange(ParseSelections(betRequest, raceNumber, Array.ConvertAll(selections.Split(','), int.Parse)));
                    break;

            }
            return ret.ToArray();
        }
    }
}
