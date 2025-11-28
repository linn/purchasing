namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    public class BomTreeReportService : IBomTreeReportsService
    {
        private readonly IBomTreeService domainService;

        public BomTreeReportService(IBomTreeService domainService)
        {
            this.domainService = domainService;
        }

        public IResult<BomTreeNode> GetTree(
            string bomName,
            int? levels = null,
            bool requirementOnly = true,
            bool showChanges = false,
            string treeType = "bom")
        {
            if (treeType == "bom")
            {
                return new SuccessResult<BomTreeNode>(
                    this.domainService.BuildBomTree(bomName, levels, requirementOnly, showChanges));
            }

            return new SuccessResult<BomTreeNode>(
                this.domainService.BuildWhereUsedTree(bomName, levels, requirementOnly, showChanges));
        }

        public IResult<IEnumerable<BomTreeNode>> GetFlatTreeExport(
            string bomName,
            int? levels,
            bool requirementOnly = true,
            bool showChanges = false,
            string treeType = "bom")
        {
            var flattened = treeType == "bom"
                                ? this.domainService.FlattenBomTree(bomName, levels, requirementOnly, showChanges)
                                : this.domainService.FlattenWhereUsedTree(
                                    bomName,
                                    levels,
                                    requirementOnly,
                                    showChanges);

            return new SuccessResult<IEnumerable<BomTreeNode>>(flattened);
        }
    }
}
