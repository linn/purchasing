namespace Linn.Purchasing.Resources
{
    using System.Runtime.InteropServices;

    using Linn.Common.Resources;

    public class BomChangeResource : HypermediaResource
    {
        public int ChangeId { get; set; }

        public string BomName { get; set; }

        public int BomId { get; set; }

        public string DocumentType { get; set; }

        public int DocumentNumber { get; set; }

        public string DateEntered { get; set; }

        public int EnteredBy { get; set; }

        public string DateApplied { get; set; }

        public int? AppliedBy { get; set; }

        public string DateCancelled { get; set; }

        public int? CancelledBy { get; set; }

        public string ChangeState { get; set; }

        public string PartNumber { get; set; }

        public string Comments { get; set; }

        public int? PhaseInWeekNumber { get; set; }

        public string PhaseInWWYYYY { get; set; }

        public string LifecycleText { get; set; }
    }
}
