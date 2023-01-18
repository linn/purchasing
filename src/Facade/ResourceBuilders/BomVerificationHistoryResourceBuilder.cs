namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Resources.Boms;

    public class BomVerificationHistoryResourceBuilder : IBuilder<BomVerificationHistory>
    {
        private readonly IAuthorisationService authService;

        public BomVerificationHistoryResourceBuilder(IAuthorisationService authService)
        {
            this.authService = authService;
        }

        public BomVerificationHistoryResource Build(BomVerificationHistory model, IEnumerable<string> claims)
        {
            var resource = new BomVerificationHistoryResource
            {
                TRef = model.TRef,
                PartNumber = model.PartNumber,
                VerifiedBy = model.VerifiedBy,
                Remarks = model.Remarks,
                DocumentType = model.DocumentType,
                DocumentNumber = model.DocumentNumber,
                Links = claims != null ? this.BuildLinks(claims).ToArray() : null
            };

            return resource;
        }

        public string GetLocation(BomVerificationHistory model)
        {
            return $"/purchasing/bom-verification/{model.DocumentNumber}";
        }

        object IBuilder<BomVerificationHistory>.Build(BomVerificationHistory entity, IEnumerable<string> claims) => this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(IEnumerable<string> claims)
        {
            var privileges = claims as string[] ?? claims.ToArray();

            if (this.authService.HasPermissionFor(AuthorisedAction.ChangeBomType, privileges))
            {
                yield return new LinkResource { Rel = "verify-bom", Href = $"/purchasing/bom-verification" };
            }
        }
    }
}
