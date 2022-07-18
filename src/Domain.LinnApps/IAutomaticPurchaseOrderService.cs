using Linn.Purchasing.Domain.LinnApps.AutomaticPurchaseOrders;

namespace Linn.Purchasing.Domain.LinnApps
{
    public interface IAutomaticPurchaseOrderService
    {
        AutomaticPurchaseOrder CreateAutomaticPurchaseOrder(AutomaticPurchaseOrder proposedAutomaticPurchaseOrder);
    }
}
