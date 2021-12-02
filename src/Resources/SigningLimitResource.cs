namespace Linn.Purchasing.Resources
{
    using Linn.Common.Resources;

    public class SigningLimitResource : HypermediaResource
    {
        public int UserNumber { get; set; }

        public EmployeeResource User { get; set; }

        public decimal ProductionLimit { get; set; }

        public decimal SundryLimit { get; set; }

        public string Unlimited { get; set; }

        public string ReturnsAuthorisation { get; set; }
    }
}
