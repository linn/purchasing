namespace Linn.Purchasing.Resources
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Resources;

    public class PurchaseOrderResource : HypermediaResource
    {
        public int OrderNumber { get; set; }

        public string CurrCode { get; set; }
        //todo delete and just use Currency

        public CurrencyResource Currency { get; set; }

        public DateTime DateOfOrder { get; set; }

        public string OrderMethod { get; set; }
        //todo keep below remove string
        //public OrderMethodResource OrderMethod { get; set; }

        public string Cancelled { get; set; }

        public string Overbook { get; set; }

        public string DocumentType { get; set; }
        //remove above keep below
        //public DocumentTypeResource DocumentType { get; set; }
        
        public int SupplierId { get; set; }
        //todo remove id and just use object

        public SupplierResource Supplier { get; set; }
        //todo .include phone number and email address for supplier contact for orders

        public decimal? OverbookQty { get; set; }

        public IList<PurchaseOrderDetailResource> Details { get; set; }

        public DateTime OrderDate { get; set; }

        //todo make sure this is added in domain from supplier contact when creating
        //or could show as field on front end and pass back
        public string OrderContactName { get; set; }

        public string OrderMethodName { get; set; }


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

        public DateTime? DateFilCancelled { get; set; }

        public int? PeriodFilCancelled { get; set; }

        public bool CurrentlyUsingOverbookForm { get; set; }
    }
}
