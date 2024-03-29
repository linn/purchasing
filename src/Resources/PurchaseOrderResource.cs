﻿namespace Linn.Purchasing.Resources
{
    using System.Collections.Generic;

    using Linn.Common.Resources;

    public class PurchaseOrderResource : HypermediaResource
    {
        public int OrderNumber { get; set; }

        public CurrencyResource Currency { get; set; }

        public string OrderDate { get; set; }

        public OrderMethodResource OrderMethod { get; set; }

        public string Cancelled { get; set; }

        public string Overbook { get; set; }

        public DocumentTypeResource DocumentType { get; set; }

        public SupplierResource Supplier { get; set; }

        public decimal? OverbookQty { get; set; }

        public IEnumerable<PurchaseOrderDetailResource> Details { get; set; }

        public string OrderContactName { get; set; }

        public decimal? ExchangeRate { get; set; }

        public string IssuePartsToSupplier { get; set; }

        public LinnDeliveryAddressResource DeliveryAddress { get; set; }

        public EmployeeResource RequestedBy { get; set; }

        public EmployeeResource EnteredBy { get; set; }

        public string QuotationRef { get; set; }

        public EmployeeResource AuthorisedBy { get; set; }

        public string SentByMethod { get; set; }

        public string FilCancelled { get; set; }

        public string Remarks { get; set; }

        public string DateFilCancelled { get; set; }

        public int? PeriodFilCancelled { get; set; }

        public AddressResource OrderAddress { get; set; }

        public int InvoiceAddressId { get; set; }

        public string SupplierContactEmail { get; set; }

        public string SupplierContactPhone { get; set; }

        public decimal OrderNetTotal { get; set; }

        public decimal BaseOrderNetTotal { get; set; }

        public string ReasonCancelled { get; set; }

        public string CancelledByName { get; set; }

        public string DateCancelled { get; set; }

        public IEnumerable<PurchaseLedgerResource> LedgerEntries { get; set; }

        public string NotesForBuyer { get; set; }
    }
}
