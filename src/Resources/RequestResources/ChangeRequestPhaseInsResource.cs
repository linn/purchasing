namespace Linn.Purchasing.Resources.RequestResources
{
    using System;
    using System.Collections.Generic;

    public class ChangeRequestPhaseInsResource
    {
        public int DocumentNumber { get; set; }

        public DateTime? PhaseInWeekStart { get; set; }

        public int? PhaseInWeek { get; set; }

        public List<int> SelectedBomChangeIds { get; set; }
    }
}
