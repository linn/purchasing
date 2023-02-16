namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Boms;

    public class BomHistoryReportService : IBomHistoryReportService
    {
        private readonly IBomTreeService treeService;

        private readonly IQueryRepository<BomHistoryViewEntry> bomHistoryRepository;

        public BomHistoryReportService(
            IBomTreeService treeService,
            IQueryRepository<BomHistoryViewEntry> bomHistoryRepository)
        {
            this.treeService = treeService;
            this.bomHistoryRepository = bomHistoryRepository;
        }

        public IEnumerable<BomHistoryViewEntry> GetReport(
            string bomName, DateTime from, DateTime to)
        {
            return this.bomHistoryRepository.FilterBy(
                    x => x.DateApplied >= from
                         && x.DateApplied <= to
                         && x.BomName == bomName)
                .OrderBy(x => x.ChangeId).ThenBy(x => x.DetailId);
        }

        public IEnumerable<BomHistoryViewEntry> GetReportWithSubAssemblies(
            string bomName, DateTime from, DateTime to)
        {
            var subAssemblies = this.treeService.FlattenBomTree(bomName, null, false)
                .Where(x => x.Type != "C").Select(x => x.Name);

            var result = this.bomHistoryRepository.FilterBy(
                    x => x.DateApplied >= from && x.DateApplied <= to && subAssemblies.Contains(x.BomName))
                .OrderBy(x => x.ChangeId).ThenBy(x => x.DetailId);

            return result;
        }
    }
}
