namespace Linn.Purchasing.Resources
{
    using Linn.Common.Resources;

    public class SupplierResource : HypermediaResource
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string VendorManager { get; set; }

        public int? Planner { get; set; }

        public string CurrencyCode { get; set; }

        public string WebAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string OrderContactMethod { get; set; }

        public string InvoiceContactMethod { get; set; }

        public string SuppliersReference { get; set; }

        public string LiveOnOracle { get; set; }

        public int? InvoiceGoesToId { get; set; }

        public string InvoiceGoesToName { get; set; }

        public string ExpenseAccount { get; set; }

        public int PaymentDays { get; set; }

        public string PaymentMethod { get; set; }

        public string PaysInFc { get; set; }

        public string CurrencyName { get; set; }

        public string ApprovedCarrier { get; set; }
    }
}
