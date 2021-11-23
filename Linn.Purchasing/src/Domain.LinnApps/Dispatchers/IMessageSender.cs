namespace Linn.Purchasing.Domain.LinnApps.Dispatchers
{
    public interface IMessageSender
    {
        void SendMessage(string text);
    }
}
