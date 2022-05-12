namespace Linn.Purchasing.Domain.LinnApps.MaterialRequirements
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;

    public class MaterialRequirementsReportService : IMaterialRequirementsReportService
    {
        private readonly IQueryRepository<MrHeader> repository;

        private readonly IRepository<MrpRunLog, int> runLogRepository;

        private readonly ISingleRecordRepository<MrMaster> masterRepository;

        public MaterialRequirementsReportService(
            IQueryRepository<MrHeader> repository,
            IRepository<MrpRunLog, int> runLogRepository,
            ISingleRecordRepository<MrMaster> masterRepository)
        {
            this.repository = repository;
            this.runLogRepository = runLogRepository;
            this.masterRepository = masterRepository;
        }

        public MrReport GetMaterialRequirements(string jobRef, IEnumerable<string> partNumbers)
        {
            if (string.IsNullOrEmpty(jobRef))
            {
                var master = this.masterRepository.GetRecord();
                jobRef = master.JobRef;
            }

            var runLog = this.runLogRepository.FindBy(a => a.JobRef == jobRef);

            var report = new MrReport
                             {
                                 RunWeekNumber = runLog.RunWeekNumber,
                                 Headers = this.repository.FilterBy(
                                     a => a.JobRef == jobRef && partNumbers.Contains(a.PartNumber)).ToList()
                             };
            return report;
        }
    }
}
