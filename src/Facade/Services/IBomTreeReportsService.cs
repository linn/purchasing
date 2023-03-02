namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    public interface IBomTreeReportsService
    {
        IResult<BomTreeNode> GetTree(
            string bomName, 
            int? levels = null,
            bool requirementOnly = true,
            bool showChanges = false,
            string treeType = "bom");

        CsvResult GetFlatTreeExport(
            string bomName, 
            int? levels,
            bool requirementOnly = true,
            bool showChanges = false,
            string treeType = "bom");
    }
}
