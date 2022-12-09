namespace Linn.Purchasing.Resources
{
    public class PcasChangeResource
    {
        public int ChangeId { get; set; }

        public string BoardCode { get; set; }

        public string RevisionCode { get; set; }

        public string DocumentType { get; set; }

        public int DocumentNumber { get; set; }

        public string DateEntered { get; set; }

        public int EnteredBy { get; set; }

        public string DateApplied { get; set; }

        public int? AppliedBy { get; set; }

        public string DateCancelled { get; set; }

        public int? CancelledBy { get; set; }

        public string ChangeState { get; set; }

        public string Comments { get; set; }

        public string LifecycleText { get; set; }
    }
}
