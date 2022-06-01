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
            IEnumerable<string> parts);

        MrReportOptions GetOptions();
    }
}
