namespace Linn.Purchasing.Domain.LinnApps.MaterialRequirements
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    public class MaterialRequirementsReportService : IMaterialRequirementsReportService
    {
        private readonly IQueryRepository<MrHeader> repository;

        private readonly IRepository<MrpRunLog, int> runLogRepository;

        private readonly ISingleRecordRepository<MrMaster> masterRepository;

        private readonly IRepository<Planner, int> plannerRepository;

        private readonly IRepository<Employee, int> employeeRepository;

        private Expression<Func<MrHeader, bool>> filterQuery;

        public MaterialRequirementsReportService(
            IQueryRepository<MrHeader> repository,
            IRepository<MrpRunLog, int> runLogRepository,
            ISingleRecordRepository<MrMaster> masterRepository,
            IRepository<Planner, int> plannerRepository,
            IRepository<Employee, int> employeeRepository)
        {
            this.repository = repository;
            this.runLogRepository = runLogRepository;
            this.masterRepository = masterRepository;
            this.plannerRepository = plannerRepository;
            this.employeeRepository = employeeRepository;
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
            else if (partSelector.StartsWith("Planner"))
            {
                var planner = int.Parse(partSelector.Substring(7));
                this.filterQuery = a => a.JobRef == jobRef && a.Planner == planner;
            }

            var report = new MrReport
                             {
                                 JobRef = jobRef,
                                 RunWeekNumber = runLog.RunWeekNumber,
                                 Headers = this.repository.FilterBy(this.filterQuery).ToList()
                             };
            return report;
        }

        public MrReportOptions GetOptions()
        {
            var partSelectorOptions = new List<ReportOption>
                                          {
                                              new ReportOption("Select Parts", "Select Parts", 0)
                                          };
            var dangerLevelOptions = new List<ReportOption> { new ReportOption("All Danger Levels", "All", 0) };

            var planners = this.plannerRepository.FindAll();

            foreach (var planner in planners.Where(a => a.ShowAsMrOption == "Y"))
            {
                var employee = this.employeeRepository.FindById(planner.Id);
                partSelectorOptions.Add(new ReportOption($"Planner{planner.Id}", $"{employee.FullName}'s Suppliers"));
            }

            var displaySequence = 1;
            foreach (var partSelectorOption in partSelectorOptions.Where(a => a.DisplaySequence is null).OrderBy(b => b.DisplayText))
            {
                partSelectorOption.DisplaySequence = displaySequence++;
            }

            return new MrReportOptions
                       {
                           PartSelectorOptions = partSelectorOptions, DangerLevelOptions = dangerLevelOptions
                       };
        }
    }
}
