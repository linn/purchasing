namespace Linn.Purchasing.Domain.LinnApps.Suppliers
{
    public class SupplierContact
    {
        public int SupplierId { get; set; }

        public int ContactId { get; set; }

        public string IsMainOrderContact { get; set; }

        public string IsMainInvoiceContact { get; set; }

        public int PersonId { get; set; }

        public Contact Contact { get; set; }
    }
}
