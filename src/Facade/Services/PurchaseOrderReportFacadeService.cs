namespace Linn.Purchasing.Facade.Services
{
    using System;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Resources;

    public class PurchaseOrderReportFacadeService : IPurchaseOrderReportFacadeService
    {
        private readonly IPurchaseOrdersReportService domainService;

        public PurchaseOrderReportFacadeService(IPurchaseOrdersReportService domainService)
        {
            this.domainService = domainService;
        }

        public IResult<ResultsModel> GetOrdersBySupplierReport(OrdersBySupplierSearchResource resource)
        {
            var fromValid = DateTime.TryParse(resource.From, out var from);
            var toValid = DateTime.TryParse(resource.To, out var to);

            if (!fromValid || !toValid)
            {
                return new BadRequestResult<ResultsModel>(
                    "Invalid dates supplied to orders by supplier report");
            }

            return new SuccessResult<ResultsModel>(this.domainService.GetOrdersBySupplierReport(from, to, resource.SupplierId));

        }
    }
}
