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

        public ChangeRequestResourceBuilder(
            IBuilder<BomChange> bomChangeBuilder,
            IBuilder<PcasChange> pcasChangeBuilder,
            IAuthorisationService authService)
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
                           NewPartNumber = model.NewPartNumber,
                           BoardCode = model.BoardCode,
                           RevisionCode = model.RevisionCode,
                           ChangeType = model.ChangeRequestType,
                           OldPartNumber = model.OldPartNumber,
                           OldPartDescription = model.OldPart?.Description,
                           NewPartDescription = model.NewPart?.Description,
                           BoardDescription = model.CircuitBoard?.Description,  
                           GlobalReplace = (model.GlobalReplace == "Y"),
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
                           Links = this.BuildLinks(model, claims).ToArray()
            };
        }

        public string GetLocation(ChangeRequest model)
        {
            return $"/purchasing/change-requests/{model.DocumentNumber}";
        }

        object IBuilder<ChangeRequest>.Build(ChangeRequest entity, IEnumerable<string> claims) => this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(ChangeRequest model, IEnumerable<string> claims)
        {
            var privileges = claims == null ? Array.Empty<string>() : claims as string[] ?? claims.ToArray();
            var adminPrivs = this.authService.HasPermissionFor(AuthorisedAction.AdminChangeRequest, privileges);

            if (this.authService.HasPermissionFor(AuthorisedAction.ApproveChangeRequest, privileges) && model.CanApprove())
            {
                yield return new LinkResource { Rel = "approve", Href = $"/purchasing/change-requests/status" };
            }

            if (model.CanCancel(adminPrivs))
            {
                yield return new LinkResource { Rel = "cancel", Href = $"/purchasing/change-requests/status" };
            }

            if (model.CanMakeLive() && this.authService.HasPermissionFor(AuthorisedAction.MakeLiveChangeRequest, privileges))
            {
                yield return new LinkResource { Rel = "make-live", Href = $"/purchasing/change-requests/status" };
            }

            if (model.CanPhaseIn() && this.authService.HasPermissionFor(AuthorisedAction.AdminChangeRequest, privileges))
            {
                yield return new LinkResource { Rel = "phase-in", Href = $"/purchasing/change-requests/phase-ins" };
            }

            if (model.CanUndo() && this.authService.HasPermissionFor(AuthorisedAction.AdminChangeRequest, privileges))
            {
                yield return new LinkResource { Rel = "undo", Href = $"/purchasing/change-requests/phase-ins" };
            }

            if (model.CanReplace(adminPrivs))
            {
                yield return new LinkResource { Rel = "replace", Href = $"/purchasing/change-requests/replace" };
            }

            yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
        }
    }
}
