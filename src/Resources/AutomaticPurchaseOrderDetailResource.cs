namespace Linn.Purchasing.Resources
{
    public class AutomaticPurchaseOrderDetailResource
    {
        public int Id { get; set; }

        public int Sequence { get; set; }

        public string PartNumber { get; set; }

        public int SupplierId { get; set; }

        public string SupplierName { get; set; }

        public int OrderNumber { get; set; }

        public decimal Quantity { get; set; }

        public decimal QuantityRecommended { get; set; }

        public string RecommendationCode { get; set; }

        public string OrderLog { get; set; }

        public string CurrencyCode { get; set; }

        public decimal CurrencyPrice { get; set; }
        
        public decimal BasePrice { get; set; }

        public string RequestedDate { get; set; }

        public string OrderMethod { get; set; }

        public string IssuePartsToSupplier { get; set; }

        public string IssueSerialNumbers { get; set; }

        public string AuthorisedAtCreation { get; set; }
    }
}
