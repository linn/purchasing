namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.Parts;

    public class PurchaseOrderDetail
    {
        public decimal? BaseDetailTotal { get; set; }

        public decimal BaseNetTotal { get; set; }

        public decimal? BaseOrderUnitPrice { get; set; }

        public decimal? BaseOurUnitPrice { get; set; }

        public decimal? BaseVatTotal { get; set; }

        public string Cancelled { get; set; }

        public ICollection<CancelledOrderDetail> CancelledDetails { get; set; }

        public Employee DeliveryConfirmedBy { get; set; }

        public int? DeliveryConfirmedById { get; set; }

        public string DeliveryInstructions { get; set; }

        public decimal? DetailTotalCurrency { get; set; }

        public decimal? Duty { get; set; }

        public string InternalComments { get; set; }

        public int Line { get; set; }

        public IEnumerable<MrOrder> MrOrders { get; set; }

        public decimal NetTotalCurrency { get; set; }

        public int OrderNumber { get; set; }

        public PurchaseOrderPosting OrderPosting { get; set; }

        public string OrderUnitOfMeasure { get; set; }

        public decimal? OrderUnitPriceCurrency { get; set; }

        public int? OriginalOrderLine { get; set; }

        public int? OriginalOrderNumber { get; set; }

        public int? OurQty { get; set; }

        public string OurUnitOfMeasure { get; set; }

        public decimal? OurUnitPriceCurrency { get; set; }

        public Part Part { get; set; }

        public string PartNumber { get; set; }

        public IEnumerable<PurchaseOrderDelivery> PurchaseDeliveries { get; set; }

        public PurchaseOrder PurchaseOrder { get; set; }

        public string RohsCompliant { get; set; }

        public string StockPoolCode { get; set; }

        public string SuppliersDesignation { get; set; }

        public decimal? VatTotalCurrency { get; set; }

        public decimal? OrderConversionFactor { get; set; }
    }
}
