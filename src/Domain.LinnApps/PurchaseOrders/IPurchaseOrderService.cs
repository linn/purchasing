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

        ProcessResult SendPdfEmail(string emailAddress, int orderNumber, bool bcc, int currentUserId);

        ProcessResult SendSupplierAssemblyEmail(int orderNumber);

        ProcessResult SendFinanceAuthRequestEmail(int currentUserId, int orderNumber);

        PurchaseOrder UpdateOrder(PurchaseOrder current, PurchaseOrder updated, IEnumerable<string> privileges);

        PurchaseOrder FillOutUnsavedOrder(PurchaseOrder order, int userId);

        ProcessResult AuthorisePurchaseOrder(PurchaseOrder order, int userNumber, IEnumerable<string> privileges);

        ProcessResult AuthoriseMultiplePurchaseOrders(IList<int> orderNumbers, int userNumber);

        ProcessResult EmailMultiplePurchaseOrders(IList<int> orderNumbers, int userNumber, bool copyToSelf);

        string GetPurchaseOrderAsHtml(int orderNumber);

        PurchaseOrder CreateOrder(PurchaseOrder order, IEnumerable<string> privileges);

        void CreateMiniOrder(PurchaseOrder order);

        ProcessResult EmailDept(int orderNumber, int userId);
    }
}
