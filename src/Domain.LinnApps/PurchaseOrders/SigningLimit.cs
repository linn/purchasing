namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    public class SigningLimit
    {
        public Employee User { get; set; }

        public int UserNumber { get; set; }

        public decimal ProductionLimit { get; set; }

        public decimal SundryLimit { get; set; }

        public string Unlimited { get; set; }

        public string ReturnsAuthorisation { get; set; }
    }
}
