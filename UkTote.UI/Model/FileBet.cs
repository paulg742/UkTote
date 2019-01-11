using System;
using System.Globalization;
using UkTote.Message;

namespace UkTote.UI.Model
{
    public class FileBet
    {
        public string Raw { get; set; }
        public bool IsValid { get; set; }
        public string Error { get; set; }
        public BetRequest Request { get; set; }

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

            if (fileBet.IsValid)
            {
                if (int.TryParse(fields[2], out int raceNumber))
                {
                    request.RaceNumber = raceNumber;
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
                if (Enum.TryParse(fields[5], out Enums.BetCode betCode))
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
                if (Enum.TryParse(fields[6], out Enums.BetOption betOption))
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
                var selections = fields[7].Split(',');
                request.Selections = Array.ConvertAll(selections, int.Parse);
                fileBet.Request = request;
            }

            return fileBet;
        }
    }
}
