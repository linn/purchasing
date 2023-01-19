namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Resources.Boms;

    public class BomVerificationHistoryResourceBuilder : IBuilder<BomVerificationHistory>
    {
        public BomVerificationHistoryResource Build(BomVerificationHistory model, IEnumerable<string> claims)
        {
            var resource = new BomVerificationHistoryResource
            {
                TRef = model.TRef,
                PartNumber = model.PartNumber,
                VerifiedBy = model.VerifiedBy,
                DateVerified= model.DateVerified?.ToString("dd-MMM-yyyy"),
                Remarks = model.Remarks,
                DocumentType = model.DocumentType,
                DocumentNumber = model.DocumentNumber,
                Links = this.BuildLinks(model, claims).ToArray()
            };

            return resource;
        }

        public string GetLocation(BomVerificationHistory model)
        {
            return $"/purchasing/bom-verification/{model.DocumentNumber}";
        }

        object IBuilder<BomVerificationHistory>.Build(BomVerificationHistory entity, IEnumerable<string> claims) => this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(BomVerificationHistory model, IEnumerable<string> claims)
        {
            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
            }
        }
    }
}
