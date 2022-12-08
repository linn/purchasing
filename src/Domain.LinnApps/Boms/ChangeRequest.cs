﻿namespace Linn.Purchasing.Domain.LinnApps.Boms
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

        public string GlobalReplace { get; set; }

        // RequiresStartingSernos and RequiresVerification only included for historic Change Requests
        public string RequiresStartingSernos { get; set; }

        public string RequiresVerification { get; set; }

        public ICollection<BomChange> BomChanges { get; set; }

        public ICollection<PcasChange> PcasChanges { get; set; }

        public bool CanApprove()
        {
            return this.ChangeState == "PROPOS";
        }

        public bool CanCancel(bool adminPrivs)
        {
            if (this.ChangeState == "PROPOS")
            {
                return true;
            }

            if (this.ChangeState == "ACCEPT")
            {
                return adminPrivs;
            }

            return false;
        }

        public void Approve()
        {
            if (this.CanApprove())
            {
                this.ChangeState = "ACCEPT";
                this.DateAccepted = DateTime.Now;
            }
        }

        public void CancelAll(Employee cancelledBy)
        {
            if (this.CanCancel(true))
            {
                this.ChangeState = "CANCEL";
                if (this.BomChanges != null)
                {
                    foreach (var bomChange in this.BomChanges)
                    {
                        if (bomChange.CanCancel())
                        {
                            bomChange.Cancel(cancelledBy);
                        }
                    }
                }

                if (this.PcasChanges != null)
                {
                    foreach (var pcasChange in this.PcasChanges)
                    {
                        if (pcasChange.CanCancel())
                        {
                            pcasChange.Cancel(cancelledBy);
                        }
                    }
                }
            }
        }
    }
}
