namespace UkTote
{
    using Message;
    public class PayEnquiryReply
    {
        public string TSN { get; set; }
        public uint PayoutAmount { get; set; }
        public uint VoidAmount { get; set; }
        public Enums.ErrorCode ErrorCode { get; set; }
        public string ErrorText { get; set; }
    }
}
