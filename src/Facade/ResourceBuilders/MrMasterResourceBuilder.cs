namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Resources.MaterialRequirements;

    public class MrMasterResourceBuilder : IBuilder<MrMaster>
    {
        private readonly IAuthorisationService authService;

        public MrMasterResourceBuilder(IAuthorisationService authService)
        {
            this.authService = authService;
        }

        public MrMasterResource Build(MrMaster entity, IEnumerable<string> claims)
        {
            return new MrMasterResource
            {
                JobRef = entity.JobRef,
                RunDate = entity.RunDate.ToString("o"),
                RunLogIdCurrentlyInProgress = entity.RunLogIdCurrentlyInProgress,
                Links = this.BuildLinks(entity, claims).ToArray()
            };
        }

        public string GetLocation(MrMaster master)
        {
            return "/purchasing/material-requirements/last-run";
        }

        object IBuilder<MrMaster>.Build(MrMaster entity, IEnumerable<string> claims)
        {
            return this.Build(entity, claims);
        }

        private IEnumerable<LinkResource> BuildLinks(MrMaster model, IEnumerable<string> claims)
        {
            yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
            yield return new LinkResource { Rel = "last-run-log", Href = $"/purchasing/material-requirements/run-logs?jobRef={model.JobRef}" };
            
            if (!model.RunLogIdCurrentlyInProgress.HasValue && this.authService.HasPermissionFor(AuthorisedAction.MrpRun, claims))
            {
                yield return new LinkResource { Rel = "run-mrp", Href = "/purchasing/material-requirements/run-mrp" };
            }
        }
    }
}
