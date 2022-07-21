namespace Linn.Purchasing.Domain.LinnApps.Dispatchers  // will move to common
{
    public interface IMessageDispatcher<in T>
    {
        void Dispatch(T data);
    }
}
