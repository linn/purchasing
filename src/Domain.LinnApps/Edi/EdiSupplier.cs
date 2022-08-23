namespace Linn.Purchasing.Domain.LinnApps.Edi
{
    public class EdiSupplier
    {
        public int SupplierId { get; set; }

        public string SupplierName { get; set; }

        public string VendorManager { get; set; }

        public string VendorManagerName { get; set; }

        public string EdiEmailAddress { get; set; }

        public int NumOrders { get; set; }
    }
}
