namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System;

    public class BomChange
    {
        public int ChangeId { get; set; }

        public string BomName { get; set; }

        public int BomId { get; set; }

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

        public int? PhaseInWeekNumber { get; set; }

        public LinnWeek PhaseInWeek { get; set; }

        public string PartNumber { get; set; }

        public string Comments { get; set; }

        public string PcasChange { get; set; }
    }
}
