namespace Linn.Purchasing.Resources.RequestResources
{
    using System.Collections.Generic;

    public class ChangeRequestStatusChangeResource // i know it's RequestRequest
    {
        public int Id { get; set; }

        public string Status { get; set; }

        public List<int> SelectedBomChangeIds { get; set; }

        public List<int> SelectedPcasChangeIds { get; set; }
    }
}
