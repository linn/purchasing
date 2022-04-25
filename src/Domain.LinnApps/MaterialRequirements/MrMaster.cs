namespace Linn.Purchasing.Domain.LinnApps.MaterialRequirements
{
    using System;

    public class MrMaster
    {
        public string JobRef { get; set; }

        public DateTime RunDate { get; set; }

        public int? RunLogIdCurrentlyInProgress { get; set; }
    }
}
