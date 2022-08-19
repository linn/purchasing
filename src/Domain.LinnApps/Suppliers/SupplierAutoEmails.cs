namespace Linn.Purchasing.Domain.LinnApps.Suppliers
{
    public class SupplierAutoEmails
    {
        public int SupplierId { get; set; }

        public string EmailAddress { get; set; }

        public string OrderBook { get; set; }

        public string Forecast { get; set; }

        public string ForecastInterval { get; set; }
    }
}
