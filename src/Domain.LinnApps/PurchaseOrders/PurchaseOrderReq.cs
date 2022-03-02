namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System;

    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    public class PurchaseOrderReq
    {
        public int ReqNumber { get; set; }

        public string State { get; set; }

        public DateTime ReqDate { get; set; }

        public int? OrderNumber { get; set; }

        public string PartNumber { get; set; }

        public string PartDescription { get; set; }

        public decimal Qty { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal? Carriage { get; set; }

        public decimal? TotalReqPrice { get; set; }

        public Currency Currency { get; set; }

        public Supplier Supplier { get; set; }

        public string SupplierName { get; set; }

        public string SupplierContact { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

        public string AddressLine4 { get; set; }

        public string PostCode { get; set; }

        public Country Country { get; set; }

        public string PhoneNumber { get; set; }

        public string QuoteRef { get; set; }

        public string Email { get; set; }

        public DateTime? DateRequired { get; set; }

        public Employee RequestedBy { get; set; }

        public Employee AuthorisedBy { get; set; }

        public Employee SecondAuthBy { get; set; }

        public Employee FinanceCheckBy { get; set; }

        public Employee TurnedIntoOrderBy { get; set; }

        public string Nominal { get; set; }

        public string RemarksForOrder { get; set; }

        public string InternalNotes { get; set; }

        public string Department { get; set; }
    }
}
