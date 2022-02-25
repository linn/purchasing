namespace Linn.Purchasing.Resources
{
    public class SupplierContactResource
    {
        public int Id { get; set; }

        public int SupplierId { get; set; }

        public string IsMainOrderContact { get; set; }

        public string IsMainInvoiceContact { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string MobileNumber { get; set; }

        public string EmailAddress { get; set; }

        public string Comments { get; set; }

        public int? PersonId { get; set; }

        public string JobTitle { get; set; }
    }
}
