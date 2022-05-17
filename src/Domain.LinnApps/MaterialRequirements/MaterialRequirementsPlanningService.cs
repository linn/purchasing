namespace Linn.Purchasing.Domain.LinnApps.MaterialRequirements
{
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;

    public class MaterialRequirementsPlanningService : IMaterialRequirementsPlanningService
    {
        private readonly IMrpLoadPack mrpLoadPack;

        private readonly ISingleRecordRepository<MrMaster> masterRepository;

        public MaterialRequirementsPlanningService(IMrpLoadPack mrpLoadPack, ISingleRecordRepository<MrMaster> masterRepository)
        {
            this.mrpLoadPack = mrpLoadPack;
            this.masterRepository = masterRepository;
        }

        public ProcessResult RunMrp()
        {
            var master = this.masterRepository.GetRecord();
            if (master.RunLogIdCurrentlyInProgress.HasValue)
            {
                return new ProcessResult(false, "MRP is already in progress");
            }

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
