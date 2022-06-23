namespace Linn.Purchasing.Resources
{
    using System.Collections.Generic;

    public class PurchaseOrderDetailResource
    {
        public decimal? BaseDetailTotal { get; set; }

        public decimal BaseNetTotal { get; set; }

        public decimal? BaseOrderUnitPrice { get; set; }

        public decimal? BaseOurUnitPrice { get; set; }

        public decimal? BaseVatTotal { get; set; }

        public string Cancelled { get; set; }

        public IEnumerable<CancelledPurchaseOrderDetailResource> CancelledDetails { get; set; }

        public EmployeeResource DeliveryConfirmedBy { get; set; }

        public string DeliveryInstructions { get; set; }

        public DepartmentResource Department { get; set; }

        public decimal? DetailTotalCurrency { get; set; }

        public decimal? Duty { get; set; }

        public string InternalComments { get; set; }

        public int Line { get; set; }

        public IEnumerable<MrOrderResource> MrOrders { get; set; }

        public decimal NetTotalCurrency { get; set; }

        public NominalResource Nominal { get; set; }

        public int OrderNumber { get; set; }

        public PurchaseOrderPostingResource OrderPosting { get; set; }

        public string OrderUnitOfMeasure { get; set; }

        public decimal? OrderUnitPriceCurrency { get; set; }

        public int? OriginalOrderLine { get; set; }

        public int? OriginalOrderNumber { get; set; }

        public decimal? OurQty { get; set; }

        public string OurUnitOfMeasure { get; set; }

        public decimal? OurUnitPriceCurrency { get; set; }

        public string PartDescription { get; set; }

        public string PartNumber { get; set; }

        public IEnumerable<PurchaseOrderDeliveryResource> PurchaseDeliveries { get; set; }

        public string RohsCompliant { get; set; }

        public string StockPoolCode { get; set; }

        public string SuppliersDesignation { get; set; }

        public decimal? VatTotalCurrency { get; set; }
    }
}
