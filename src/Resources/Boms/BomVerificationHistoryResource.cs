using Linn.Common.Resources;
using System;

namespace Linn.Purchasing.Resources.Boms
{
    public class BomVerificationHistoryResource : HypermediaResource
    {
        public int TRef { get; set; }

        public string PartNumber { get; set; }

        public string DateVerified { get; set; }

        public int VerifiedBy { get; set; }

        public string DocumentType { get; set; }

        public int? DocumentNumber { get; set; }

        public string Remarks { get; set; }
    }
}
