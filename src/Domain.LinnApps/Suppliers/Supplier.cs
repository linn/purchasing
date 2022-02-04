namespace Linn.Purchasing.Domain.LinnApps.Suppliers
{
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    public class Supplier
    {
        public int SupplierId { get; set; }

        public string Name { get; set; }

        public string VendorManager { get; set; }

        public int? Planner { get; set; }

        public string WebAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string OrderContactMethod { get; set; }

        public string InvoiceContactMethod { get; set; }

        public string SuppliersReference { get; set; }

        public string LiveOnOracle { get; set; }

        public Supplier InvoiceGoesTo { get; set; }

        public string ExpenseAccount { get; set; }

        public int PaymentDays { get; set; }

        public string PaymentMethod { get; set; }

        public string PaysInFc { get; set; }

        public Currency Currency { get; set; }

        public string ApprovedCarrier { get; set; }
    }
}
