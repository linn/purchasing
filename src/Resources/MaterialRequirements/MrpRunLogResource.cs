namespace Linn.Purchasing.Resources.MaterialRequirements
{
    using Linn.Common.Resources;

    public class MrpRunLogResource : HypermediaResource
    {
        public int MrRunLogId { get; set; }

        public string JobRef { get; set; }

        public string BuildPlanName { get; set; }

        public string RunDate { get; set; }

        public string RunDetails { get; set; }

        public string FullRun { get; set; }

        public string Kill { get; set; }

        public string Success { get; set; }

        public string LoadMessage { get; set; }

        public string MrMessage { get; set; }

        public string DateTidied { get; set; }

        public int RunWeekNumber { get; set; }
    }
}
