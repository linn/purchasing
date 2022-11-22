namespace Linn.Purchasing.Resources
{
    using System.Collections.Generic;

    using Linn.Common.Resources;

    public class ChangeRequestResource : HypermediaResource
    {
        public string DocumentType { get; set; }

        public int DocumentNumber { get; set; }

        public string DateEntered { get; set; }

        public string DateAccepted { get; set; }

        public string ChangeState { get; set; }

        public string ReasonForChange { get; set; }

        public string DescriptionOfChange { get; set; }

        public IEnumerable<BomChangeResource> BomChanges { get; set; }

        public IEnumerable<PcasChangeResource> PcasChanges { get; set; }
    }
}
