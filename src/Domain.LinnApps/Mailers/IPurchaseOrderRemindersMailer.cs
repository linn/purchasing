namespace Linn.Purchasing.Domain.LinnApps.Mailers
{
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.Keys;

    public interface IPurchaseOrderRemindersMailer
    {
        void SendDeliveryReminder(IEnumerable<PurchaseOrderDeliveryKey> deliveryKeys, bool test = false);
    }
}
