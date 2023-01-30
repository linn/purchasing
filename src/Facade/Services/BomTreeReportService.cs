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

        public IResult<BomTreeNode> GetTree(
            string bomName, 
            int? levels = null,
            bool requirementOnly = true,
            bool showChanges = false,
            string treeType = "bom")
        {
            if (treeType == "bom")
            {
                return new SuccessResult<BomTreeNode>(this.domainService.BuildBomTree(bomName, levels, requirementOnly, showChanges));
            }
            else
            {
                return new SuccessResult<BomTreeNode>(this.domainService.BuildWhereUsedTree(bomName, levels, requirementOnly, showChanges));
            }
        }

        public IEnumerable<IEnumerable<string>> GetFlatTreeExport(
            string bomName, 
            int? levels,
            bool requirementOnly = true,
            bool showChanges = false,
            string treeType = "bom")
        {
            var flattened = treeType == "bom" ? this.domainService.FlattenBomTree(bomName, levels, requirementOnly, showChanges)
                                : this.domainService.FlattenWhereUsedTree(bomName, levels, requirementOnly, showChanges);
            var csvData = new List<List<string>>();
            foreach (var node in flattened)
            {
                csvData.Add(new List<string>
                                {
                                    node.Name,
                                    node.Description,
                                    node.Qty.ToString(CultureInfo.InvariantCulture),
                                    node.ParentName
                                });
            }

            if (csvData.Count > 0)
            {
                csvData.RemoveAt(0);
                csvData.Insert(0, new List<string> { bomName, "DESCRIPTION", "QTY", "PARENT" });
            }
            
            return csvData;
        }
    }
}
