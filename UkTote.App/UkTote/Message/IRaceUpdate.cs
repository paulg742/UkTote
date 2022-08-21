namespace UkTote.Message
{
    public interface IRaceUpdate: IUpdate
    {
        ushort RaceNumber { get; }
    }
}
