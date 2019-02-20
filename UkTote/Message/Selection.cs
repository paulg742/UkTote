using System;
using System.Linq;
using BinarySerialization;

namespace UkTote.Message
{
    public class Selection
    {
        public Selection()
        {
            // requests need these initialized
            Reserved = new byte[6];
        }

        [FieldOrder(0)]
        public ushort MeetingNumber { get; set; }

        [FieldOrder(1)]
        public ushort RaceNumber { get; set; }

        [FieldOrder(2)]
        public ushort HorseNumber { get; set; }

        [FieldOrder(3)]
        [FieldLength(6)]
        public byte[] Reserved { get; set; }

        [FieldOrder(4)]
        public byte IsBanker { get; set; }

        public static Selection[] Create(int meetingNumber, int raceNumber, int[] selections)
        {
            return selections.Select(s => new Selection()
            {
                MeetingNumber = (ushort)meetingNumber,
                RaceNumber = (ushort)raceNumber,
                HorseNumber = (ushort)((s > 900) ? s - 900 : s),
                IsBanker = (byte)((s > 900) ? 1 : 0)
            }).ToArray();
        }

        public static Selection[] Create(int meetingNumber, (int raceNumber, int selection)[] selections)
        {
            return selections.Select(s => new Selection()
            {
                MeetingNumber = (ushort)meetingNumber,
                RaceNumber = (ushort)s.raceNumber,
                HorseNumber = (ushort)((s.selection > 900) ? s.selection - 900 : s.selection),
                IsBanker = (byte)((s.selection > 900) ? 1 : 0)
            }).ToArray();
        }

        public static Selection[] Create(int meetingNumber, int[] selections)
        {
            return selections.Select(s => new Selection()
            {
                MeetingNumber = (ushort)meetingNumber,
                RaceNumber = (ushort)(s / 100),
                HorseNumber = (ushort)(s % 100),
                IsBanker = 0
            }).ToArray();
        }
    }
}
