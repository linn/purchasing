namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Resources;

    public class WhatsInInspectionReportFacadeService : IWhatsInInspectionReportFacadeService
    {
        private readonly IWhatsInInspectionReportService domainService;

        private readonly IBuilder<ResultsModel> resultsModelResourceBuilder;

        public WhatsInInspectionReportFacadeService(
            IWhatsInInspectionReportService domainService,
            IBuilder<ResultsModel> resultsModelResourceBuilder)
        {
            this.domainService = domainService;
            this.resultsModelResourceBuilder = resultsModelResourceBuilder;
        }

        public IResult<IEnumerable<WhatsInInspectionReportResource>> GetReport(
            bool includePartsWithNoOrderNumber = false,
            bool showStockLocations = true,
            bool includeFailedStock = false,
            bool excludeFinishedGoods = false,
            bool showBackOrdered = true)
        {
            try
            {
                var result = this.domainService.GetReport(
                    includePartsWithNoOrderNumber,
                    showStockLocations,
                    includeFailedStock,
                    excludeFinishedGoods,
                    showBackOrdered);

                return new SuccessResult<IEnumerable<WhatsInInspectionReportResource>>(
                    result.Select(m => new WhatsInInspectionReportResource
                                           {
                                               PartNumber = m.PartNumber,
                                               Description = m.Description,
                                               QtyInStock = m.QtyInStock,
                                               QtyInInspection = m.QtyInInspection,
                                               OurUnitOfMeasure = m.OurUnitOfMeasure,
                                               OrdersBreakdown = (ReportReturnResource)this
                                                   .resultsModelResourceBuilder.Build(m.OrdersBreakdown, null)
                                           }));
            }
            catch (Exception e)
            {
                return new BadRequestResult<IEnumerable<WhatsInInspectionReportResource>>(e.Message);
            }
        }
    }
}
