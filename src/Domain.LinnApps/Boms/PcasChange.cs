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

        public int EnteredById { get; set; }

        public Employee EnteredBy { get; set; }

        public DateTime? DateApplied { get; set; }

        public int? AppliedById { get; set; }

        public Employee AppliedBy { get; set; }

        public DateTime? DateCancelled { get; set; }

        public int? CancelledById { get; set; }

        public Employee CancelledBy { get; set; }

        public string ChangeState { get; set; }

        public string Comments { get; set; }

        public bool CanCancel() => this.ChangeState == "PROPOS" || this.ChangeState == "ACCEPT";

        public bool CanMakeLive() => this.ChangeState == "ACCEPT";

        public void Cancel(Employee cancelledBy)
        {
            if (this.CanCancel() && cancelledBy != null)
            {
                this.ChangeState = "CANCEL";
                this.DateCancelled = DateTime.Now;
                this.CancelledById = cancelledBy.Id;
                this.CancelledBy = cancelledBy;
            }
        }

        public void MakeLive(Employee appliedBy)
        {
            if (this.CanMakeLive() && appliedBy != null)
            {
                this.ChangeState = "LIVE";
                this.DateApplied = DateTime.Now;
                this.AppliedById = appliedBy.Id;
                this.AppliedBy = appliedBy;
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

            return $"Created on {this.DateEntered:dd-MMM-yy} by {this.EnteredBy?.FullName}";
        }
    }
}
