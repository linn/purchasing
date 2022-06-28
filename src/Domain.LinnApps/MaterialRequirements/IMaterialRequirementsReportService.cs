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
            IEnumerable<string> parts,
            int reportSegment = 0);

        MrReportOptions GetOptions();

        IEnumerable<MrPurchaseOrderDetail> GetMaterialRequirementsOrders(
            string requestJobRef,
            IEnumerable<string> parts);
    }
}
