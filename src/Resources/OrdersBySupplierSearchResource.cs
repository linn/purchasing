namespace Linn.Purchasing.Resources
{
    public class OrdersBySupplierSearchResource
    {
        public string From { get; set; }

        public int SupplierId { get; set; }

        public string To { get; set; }

        public string Returns { get; set; }

        public string Outstanding { get; set; }

        public string Cancelled { get; set; }

        public string Credits { get; set; }

        public string StockControlled { get; set; }
    }
}
