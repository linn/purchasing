namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System.Collections.Generic;

    public interface IPurchaseOrderService
    {
        public void AllowOverbook(
            PurchaseOrder current,
            string allowOverBook,
            decimal? overbookQty,
            IEnumerable<string> privileges);

        public ProcessResult SendPdfEmail(
            string html,
            string emailAddress,
            int orderNumber,
            bool bcc,
            int currentUserId,
            PurchaseOrder order);
    }
}
