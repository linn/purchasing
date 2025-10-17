namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System;

    public class PurchaseOrderDetailLogEntry
    {
        public int LogId { get; set; }

        public int LogUserNumber { get; set; }

        public string LogAction { get; set; }

        public DateTime LogTime { get; set; }

        public int OrderNumber { get; set; }

        public int Line { get; set; }

        public string DrawingReference { get; set; }

        public string SuppliersDesignation { get; set; }

        public decimal? OurQty { get; set; }

        public decimal? OrderQty { get; set; }

        public decimal? VatTotalCurrency { get; set; }

        public string OurUnitOfMeasure { get; set; }

        public string OrderUnitOfMeasure { get; set; }

        public decimal? OrderConversionFactor { get; set; }

        public string PartNumber { get; set; }

        public string PriceType { get; set; }

        public string QuotationRef { get; set; }

        public string Cancelled { get; set; }

        public string FilCancelled { get; set; }

        public string UpdatePartsupPrice { get; set; }

        public string IssuePartsToSupplier { get; set; }

        public string WasPreferredSupplier { get; set; }

        public string DeliveryInstructions { get; set; }

        public decimal NetTotalCurrency { get; set; }

        public decimal? DetailTotalCurrency { get; set; }

        public string StockPoolCode { get; set; }

        public int? OriginalOrderNumber { get; set; }

        public int? OriginalOrderLine { get; set; }
        
        public decimal? BaseOurUnitPrice { get; set; }

        public decimal? BaseOrderUnitPrice { get; set; }

        public decimal BaseNetTotal { get; set; }

        public decimal? BaseDetailTotal { get; set; }

        public decimal? BaseVatTotal { get; set; }

        public decimal? NextOurUnitPrice { get; set; }

        public decimal? NextOrderUnitPrice { get; set; }

        public string ManufacturerPartNumber { get; set; }

        public DateTime? DateFilCancelled { get; set; }

        public int? PeriodFilCancelled { get; set; }

        public decimal OverbookQtyAllowed { get; set; }

        public string RohsCompliant { get; set; }

        public string ShouldHaveBeenBlueReq { get; set; }

        public int? MpvAuthorisedBy { get; set; }

        public string MpvReason { get; set; }

        public int? PpvAuthorisedBy { get; set; }

        public string PpvReason { get; set; }

        public string MpvPpvComments { get; set; }

        public int? DeliveryConfirmedById { get; set; }

        public decimal? TotalQtyDelivered { get; set; }

        public string InternalComments { get; set; }
    }
}
