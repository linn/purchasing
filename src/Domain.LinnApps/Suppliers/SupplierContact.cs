namespace Linn.Purchasing.Domain.LinnApps.Suppliers
{
    public class SupplierContact
    {
        public int SupplierId { get; set; }

        public int ContactId { get; set; }

        public string IsMainOrderContact { get; set; }

        public string IsMainInvoiceContact { get; set; }

        public string PhoneNumber { get; set; }

        public string MobileNumber { get; set; }

        public string EmailAddress { get; set; }

        public string JobTitle { get; set; }

        public string Comments { get; set; }

        public Person Person { get; set; }
    }
}
