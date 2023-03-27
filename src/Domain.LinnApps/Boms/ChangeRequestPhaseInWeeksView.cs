namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System;

    public class ChangeRequestPhaseInWeeksView
    {
        public string PhaseInWeek { get; set; }

        public int? LinnWeekNumber { get; set; }

        public DateTime? LinnEndDate { get; set; }

        public int? DocumentNumber { get; set; }

        public DateTime? DateAccepted { get; set; }

        public string DisplayName { get; set; }

        public string OldNewPartNumber { get; set; }

        public string ReasonForChange { get; set; }

        public string Notes { get; set; }

        public string DescriptionOfChange { get; set; }

        public int? CountOfBomChanges { get; set; }

        public string PhaseInType { get; set; }

        public string OldPartNumber { get; set; }

        public string NewPartNumber { get; set; }

        public int? OldPartStock { get; set; }

        public int? NewPartStock { get; set; }
    }
}
