namespace Linn.Purchasing.Resources
{
    public class SupplierContactResource
    {
        public ContactResource Contact { get; set; }

        public int SupplierId { get; set; }

        public string IsMainOrderContact { get; set; }

        public string IsMainInvoiceContact { get; set; }
    }
}
