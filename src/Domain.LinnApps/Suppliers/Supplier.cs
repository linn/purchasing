namespace Linn.Purchasing.Domain.LinnApps.Suppliers
{
    using System;
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    public class Supplier
    {
        public int SupplierId { get; set; }

        public string Name { get; set; }

        public string WebAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string OrderContactMethod { get; set; }

        public string InvoiceContactMethod { get; set; }

        public string SuppliersReference { get; set; }

        public string LiveOnOracle { get; set; }

        public Supplier InvoiceGoesTo { get; set; }

        public string ExpenseAccount { get; set; }

        public int PaymentDays { get; set; }

        public string PaymentMethod { get; set; }

        public string PaysInFc { get; set; }

        public Currency Currency { get; set; }

        public string ApprovedCarrier { get; set; }

        public string AccountingCompany { get; set; }

        public string VatNumber { get; set; }

        public PartCategory PartCategory { get; set; }

        public string OrderHold { get; set; }

        public string NotesForBuyer { get; set; }

        public string DeliveryDay { get; set; }

        public Supplier RefersToFc { get; set; }

        public int? PmDeliveryDaysGrace { get; set; }

        public Address OrderAddress { get; set; }

        public FullAddress InvoiceFullAddress { get; set; }

        public Planner Planner { get; set; }

        public string VendorManagerId { get; set; }

        public VendorManager VendorManager { get; set; }

        public Employee AccountController { get; set; }

        public DateTime DateOpened { get; set; }

        public Employee OpenedBy { get; set; }

        public DateTime? DateClosed { get; set; }

        public Employee ClosedBy { get; set; }

        public string ReasonClosed { get; set; }

        public string Notes { get; set; }

        public int OrganisationId { get; set; }

        public IEnumerable<SupplierContact> SupplierContacts { get; set; }

        public string Country { get; set; }

        public SupplierGroup Group { get; set; }

        public string ReceivesOrderReminders { get; set; }

        public string PrintTerms { get; set; }
    }
}
