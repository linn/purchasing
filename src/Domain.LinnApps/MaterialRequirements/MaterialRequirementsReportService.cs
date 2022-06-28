namespace Linn.Purchasing.Domain.LinnApps.MaterialRequirements
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public class MaterialRequirementsReportService : IMaterialRequirementsReportService
    {
        private readonly IQueryRepository<MrHeader> repository;

        private readonly IRepository<MrpRunLog, int> runLogRepository;

        private readonly ISingleRecordRepository<MrMaster> masterRepository;

        private readonly IRepository<Planner, int> plannerRepository;

        private readonly IRepository<Employee, int> employeeRepository;

        private readonly IQueryRepository<MrPurchaseOrderDetail> purchaseOrdersRepository;

        private readonly IQueryRepository<PartAndAssembly> partsAndAssembliesRepository;

        private Expression<Func<MrHeader, bool>> filterQuery;

        public MaterialRequirementsReportService(
            IQueryRepository<MrHeader> repository,
            IRepository<MrpRunLog, int> runLogRepository,
            ISingleRecordRepository<MrMaster> masterRepository,
            IRepository<Planner, int> plannerRepository,
            IRepository<Employee, int> employeeRepository,
            IQueryRepository<MrPurchaseOrderDetail> purchaseOrdersRepository,
            IQueryRepository<PartAndAssembly> partsAndAssembliesRepository)
        {
            this.repository = repository;
            this.runLogRepository = runLogRepository;
            this.masterRepository = masterRepository;
            this.plannerRepository = plannerRepository;
            this.employeeRepository = employeeRepository;
            this.purchaseOrdersRepository = purchaseOrdersRepository;
            this.partsAndAssembliesRepository = partsAndAssembliesRepository;
        }

        public MrReport GetMaterialRequirements(
            string jobRef,
            string typeOfReport,
            string partSelector,
            string stockLevelSelector,
            string orderBySelector,
            IEnumerable<string> partNumbers,
            int reportSegment = 0)
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

            switch (partSelector)
            {
                case "Parts Used On":
                    partNumbers = this.GetComponents(partNumbers, false, true).Distinct();
                    break;
                case "Assemblies Used On":
                    partNumbers = this.GetComponents(partNumbers, true, true).Distinct();
                    break;
                case "Parts Where Used":
                    throw new InvalidOptionException("Where used option not yet supported");
            }

            if (partSelector.StartsWith("Planner"))
            {
                var planner = int.Parse(partSelector.Substring(7));
                this.filterQuery = a => a.JobRef == jobRef && a.Planner == planner;
            }
            else
            {
                this.filterQuery = a => a.JobRef == jobRef && partNumbers.Contains(a.PartNumber);
            }

            var results = this.repository.FilterBy(this.filterQuery);

            switch (stockLevelSelector)
            {
                case "0-4":
                    results = results.Where(a => a.DangerLevel >= 0 && a.DangerLevel <= 4);
                    break;
                case "0-2":
                    results = results.Where(a => a.DangerLevel >= 0 && a.DangerLevel <= 2);
                    break;
                case "0" or "1" or "2" or "3" or "4":
                    results = results.Where(a => a.DangerLevel == int.Parse(stockLevelSelector));
                    break;
            }

            results = orderBySelector switch
                {
                    "part" => results.OrderBy(a => a.PartNumber),
                    "supplier/part" => results.OrderBy(a => a.PreferredSupplierId).ThenBy(b => b.PartNumber),
                    _ => results
                };

            // deploying before working out where this needs to be set
            // results = results.Skip(reportSegment * 100).Take(100);

            var report = new MrReport
                             {
                                 JobRef = jobRef,
                                 RunWeekNumber = runLog.RunWeekNumber,
                                 Headers = results
                             };
            return report;
        }

        private IEnumerable<string> GetComponents(IEnumerable<string> partNumbers, bool assembliesOnly, bool addParentParts = false)
        {
            var results = new List<string>();

            foreach (var partNumber in partNumbers)
            {
                if (addParentParts)
                {
                    results.Add(partNumber);
                }

                var components = this.partsAndAssembliesRepository.FilterBy(a => a.AssemblyNumber == partNumber);

                results.AddRange(
                    assembliesOnly == false
                        ? components.Where(a => a.PartBomType != "P").Select(a => a.PartNumber)
                        : components.Where(a => a.PartBomType == "A").Select(a => a.PartNumber));

                var assemblies = components.Where(a => a.PartBomType != "C");
                if (assemblies.Any())
                {
                    results.AddRange(this.GetComponents(assemblies.Select(a => a.PartNumber), assembliesOnly));
                }
            }

            return results;
        }

        public MrReportOptions GetOptions()
        {
            var partSelectorOptions = new List<ReportOption>
                                          {
                                              new ReportOption("Select Parts", "Select Parts", 0, "parts")
                                                  {
                                                      DefaultOption = true
                                                  },
                                              new ReportOption("Parts Used On", "Components Of...", 1, "parts") ,
                                              new ReportOption("Assemblies Used On", "Assemblies Of...",2, "parts"),
                                              new ReportOption("Parts Where Used", "Assemblies Containing...", 3, "parts")
                                          };
            var stockLevelOptions = new List<ReportOption>
                                        {
                                            new ReportOption("0-4", "Danger Levels 0 - 4", 0),
                                            new ReportOption("0-2", "Danger Levels 0 - 2", 1),
                                            new ReportOption("All", "All Stock Levels", 2) { DefaultOption = true },
                                            new ReportOption("0", "Danger Level 0 Short for triggered builds", 3),
                                            new ReportOption("1", "Danger Level 1 Short now", 4),
                                            new ReportOption("2", "Danger Level 2 Zero at lead time", 5),
                                            new ReportOption("3", "Danger Level 3 Low at lead time", 6),
                                            new ReportOption("4", "Danger Level 4 Very low before lead time", 7)
                                        };

            var orderByOptions = new List<ReportOption>
                                        {
                                            new ReportOption("supplier/part", "Supplier Id Then Part", 0)
                                                {
                                                    DefaultOption = true
                                                },
                                            new ReportOption("part", "Part Number", 1)
                                        };

            var planners = this.plannerRepository.FindAll();

            foreach (var planner in planners.Where(a => a.ShowAsMrOption == "Y"))
            {
                var employee = this.employeeRepository.FindById(planner.Id);
                partSelectorOptions.Add(new ReportOption($"Planner{planner.Id}", $"{employee.FullName}'s Suppliers", null, "planner"));
            }

            var displaySequence = 4;
            foreach (var partSelectorOption in partSelectorOptions.Where(a => a.DisplaySequence is null).OrderBy(b => b.DisplayText))
            {
                partSelectorOption.DisplaySequence = displaySequence++;
            }

            return new MrReportOptions
                       {
                           PartSelectorOptions = partSelectorOptions,
                           StockLevelOptions = stockLevelOptions,
                           OrderByOptions = orderByOptions
                       };
        }

        public IEnumerable<MrPurchaseOrderDetail> GetMaterialRequirementsOrders(
            string jobRef,
            IEnumerable<string> parts)
        {
            var results =
                this.purchaseOrdersRepository.FilterBy(a => a.JobRef == jobRef && parts.Contains(a.PartNumber));

            return results.OrderBy(a => a.OrderNumber).ThenBy(b => b.OrderLine);
        }
    }
}
