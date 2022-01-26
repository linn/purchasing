namespace Linn.Purchasing.Domain.LinnApps.Suppliers
{
    public class Supplier
    {
        public int SupplierId { get; set; }

        public string Name { get; set; }

        public int LedgerStream { get; set; }

        public string VendorManager { get; set; }

        public int? Planner { get; set; }
    }
}
