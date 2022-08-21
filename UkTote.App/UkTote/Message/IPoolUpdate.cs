namespace UkTote.Message
{
    public interface IPoolUpdate: IUpdate
    {
        ushort PoolNumber { get; }
    }
}
