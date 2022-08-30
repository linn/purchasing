namespace Linn.Purchasing.Domain.LinnApps.Reports.Models
{
    public class MonthlyForecastPartRequirement
    {
        public string PartNumber { get; set; }

        public int MonthEndWeek { get; set; }

        public decimal NettRequirement { get; set; }

        public string NettRequirementK { get; set; }
    } 
}
