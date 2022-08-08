namespace Linn.Purchasing.Domain.LinnApps.Mailers
{
    public interface IMrOrderBookMailer
    {
        void SendOrderBookEmail(
            string toAddress, 
            int toSupplier, 
            string timestamp, 
            bool test = false);
    }
}
