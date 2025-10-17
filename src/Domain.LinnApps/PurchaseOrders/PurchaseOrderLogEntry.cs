namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System;

    public class PurchaseOrderLogEntry
    {
        public int LogId { get; set; }

        public int LogUserNumber { get; set; }

        public string LogAction { get; set; }

        public DateTime LogTime { get; set; }

        public int OrderNumber { get; set; }

        public string Cancelled { get; set; }

        public string FilCancelled { get; set; }

        public string DocumentTypeName { get; set; }

        public DateTime? OrderDate { get; set; }

        public int SupplierId { get; set; }

        public string Overbook { get; set; }

        public decimal? OverbookQty { get; set; }

        public string CurrencyCode { get; set; }

        public string BaseCurrencyCode { get; set; }

        public string OrderContactName { get; set; }

        public string OrderMethodName { get; set; }

        public decimal? ExchangeRate { get; set; }

        public string IssuePartsToSupplier { get; set; }

        public int DeliveryAddressId { get; set; }

        public int RequestedById { get; set; }

        public int EnteredById { get; set; }

        public string QuotationRef { get; set; }

        public int? AuthorisedById { get; set; }

        public string SentByMethod { get; set; }

        public string Remarks { get; set; }

        public DateTime? DateFilCancelled { get; set; }

        public int? PeriodFilCancelled { get; set; }

        public int OrderAddressId { get; set; }

        public int InvoiceAddressId { get; set; }

        public decimal? DamagesPercent { get; set; }

        public decimal? OrderNetTotal { get; set; }

        public decimal? BaseOrderNetTotal { get; set; }

        public decimal? OrderVatTotal { get; set; }

        public decimal? OrderTotal { get; set; }

        public decimal? BaseOrderVatTotal { get; set; }

        public decimal? BaseOrderTotal { get; set; }

        public string ArchiveOrder { get; set; }
    }
}
