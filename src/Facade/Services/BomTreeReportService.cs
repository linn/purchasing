namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;
    using System.Globalization;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    public class BomTreeReportService : IBomTreeReportsService
    {
        private readonly IBomTreeService domainService;

        public BomTreeReportService(
           IBomTreeService domainService)
        {
           this.domainService = domainService;
        }

        public IResult<BomTreeNode> GetBomTree(string bomName, int? levels = null)
        {
            return new SuccessResult<BomTreeNode>(this.domainService.BuildTree(bomName, levels));
        }

        public IEnumerable<IEnumerable<string>> GetFlatBomTreeExport(string bomName, int? levels)
        {
            var flattened = this.domainService.FlattenTree(bomName, levels);
            var csvData = new List<List<string>>();
            foreach (var node in flattened)
            {
                csvData.Add(new List<string>
                                {
                                    node.Name,
                                    node.Description,
                                    node.Qty.ToString(CultureInfo.InvariantCulture)
                                });
            }

            return csvData;
        }
    }
}
