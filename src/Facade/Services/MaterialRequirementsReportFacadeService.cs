namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Resources.MaterialRequirements;

    public class MaterialRequirementsReportFacadeService : IMaterialRequirementsReportFacadeService
    {
        private readonly IMaterialRequirementsReportService materialRequirementsReportService;

        private readonly IBuilder<MrReport> builder;

        private readonly IBuilder<MrReportOptions> optionsBuilder;

        private readonly IBuilder<MrPurchaseOrderDetail> ordersBuilder;

        public MaterialRequirementsReportFacadeService(
            IMaterialRequirementsReportService materialRequirementsReportService,
            IBuilder<MrReport> builder,
            IBuilder<MrReportOptions> optionsBuilder,
            IBuilder<MrPurchaseOrderDetail> ordersBuilder)
        {
            this.materialRequirementsReportService = materialRequirementsReportService;
            this.builder = builder;
            this.optionsBuilder = optionsBuilder;
            this.ordersBuilder = ordersBuilder;
        }

        public IResult<MrReportResource> GetMaterialRequirements(MrRequestResource request, IEnumerable<string> privileges)
        {
            var parts = this.GetPartOrParts(request);

            var report = this.materialRequirementsReportService.GetMaterialRequirements(
                request.JobRef,
                request.TypeOfReport,
                request.PartSelector,
                request.StockLevelSelector,
                request.PartOption,
                request.OrderBySelector,
                request.SupplierId,
                parts);

            return new SuccessResult<MrReportResource>((MrReportResource)this.builder.Build(report, privileges));
        }

        public IResult<MrReportOptionsResource> GetOptions(IEnumerable<string> privileges)
        {
            var reportOptions = this.materialRequirementsReportService.GetOptions();
            return new SuccessResult<MrReportOptionsResource>((MrReportOptionsResource)this.optionsBuilder.Build(reportOptions, privileges));
        }

        public IResult<MrPurchaseOrdersResource> GetMaterialRequirementOrders(MrRequestResource request, IEnumerable<string> privileges)
        {
            var parts = this.GetPartOrParts(request);

            var orders = this.materialRequirementsReportService.GetMaterialRequirementsOrders(
                request.JobRef,
                parts);

            var result = orders.Select(a => (MrPurchaseOrderResource)this.ordersBuilder.Build(a, null));

            return new SuccessResult<MrPurchaseOrdersResource>(new MrPurchaseOrdersResource { Orders = result });
        }

        private IEnumerable<string> GetPartOrParts(MrRequestResource request)
        {
            return string.IsNullOrEmpty(request.PartNumber)
                       ? request.PartNumbers
                       : new List<string> { request.PartNumber };
        }
    }
}
