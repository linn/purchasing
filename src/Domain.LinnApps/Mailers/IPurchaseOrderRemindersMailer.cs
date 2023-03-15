namespace Linn.Purchasing.Domain.LinnApps.Mailers
{
    public interface IPurchaseOrderRemindersMailer
    {
        void SendDeliveryReminder(int orderNumber, int orderLine, int deliverySeq, bool test = false);
    }
}
