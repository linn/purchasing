namespace Linn.Purchasing.Domain.LinnApps.Dispatchers
{
    public interface IMessageDispatcher<in T>
    {
        void Dispatch(T data);
    }
}
