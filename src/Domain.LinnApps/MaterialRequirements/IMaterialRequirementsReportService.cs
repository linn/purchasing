namespace Linn.Purchasing.Domain.LinnApps.MaterialRequirements
{
    using System.Collections.Generic;

    public interface IMaterialRequirementsReportService
    {
        MrReport GetMaterialRequirements(
            string requestJobRef,
            string requestTypeOfReport,
            string requestPartSelector,
            string stockLevelSelector,
            string orderBySelector,
            IEnumerable<string> parts,
            int reportSegment = 0);

        MrReportOptions GetOptions();

        IEnumerable<MrPurchaseOrderDetail> GetMaterialRequirementsOrders(
            string requestJobRef,
            IEnumerable<string> parts);
    }
}
