namespace Linn.Purchasing.Domain.LinnApps.MaterialRequirements
{
    using System.Collections.Generic;

    public class MrReport
    {
        public string JobRef { get; set; }

        public int RunWeekNumber { get; set; }

        public IEnumerable<MrHeader> Headers { get; set; }
    }
}
