namespace UkTote
{
    using Message;
    public class BetReply
    {
        public string TSN { get; set; }
        public uint BetId { get; set; }
        public Enums.ErrorCode ErrorCode { get; set; }
        public string ErrorText { get; set; }
    }
}
