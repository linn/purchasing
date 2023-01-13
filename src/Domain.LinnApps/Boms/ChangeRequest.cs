namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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

        public string BoardCode { get; set; }

        public string RevisionCode { get; set; }

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

        public bool CanEdit(bool adminPrivs)
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

        public void Cancel(Employee cancelledBy, IEnumerable<int> selectedBomChangeIds, IEnumerable<int> selectedPcasChangeIds)
        {
            if (this.CanCancel(true))
            {
                var cancelledAll = true;
                var globalCancel = !(selectedBomChangeIds?.Any() ?? false) && !(selectedPcasChangeIds?.Any() ?? false);

                if (this.BomChanges != null)
                {
                    foreach (var bomChange in this.BomChanges)
                    {
                        if (bomChange.CanCancel())
                        {
                            if (selectedBomChangeIds == null && !globalCancel)
                            {
                                cancelledAll = false;
                            }
                            else if (globalCancel || selectedBomChangeIds.Contains(bomChange.ChangeId))
                            {
                                bomChange.Cancel(cancelledBy);
                            }
                            else
                            {
                                cancelledAll = false;
                            }
                        }
                    }
                }

                if (this.PcasChanges != null)
                {
                    foreach (var pcasChange in this.PcasChanges)
                    {
                        if (pcasChange.CanCancel())
                        {
                            if (selectedPcasChangeIds == null && !globalCancel)
                            {
                                cancelledAll = false;
                            }
                            else if (globalCancel || selectedPcasChangeIds.Contains(pcasChange.ChangeId))
                            {
                                pcasChange.Cancel(cancelledBy);
                            }
                            else
                            {
                                cancelledAll = false;
                            }
                        }
                    }
                }

                if (cancelledAll)
                {
                    this.ChangeState = "CANCEL";
                }
            }
        }

        public bool CanMakeLive() => this.ChangeState == "ACCEPT";

        public void MakeLive(Employee appliedBy, IEnumerable<int> selectedBomChangeIds, IEnumerable<int> selectedPcasChangeIds)
        {
            if (this.CanMakeLive())
            {
                var allLive = true;
                var globalLive = !(selectedBomChangeIds?.Any() ?? false) && !(selectedPcasChangeIds?.Any() ?? false);

                if (this.BomChanges != null)
                {
                    foreach (var bomChange in this.BomChanges)
                    {
                        if (bomChange.CanMakeLive())
                        {
                            if (selectedBomChangeIds == null && !allLive)
                            {
                                allLive = false;
                            }
                            else if (globalLive || selectedBomChangeIds.Contains(bomChange.ChangeId))
                            {
                                bomChange.MakeLive(appliedBy);
                            }
                            else
                            {
                                allLive = false;
                            }
                        }
                    }
                }

                if (this.PcasChanges != null)
                {
                    foreach (var pcasChange in this.PcasChanges)
                    {
                        if (pcasChange.CanCancel())
                        {
                            if (selectedPcasChangeIds == null && !globalLive)
                            {
                                allLive = false;
                            }
                            else if (globalLive || selectedPcasChangeIds.Contains(pcasChange.ChangeId))
                            {
                                pcasChange.MakeLive(appliedBy);
                            }
                            else
                            {
                                allLive = false;
                            }
                        }
                    }
                }

                if (allLive)
                {
                    this.ChangeState = "LIVE";
                }
            }
        }

        public bool CanPhaseIn() => this.ChangeState == "ACCEPT";

        public void PhaseIn(LinnWeek week, IEnumerable<int> selectedBomChangeIds)
        {
            if (this.CanPhaseIn() && selectedBomChangeIds != null && this.BomChanges != null)
            {
                foreach (var bomChange in this.BomChanges)
                {
                    if (bomChange.CanPhaseIn())
                    {
                        if (selectedBomChangeIds.Contains(bomChange.ChangeId))
                        {
                            bomChange.PhaseIn(week);
                        }
                    }
                }
            }
        }
    }
}
