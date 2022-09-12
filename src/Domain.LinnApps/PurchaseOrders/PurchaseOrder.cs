namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System;
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    public class PurchaseOrder
    {
        public string Cancelled { get; set; }

        public ICollection<PurchaseOrderDetail> Details { get; set; }

        public string DocumentTypeName { get; set; }

        public DocumentType DocumentType { get; set; }

        public DateTime OrderDate { get; set; }

        public int OrderNumber { get; set; }

        public Supplier Supplier { get; set; }

        public int SupplierId { get; set; }

        public string Overbook { get; set; }

        public decimal? OverbookQty { get; set; }

        public Currency Currency { get; set; }

        public string CurrencyCode { get; set; }

        public string BaseCurrencyCode { get; set; }

        public string OrderContactName { get; set; }

        public string OrderMethodName { get; set; }

        public OrderMethod OrderMethod { get; set; }

        public decimal? ExchangeRate { get; set; }

        public string IssuePartsToSupplier { get; set; }

        public int DeliveryAddressId { get; set; }

        public LinnDeliveryAddress DeliveryAddress { get; set; }

        public Employee RequestedBy { get; set; }

        public int RequestedById { get; set; }

        public Employee EnteredBy { get; set; }

        public int EnteredById { get; set; }

        public string QuotationRef { get; set; }

        public Employee AuthorisedBy { get; set; }

        public int? AuthorisedById { get; set; }

        public string SentByMethod { get; set; }

        public string FilCancelled { get; set; }

        public string Remarks { get; set; }

        public DateTime? DateFilCancelled { get; set; }

        public int? PeriodFilCancelled { get; set; }

        public int OrderAddressId { get; set; }

        public Address OrderAddress { get; set; }

        public FullAddress InvoiceAddress { get; set; }

        public decimal? DamagesPercent { get; set; }

        public decimal OrderNetTotal { get; set; }

        public decimal BaseOrderNetTotal { get; set; }

        public decimal OrderVatTotal { get; set; }

        public int InvoiceAddressId { get; set; }

        public string ArchiveOrder { get; set; }

        public decimal OrderTotal { get; set; }

        public decimal BaseOrderVatTotal { get; set; }

        public decimal BaseOrderTotal { get; set; }
    }
}
