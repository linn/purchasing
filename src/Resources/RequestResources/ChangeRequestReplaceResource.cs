namespace Linn.Purchasing.Resources.RequestResources
{
    using System.Collections.Generic;

    public class ChangeRequestReplaceResource
    {
        public int DocumentNumber { get; set; }

        public bool GlobalReplace { get; set; }

        public bool HasPcasLines { get; set; }

        public decimal? NewQty { get; set; }

        public IEnumerable<int> SelectedDetailIds { get; set; }

        public IEnumerable<string> SelectedPcasComponents { get; set; }

        public IEnumerable<string> AddToBoms { get; set; }
    }
}
