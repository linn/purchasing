namespace Linn.Purchasing.Resources
{
    public class BomChangeResource
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
    }
}
