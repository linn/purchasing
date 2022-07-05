namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders.MiniOrders
{
    using System;
    using System.Collections.Generic;

    public class MiniOrder
    {
        public int OrderNumber { get; set; }

        public string DocumentType { get; set; }

        public DateTime DateOfOrder { get; set; }
        
        public DateTime? RequestedDeliveryDate { get; set; }

        public DateTime? AdvisedDeliveryDate { get; set; }

        public string Remarks { get; set; }

        public int SupplierId { get; set; }

        public string PartNumber { get; set; }

        public string Currency { get; set; }

        public string SuppliersDesignation { get; set; }

        public string VaxCurrency { get; set; }

        public decimal? VaxExchangeRate { get; set; }

        public decimal? VaxCurrencyUnitPrice { get; set; }

        public string Department { get; set; }

        public string Nominal { get; set; }

        public int? AuthorisedBy { get; set; }

        public int EnteredBy { get; set; }

        public string OurUnitOfMeasure { get; set; }

        public string OrderUnitOfMeasure { get; set; }

        public int RequestedBy { get; set; }

        public string DeliveryInstructions { get; set; }

        public decimal OurQty { get; set; }

        public decimal OrderQty { get; set; }

        public decimal? OrderConvFactor { get; set; }

        public decimal NetTotal { get; set; }

        public decimal VatTotal { get; set; }

        public decimal OrderTotal { get; set; }

        public string OrderMethod { get; set; }

        public int? CancelledBy { get; set; }

        public string ReasonCancelled { get; set; }

        public string SentByMethod { get; set; }

        public string AcknowledgeComment { get; set; }

        public int? DeliveryAddressId { get; set; }

        public int NumberOfSplitDeliveries { get; set; }

        public string QuotationRef { get; set; }

        public string IssuePartsToSupplier { get; set; }

        public string Vehicle { get; set; }

        public string Building { get; set; }

        public string Product { get; set; }

        public int? Person { get; set; }

        public string DrawingReference { get; set; }

        public string StockPoolCode { get; set; }

        public int? PrevOrderNumber { get; set; }

        public int? PrevOrderLine { get; set; }

        public int? FilCancelledBy { get; set; }

        public string ReasonFilCancelled { get; set; }

        public decimal? OurPrice { get; set; }

        public decimal? OrderPrice { get; set; }

        public string BaseCurrency { get; set; }

        public decimal? BaseOurPrice { get; set; }

        public decimal? BaseOrderPrice { get; set; }

        public decimal? BaseNetTotal { get; set; }

        public decimal? BaseVatTotal { get; set; }

        public decimal? BaseOrderTotal { get; set; }

        public decimal? ExchangeRate { get; set; }

        public string ManufacturerPartNumber { get; set; }

        public DateTime? DateFilCancelled { get; set; }

        public decimal? DutyPercent { get; set; }

        public string RohsCompliant { get; set; }

        public string ShouldHaveBeenBlueReq { get; set; }

        public string SpecialOrderType { get; set; }

        public int? PpvAuthorisedBy { get; set; }

        public string PpvReason { get; set; }

        public string MpvReason { get; set; }

        public int? MpvAuthorisedBy { get; set; }

        public string MpvPpvComments { get; set; }

        public int? DeliveryConfirmedBy { get; set; }

        public int? TotalQtyDelivered { get; set; }

        public string InternalComments { get; set; }

        public ICollection<MiniOrderDelivery> Deliveries { get; set; }

    }
}

