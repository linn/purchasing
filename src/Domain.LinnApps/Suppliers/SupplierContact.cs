namespace Linn.Purchasing.Domain.LinnApps.Suppliers
{
    public class SupplierContact
    {
        public int ContactId { get; set; }

        public Person Person { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public int SupplierId { get; set; }

        public string MainOrderContact { get; set; }
    }
}
