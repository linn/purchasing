namespace Linn.Purchasing.Resources
{
    using Linn.Common.Resources;

    public class PurchaseOrderReqResource : HypermediaResource
    {
        public int ReqNumber { get; set; }

        public string State { get; set; }

        public string ReqDate { get; set; }

        public int? OrderNumber { get; set; }

        public string PartNumber { get; set; }

        public string PartDescription { get; set; }

        public decimal Qty { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal? Carriage { get; set; }

        public decimal? TotalReqPrice { get; set; }

        public CurrencyResource Currency { get; set; }

        public SupplierResource Supplier { get; set; }

        public string SupplierContact { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

        public string AddressLine4 { get; set; }

        public string PostCode { get; set; }

        public CountryResource Country { get; set; }

        public string PhoneNumber { get; set; }

        public string QuoteRef { get; set; }

        public string Email { get; set; }

        public string DateRequired { get; set; }

        public EmployeeResource RequestedBy { get; set; }

        public EmployeeResource AuthorisedBy { get; set; }

        public EmployeeResource SecondAuthBy { get; set; }

        public EmployeeResource FinanceCheckBy { get; set; }

        public EmployeeResource TurnedIntoOrderBy { get; set; }

        public NominalResource Nominal { get; set; }

        public string RemarksForOrder { get; set; }

        public string InternalNotes { get; set; }

        public DepartmentResource Department { get; set; }
    }
}
