namespace Linn.Purchasing.Resources
{
    using System.Collections.Generic;

    using Linn.Common.Resources;

    public class SupplierResource : HypermediaResource
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string CurrencyCode { get; set; }

        public string WebAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string OrderContactMethod { get; set; }

        public string InvoiceContactMethod { get; set; }

        public string SuppliersReference { get; set; }

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

        public int? OpenedById { get; set; }

        public string OpenedByName { get; set; }

        public int? ClosedById { get; set; }

        public string ClosedByName { get; set; }

        public string DateOpened { get; set; }

        public string DateClosed { get; set; }

        public string ReasonClosed { get; set; }

        public string Notes { get; set; }

        public int OrganisationId { get; set; }

        public IEnumerable<SupplierContactResource> SupplierContacts { get; set; }

        public int? GroupId { get; set; }

        public string Country { get; set; }

        public AddressResource OrderAddress { get; set; }

        public bool ReceivesPurchaseOrderReminders { get; set; }

        public bool PrintTerms { get; set; }
    }
}
