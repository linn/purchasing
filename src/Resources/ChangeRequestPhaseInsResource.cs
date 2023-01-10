namespace Linn.Purchasing.Resources
{
    using System.Collections.Generic;

    public class ChangeRequestPhaseInsResource
    {
        public int DocumentNumber { get; set; }

        public int PhaseInWeek { get; set; }

        public List<int> SelectedBomChangeIds { get; set; }
    }
}
