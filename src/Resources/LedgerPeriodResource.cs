namespace Linn.Purchasing.Resources
{
    using Linn.Common.Resources;

    public class LedgerPeriodResource : HypermediaResource
    {
        public int PeriodNumber { get; set; }

        public string MonthName { get; set; }
    }
}
