namespace Linn.Purchasing.Resources
{
    using Linn.Common.Resources;

    public class SupplierResource : HypermediaResource
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int LedgerStream { get; set; }

        public string VendorManager { get; set; }

        public int? Planner { get; set; }

        public string Currency { get; set; }

        public string WebAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string OrderContactMethod { get; set; }

        public string InvoiceContactMethod { get; set; }

        public string SuppliersReference { get; set; }

        public string LiveOnOracle { get; set; }
    }
}
