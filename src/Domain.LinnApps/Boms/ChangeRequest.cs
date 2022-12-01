namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System;
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.Parts;

    public class ChangeRequest
    {
        public string DocumentType { get; set; }

        public int DocumentNumber { get; set; }

        public DateTime DateEntered { get; set; }

        public DateTime? DateAccepted { get; set; }

        public string ChangeState { get; set; }

        public Employee ProposedBy { get; set; }

        public int ProposedById { get; set; }

        public Employee EnteredBy { get; set; }

        public int EnteredById { get; set; }

        public string ChangeRequestType { get; set; }

        public string OldPartNumber { get; set; }

        public Part OldPart { get; set; }

        public string NewPartNumber { get; set; }

        public Part NewPart { get; set; }

        public string ReasonForChange { get; set; }

        public string DescriptionOfChange { get; set; }

        public ICollection<BomChange> BomChanges { get; set; }

        public ICollection<PcasChange> PcasChanges { get; set; }

        public bool CanApprove()
        {
            return this.ChangeState == "PROPOS";
        }

        public void Approve()
        {
            if (this.CanApprove())
            {
                this.ChangeState = "ACCEPT";
                this.DateAccepted = DateTime.Now;
            }
        }
    }
}
