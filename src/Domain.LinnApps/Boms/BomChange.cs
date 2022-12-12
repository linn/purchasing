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

        public int EnteredById { get; set; }

        public Employee EnteredBy { get; set; }

        public DateTime? DateApplied { get; set; }

        public int? AppliedById { get; set; }

        public Employee AppliedBy { get; set; }

        public DateTime? DateCancelled { get; set; }

        public int? CancelledById { get; set; }

        public Employee CancelledBy { get; set; }

        public string ChangeState { get; set; }

        public int? PhaseInWeekNumber { get; set; }

        public LinnWeek PhaseInWeek { get; set; }

        public string PartNumber { get; set; }

        public string Comments { get; set; }

        public string PcasChange { get; set; }
        
        public bool CanCancel() => this.ChangeState == "PROPOS" || this.ChangeState == "ACCEPT";

        public void Cancel(Employee cancelledBy)
        {
            if (this.CanCancel() && (cancelledBy != null))
            {
                this.ChangeState = "CANCEL";
                this.DateCancelled = DateTime.Now;
                this.CancelledById = cancelledBy.Id;
                this.CancelledBy = cancelledBy;
            }
        }

        public string LifecycleText()
        {
            if (this.ChangeState == "CANCEL")
            {
                return $"Cancelled on {this.DateCancelled?.ToString("dd-MMM-yy")} by {this.CancelledBy?.FullName}";
            }
            if (this.ChangeState == "LIVE")
            {
                return $"Live on {this.DateApplied?.ToString("dd-MMM-yy")} by {this.AppliedBy?.FullName}";
            }
            return $"Created on {this.DateEntered.ToString("dd-MMM-yy")} by {this.EnteredBy?.FullName}";
        }
    }
}
