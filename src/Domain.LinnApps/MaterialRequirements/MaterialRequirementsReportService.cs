namespace Linn.Purchasing.Domain.LinnApps.MaterialRequirements
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;

    public class MaterialRequirementsReportService : IMaterialRequirementsReportService
    {
        private readonly IQueryRepository<MrHeader> repository;

        private readonly IRepository<MrpRunLog, int> runLogRepository;

        private readonly ISingleRecordRepository<MrMaster> masterRepository;

        private Expression<Func<MrHeader, bool>> filterQuery;

        public MaterialRequirementsReportService(
            IQueryRepository<MrHeader> repository,
            IRepository<MrpRunLog, int> runLogRepository,
            ISingleRecordRepository<MrMaster> masterRepository)
        {
            this.repository = repository;
            this.runLogRepository = runLogRepository;
            this.masterRepository = masterRepository;
        }

        public MrReport GetMaterialRequirements(
            string jobRef,
            string typeOfReport,
            string partSelector,
            IEnumerable<string> partNumbers)
        {
            if (string.IsNullOrEmpty(jobRef))
            {
                var master = this.masterRepository.GetRecord();
                jobRef = master.JobRef;
            }

            if (typeOfReport != "MR")
            {
                throw new InvalidOptionException("Only standard MR layout is currently supported");
            }

            var runLog = this.runLogRepository.FindBy(a => a.JobRef == jobRef);

            if (partSelector == "Select Parts" || string.IsNullOrEmpty(partSelector))
            {
                this.filterQuery = a => a.JobRef == jobRef && partNumbers.Contains(a.PartNumber);
            }

            var report = new MrReport
                             {
                                 JobRef = jobRef,
                                 RunWeekNumber = runLog.RunWeekNumber,
                                 Headers = this.repository.FilterBy(
                                     this.filterQuery).ToList()
                             };
            return report;
        }
    }
}
