namespace UkTote
{
    using Message;
    using System;

    public class BetReply
    {
        public string Tsn { get; set; }
        public uint BetId { get; set; }
        public Guid? Ref { get; set; }
        public Enums.ErrorCode ErrorCode { get; set; }
        public string ErrorText { get; set; }
    }
}
