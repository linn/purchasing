﻿namespace Linn.Purchasing.Domain.LinnApps.Mailers
{
    public interface ISupplierAutoEmailsMailer
    {
        void SendOrderBookEmail(
            string toAddress, 
            int toSupplier, 
            string timestamp, 
            bool test = false);

        void SendMonthlyForecastEmail(
            string toAddress,
            int toSupplier,
            string timestamp,
            bool test = false);
    }
}
