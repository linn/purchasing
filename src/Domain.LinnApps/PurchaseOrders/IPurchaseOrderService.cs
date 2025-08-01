﻿namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System.Collections.Generic;

    public interface IPurchaseOrderService
    {
        PurchaseOrder AllowOverbook(
            PurchaseOrder current,
            string allowOverBook,
            decimal? overbookQty,
            IEnumerable<string> privileges);

        ProcessResult SendPdfEmail(string emailAddress, int orderNumber, bool bcc, int currentUserId);

        ProcessResult SendSupplierAssemblyEmail(int orderNumber);

        ProcessResult SendFinanceAuthRequestEmail(int currentUserId, int orderNumber);

        PurchaseOrder UpdateOrder(
            PurchaseOrder current, PurchaseOrder updated, IEnumerable<string> privileges);

        PurchaseOrder FillOutUnsavedOrder(PurchaseOrder order, int userId);

        string GetOrderNotesForBuyer(PurchaseOrder order);

        ProcessResult AuthorisePurchaseOrder(
            PurchaseOrder order, int userNumber, IEnumerable<string> privileges);

        ProcessResult AuthoriseMultiplePurchaseOrders(IList<int> orderNumbers, int userNumber);

        ProcessResult EmailMultiplePurchaseOrders(
            IList<int> orderNumbers, int userNumber, bool copyToSelf);

        string GetPurchaseOrderAsHtml(int orderNumber);

        PurchaseOrder CreateOrder(PurchaseOrder order, IEnumerable<string> privileges, out bool createCreditNote);

        void CreateMiniOrder(PurchaseOrder order);

        ProcessResult EmailDept(int orderNumber, int userId);

        PurchaseOrder CancelOrder(
            int orderNumber, int cancelledBy, string reason, IEnumerable<string> privileges);

        PurchaseOrder UnCancelOrder(int orderNumber, IEnumerable<string> privileges);

        PurchaseOrder FilCancelLine(
            int orderNumber,
            int line,
            int filCancelledBy,
            string reason,
            IEnumerable<string> privileges);

        PurchaseOrder UnFilCancelLine(int orderNumber, int line, IEnumerable<string> privileges);

        PurchaseOrder SwitchOurQtyAndPrice(
            int orderNumber,
            int orderLine,
            int employeeId,
            IEnumerable<string> privileges);
    }
}
