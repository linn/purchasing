namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System;

    public class PcasChange
    {
        public int ChangeId { get; set; }

        public string BoardCode { get; set; }

        public string RevisionCode { get; set; }

        public ChangeRequest ChangeRequest { get; set; }

        public string DocumentType { get; set; }

        public int DocumentNumber { get; set; }

        public DateTime DateEntered { get; set; }

        public int EnteredBy { get; set; }

        public DateTime? DateApplied { get; set; }

        public int? AppliedBy { get; set; }

        public DateTime? DateCancelled { get; set; }

        public int? CancelledBy { get; set; }

        public string ChangeState { get; set; }

        public string Comments { get; set; }
    }
}
