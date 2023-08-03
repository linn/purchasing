namespace Linn.Purchasing.Domain.LinnApps.Boms.Models
{
    using System.Collections.Generic;

    public class BomCostReports
    {
        public IEnumerable<BomCostReport> BomCosts { get; set; }
    }
}
