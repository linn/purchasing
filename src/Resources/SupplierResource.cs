namespace Linn.Purchasing.Resources
{
    using Linn.Common.Resources;

    public class SupplierResource : HypermediaResource
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? Planner { get; set; }

        public string CurrencyCode { get; set; }

        public string WebAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string OrderContactMethod { get; set; }

        public string InvoiceContactMethod { get; set; }

        public string SuppliersReference { get; set; }

        public string LiveOnOracle { get; set; }

        public int? InvoiceGoesToId { get; set; }

        public string InvoiceGoesToName { get; set; }

        public string ExpenseAccount { get; set; }

        public int PaymentDays { get; set; }

        public string PaymentMethod { get; set; }

        public string PaysInFc { get; set; }

        public string CurrencyName { get; set; }

        public string ApprovedCarrier { get; set; }

        public string AccountingCompany { get; set; }

        public string VatNumber { get; set; }

        public string PartCategory { get; set; }

        public string PartCategoryDescription { get; set; }

        public string OrderHold { get; set; }

        public string NotesForBuyer { get; set; }

        public string DeliveryDay { get; set; }

        public int? RefersToFcId { get; set; }

        public string RefersToFcName { get; set; }

        public int? PmDeliveryDaysGrace { get; set; }

        public int? OrderAddressId { get; set; }

        public string OrderFullAddress { get; set; }

        public int? InvoiceAddressId { get; set; }

        public string InvoiceFullAddress { get; set; }

        public string VendorManagerId { get; set; }

        public int? PlannerId { get; set; }

        public int? AccountControllerId { get; set; }

        public string AccountControllerName { get; set; }
    }
}
