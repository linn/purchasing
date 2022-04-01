namespace Linn.Purchasing.Domain.LinnApps.MaterialRequirements
{
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;

    public class MaterialRequirementsPlanningService : IMaterialRequirementsPlanningService
    {
        private readonly IMrpLoadPack mrpLoadPack;

        public MaterialRequirementsPlanningService(IMrpLoadPack mrpLoadPack)
        {
            this.mrpLoadPack = mrpLoadPack;
        }

        public ProcessResult RunMrp()
        {
            var runLogId = this.mrpLoadPack.GetNextRunLogId();
            var result = this.mrpLoadPack.ScheduleMrp(runLogId);
            if (result.Success)
            {
                result.ProcessHref = $"/purchasing/material-requirements/run-logs/{runLogId}";
            }
            
            return result;
        }
    }
}
