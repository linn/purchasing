namespace Linn.Purchasing.Resources.MaterialRequirements
{
    using System;

    using Linn.Common.Resources;

    public class MrpRunLogResource : HypermediaResource
    {
        public int MrRunLogId { get; set; }

        public string JobRef { get; set; }

        public string BuildPlanName { get; set; }

        public DateTime RunDate { get; set; }

        public string RunDetails { get; set; }

        public string FullRun { get; set; }

        public string Kill { get; set; }

        public string Success { get; set; }

        public string LoadMessage { get; set; }

        public string MrMessage { get; set; }

        public DateTime? DateTidied { get; set; }
    }
}
