namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using Linn.Common.Domain;

    public class SigningLimitLog : LogTable
    {
        public int UserNumber { get; set; }

        public decimal ProductionLimit { get; set; }

        public decimal SundryLimit { get; set; }

        public string Unlimited { get; set; }

        public string ReturnsAuthorisation { get; set; }
    }
}
