namespace UkTote.Message
{
    public interface IRacePoolUpdate: IUpdate
    {
        ushort RaceNumber { get; }
        ushort PoolNumber { get; }
    }
}
