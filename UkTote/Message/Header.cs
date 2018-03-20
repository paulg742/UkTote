using BinarySerialization;

namespace UkTote.Message
{
    public class Header : MessageBase
    {
        [Ignore]
        protected override ushort BodyLength
        {
            get
            {
                return 0;
            }
        }
    }
}
