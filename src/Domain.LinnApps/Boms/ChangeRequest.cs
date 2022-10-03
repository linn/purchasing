namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ChangeRequest
    {
        public string DocumentType { get; set; }

        public int DocumentNumber { get; set; }

        public DateTime DateEntered { get; set; }

        public string ChangeState { get; set; }

        public string ReasonForChange { get; set; }

        public string DescriptionOfChange { get; set; }

        public ICollection<BomChange> BomChanges { get; set; }

        public ICollection<PcasChange> PcasChanges { get; set; }
    }
}
