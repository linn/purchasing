namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System;

    public class BoardRevisionType
    {
        public string TypeCode { get; set; }

        public string Description { get; set; }

        public string ReferenceRevision { get; set; }

        public string ShowLayoutCode { get; set; }

        public string RevisionCode { get; set; }

        public string ShowRevisionNumber { get; set; }

        public string DefaultLayoutType { get; set; }

        public DateTime? DateObsolete { get; set; }

        public string RevisionSuffix { get; set; }
    }
}
