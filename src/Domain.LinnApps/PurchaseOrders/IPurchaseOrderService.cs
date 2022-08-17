namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System.Collections.Generic;

    public interface IPurchaseOrderService
    {
        void AllowOverbook(
            PurchaseOrder current,
            string allowOverBook,
            decimal? overbookQty,
            IEnumerable<string> privileges);

        ProcessResult SendPdfEmail(
            string html,
            string emailAddress,
            int orderNumber,
            bool bcc,
            int currentUserId,
            PurchaseOrder order);

        PurchaseOrder UpdateOrder(PurchaseOrder current, PurchaseOrder updated, IEnumerable<string> privileges);

        PurchaseOrder FillOutUnsavedOrder(PurchaseOrder order, int userId);
    }
}
