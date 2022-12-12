namespace Linn.Purchasing.Domain.LinnApps
{
    public class AuthorisedAction
    {
        public const string PartSupplierCreate = "part-supplier.create";

        public const string PartSupplierUpdate = "part-supplier.update";

        public const string SigningLimitAdmin = "purchasing.signing-limits.admin";

        public const string SupplierCreate = "purchasing.supplier.create";

        public const string SupplierUpdate = "purchasing.supplier.update";

        public const string SupplierClose = "purchasing.supplier.close";

        public const string SupplierHoldChange = "purchasing.supplier.hold-change";

        public const string PurchaseOrderUpdate = "purchase-order.update";

        public const string PurchaseOrderCancel = "purchase-order.cancel";
        
        public const string PurchaseOrderFilCancel = "purchase-order.fil-cancel";

        public const string PurchaseOrderReqFinanceCheck = "purchase-order-req.finance-check";

        public const string PurchaseOrderCreate = "purchase-order.create";

        public const string PurchaseOrderCreateForOtherUser = "purchase-order.create-for-other-user";

        public const string PlCreditDebitNoteClose = "purchasing.pl-credit-debit-note.close";

        public const string PlCreditDebitNoteCancel = "purchasing.pl-credit-debit-note.cancel";

        public const string PlCreditDebitNoteUpdate = "purchasing.pl-credit-debit-note.update";

        public const string MrpRun = "purchasing.mrp.run-mrp";

        public const string ForecastingApplyPercentageChange = "purchasing.forecasting.apply-percentage-change";

        public const string SendEdi = "purchasing.edi.send";

        public const string PurchaseOrderAuthorise = "purchase-order.authorise";

        public const string ChangeBomType = "part.change-bom-type";

        public const string ApproveChangeRequest = "change-request.approve";

        public const string AdminChangeRequest = "change-request.admin";
    }
}
