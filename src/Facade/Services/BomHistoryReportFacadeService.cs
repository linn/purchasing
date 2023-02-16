namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Resources;

    public class BomHistoryReportFacadeService : IBomHistoryReportFacadeService
    {
        private readonly IBomHistoryReportService domainService;

        public BomHistoryReportFacadeService(IBomHistoryReportService domainService)
        {
            this.domainService = domainService;
        }

        public IResult<IEnumerable<BomHistoryReportLineResource>> GetReport(
            string bomName, string from, string to, bool includeSubAssemblies)
        {
            if (includeSubAssemblies)
            {
                return new SuccessResult<IEnumerable<BomHistoryReportLineResource>>(
                    this.domainService.GetReportWithSubAssemblies(
                        bomName,
                        DateTime.Parse(from),
                        DateTime.Parse(to)).Select(BuildResource));
            }

            return new SuccessResult<IEnumerable<BomHistoryReportLineResource>>(
                this.domainService.GetReport(
                bomName,
                DateTime.Parse(from),
                DateTime.Parse(to)).Select(BuildResource));
        }

        private static BomHistoryReportLineResource BuildResource(BomHistoryViewEntry e)
        {
            return new BomHistoryReportLineResource
                       {
                           ChangeId = e.ChangeId,
                           BomName = e.BomName,
                           DocumentType = e.DocumentType,
                           DocumentNumber = e.DocumentNumber,
                           DateApplied = e.DateApplied?.ToString("o"),
                           AppliedBy = e.AppliedBy,
                           Operation = e.Operation,
                           PartNumber = e.PartNumber,
                           Qty = e.Qty,
                           GenerateRequirement = e.GenerateRequirement,
                           ReplaceSeq = e.ReplaceSeq,
                           DetailId = e.DetailId
                       };
        }
    }
}
