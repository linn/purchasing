namespace Linn.Purchasing.Resources
{
    using System.Collections.Generic;

    using Linn.Common.Resources;

    public class PurchaseOrderDetailResource : HypermediaResource
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

        public decimal? DetailTotalCurrency { get; set; }

        public string InternalComments { get; set; }

        public int Line { get; set; }

        public IEnumerable<MrOrderResource> MrOrders { get; set; }

        public decimal NetTotalCurrency { get; set; }

        public int OrderNumber { get; set; }

        public PurchaseOrderPostingResource OrderPosting { get; set; }

        public string OrderUnitOfMeasure { get; set; }

        public decimal? OrderUnitPriceCurrency { get; set; }

        public int? OriginalOrderLine { get; set; }

        public int? OriginalOrderNumber { get; set; }

        public decimal? OurQty { get; set; }

        public decimal? OrderQty { get; set; }

        public string OurUnitOfMeasure { get; set; }

        public decimal? OurUnitPriceCurrency { get; set; }

        public string PartDescription { get; set; }

        public string PartNumber { get; set; }

        public PartResource Part { get; set; }

        public IEnumerable<PurchaseOrderDeliveryResource> PurchaseDeliveries { get; set; }

        public string RohsCompliant { get; set; }

        public string StockPoolCode { get; set; }

        public string SuppliersDesignation { get; set; }

        public decimal? VatTotalCurrency { get; set; }

        public string FilCancelled { get; set; }

        public string DateFilCancelled { get; set; }

        public string ReasonFilCancelled { get; set; }

        public int? FilCancelledBy { get; set; }

        public string FilCancelledByName { get; set; }

        public string DrawingReference { get; set; }
    }
}
