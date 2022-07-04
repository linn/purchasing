namespace Linn.Purchasing.Domain.LinnApps.MaterialRequirements
{
    using System.Collections.Generic;

    public interface IMaterialRequirementsReportService
    {
        MrReport GetMaterialRequirements(
            string jobRef,
            string typeOfReport,
            string partSelector,
            string stockLevelOption,
            string partOption,
            string orderBy,
            int? supplierId,
            IEnumerable<string> parts,
            string partNumberList,
            string stockCategoryName,
            int reportChunk = 0);

        MrReportOptions GetOptions();

        IEnumerable<MrPurchaseOrderDetail> GetMaterialRequirementsOrders(
            string requestJobRef,
            IEnumerable<string> parts);
    }
}
