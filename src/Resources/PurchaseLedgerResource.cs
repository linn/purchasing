namespace Linn.Purchasing.Resources
{
    public class PurchaseLedgerResource
    {
        public string TransType { get; set; }

        public string PlDeliveryRef { get; set; }

        public decimal? Qty { get; set; }

        public decimal? NetTotal { get; set; }

        public decimal? VatTotal { get; set; }

        public string InvoiceRef { get; set; }

        public decimal? BaseVat { get; set; }

        public string InvoiceDate { get; set; }

        public int Tref { get; set; }
    }
}
