namespace Linn.Purchasing.Domain.LinnApps.Mailers
{
    public interface ISupplierAutoEmailsMailer
    {
        void SendOrderBookEmail(
            string toAddresses, 
            int toSupplier, 
            string timestamp, 
            bool test = false);

        void SendMonthlyForecastEmail(
            string toAddresses,
            int toSupplier,
            string timestamp,
            bool test = false);
    }
}
