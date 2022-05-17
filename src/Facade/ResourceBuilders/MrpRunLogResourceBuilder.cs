namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Resources.MaterialRequirements;

    public class MrpRunLogResourceBuilder : IBuilder<MrpRunLog>
    {
        public MrpRunLogResource Build(MrpRunLog entity, IEnumerable<string> claims)
        {
            return new MrpRunLogResource
            {
                MrRunLogId = entity.MrRunLogId,
                JobRef = entity.JobRef,
                LoadMessage = entity.LoadMessage,
                BuildPlanName = entity.BuildPlanName,
                DateTidied = entity.DateTidied,
                FullRun = entity.FullRun,
                Kill = entity.Kill,
                Links = this.BuildLinks(entity, claims).ToArray(),
                MrMessage = entity.MrMessage,
                RunDate = entity.RunDate,
                RunDetails = entity.RunDetails,
                Success = entity.Success,
                RunWeekNumber = entity.RunWeekNumber
            };
        }

        public string GetLocation(MrpRunLog mrpRunLog)
        {
            return $"/purchasing/material-requirements/run-logs/{mrpRunLog.MrRunLogId}";
        }

        object IBuilder<MrpRunLog>.Build(MrpRunLog entity, IEnumerable<string> claims)
        {
            return this.Build(entity, claims);
        }

        private IEnumerable<LinkResource> BuildLinks(MrpRunLog model, IEnumerable<string> claims)
        {
            yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
        }
    }
}
