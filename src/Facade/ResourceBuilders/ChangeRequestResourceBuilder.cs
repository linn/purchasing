namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Resources;

    public class ChangeRequestResourceBuilder : IBuilder<ChangeRequest>
    {
        private readonly IBuilder<BomChange> bomChangeBuilder;

        private readonly IBuilder<PcasChange> pcasChangeBuilder;

        private readonly IAuthorisationService authService;

        public ChangeRequestResourceBuilder(IBuilder<BomChange> bomChangeBuilder, IBuilder<PcasChange> pcasChangeBuilder, IAuthorisationService authService)
        {
            this.bomChangeBuilder = bomChangeBuilder;
            this.pcasChangeBuilder = pcasChangeBuilder;
            this.authService = authService;
        }

        public ChangeRequestResource Build(ChangeRequest model, IEnumerable<string> claims)
        {
            return new ChangeRequestResource
                       {
                           DocumentType = model.DocumentType, 
                           DocumentNumber = model.DocumentNumber, 
                           ChangeState = model.ChangeState, 
                           ReasonForChange = model.ReasonForChange, 
                           DescriptionOfChange = model.DescriptionOfChange,
                           DateEntered = model.DateEntered.ToString("o"),
                           DateAccepted = model.DateAccepted?.ToString("o"),
                           ProposedBy = 
                               new EmployeeResource
                                   {
                                       Id = model.ProposedById,
                                       FullName = model.ProposedBy?.FullName
                                   },
                           EnteredBy =
                               new EmployeeResource { Id = model.EnteredById, FullName = model.EnteredBy?.FullName },
                           BomChanges =
                               model.BomChanges?.Select(
                                   d => (BomChangeResource)this.bomChangeBuilder.Build(d, claims)),
                           PcasChanges =
                               model.PcasChanges?.Select(
                                   d => (PcasChangeResource)this.pcasChangeBuilder.Build(d, claims)),
                           Links = claims != null ? this.BuildLinks(claims).ToArray() : null
            };
        }

        public string GetLocation(ChangeRequest model)
        {
            throw new NotImplementedException();
        }

        object IBuilder<ChangeRequest>.Build(ChangeRequest entity, IEnumerable<string> claims) => this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(IEnumerable<string> claims)
        {
            var privileges = claims as string[] ?? claims.ToArray();

            if (this.authService.HasPermissionFor(AuthorisedAction.ApproveChangeRequest, privileges))
            {
                yield return new LinkResource { Rel = "approve", Href = $"/purchasing/change-requests" };
            }
        }
    }
}
