namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Linn.Common.Domain.Exceptions;
    using Linn.Common.Facade;
    using Linn.Common.Pdf;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;

    public class BomReportsFacadeService : IBomReportsFacadeService
    {
        private readonly IBomReportsService domainService;

        private readonly IReportReturnResourceBuilder reportReturnResourceBuilder;

        private readonly IHtmlTemplateService<BomCostReports> bomCostReportHtmlService;

        private readonly IPdfService pdfService;

        public BomReportsFacadeService(
            IBomReportsService domainService,
            IReportReturnResourceBuilder reportReturnResourceBuilder,
            IHtmlTemplateService<BomCostReports> bomCostReportHtmlService,
            IPdfService pdfService)
        {
            this.domainService = domainService;
            this.reportReturnResourceBuilder = reportReturnResourceBuilder;
            this.bomCostReportHtmlService = bomCostReportHtmlService;
            this.pdfService = pdfService;
        }

        public IResult<ReportReturnResource> GetPartsOnBomReport(string bomName)
        {
            var resource = this.reportReturnResourceBuilder.Build(
                this.domainService.GetPartsOnBomReport(bomName.ToUpper().Trim()));

            return new SuccessResult<ReportReturnResource>(resource);
        }

        public IResult<ReportReturnResource> GetBomDifferencesReport(string bom1, string bom2)
        {
            var resource = 
                this.reportReturnResourceBuilder
                    .Build(this.domainService.GetBomDifferencesReport(
                        bom1.Trim().ToUpper(), bom2.Trim().ToUpper()));

            return new SuccessResult<ReportReturnResource>(resource);
        }

        public IResult<IEnumerable<BomCostReportResource>> GetBomCostReport(
            string bomName, bool splitBySubAssembly, int levels, decimal labourHourlyRate)
        {
            var result = this.domainService
                .GetBomCostReport(bomName.Trim().ToUpper(), splitBySubAssembly, levels, labourHourlyRate)
                .Select(r => new BomCostReportResource
                                          {
                                              Breakdown = this.reportReturnResourceBuilder.Build(r.Breakdown),
                                              SubAssembly = r.SubAssembly,
                                              MaterialTotal = r.MaterialTotal,
                                              StandardTotal = r.StandardTotal
                                          });
            return new SuccessResult<IEnumerable<BomCostReportResource>>(result);
        }

        public IResult<ReportReturnResource> GetBoardDifferenceReport(
            BomDifferenceReportRequestResource resource)
        {
            ResultsModel result;

            try
            {
                result = this.domainService.GetBoardDifferenceReport(
                    resource.BoardCode1,
                    resource.RevisionCode1,
                    resource.BoardCode2,
                    resource.RevisionCode2,
                    resource.LiveOnly);
            }
            catch (DomainException exception)
            {
                return new BadRequestResult<ReportReturnResource>(exception.Message);
            }

            return new SuccessResult<ReportReturnResource>(this.reportReturnResourceBuilder.Build(result));
        }

        public IResult<ReportReturnResource> GetBoardComponentSummaryReport(
            BoardComponentSummaryReportRequestResource resource)
        {
            ResultsModel result;

            try
            {
                result = this.domainService.GetBoardComponentSummaryReport(
                    resource.BoardCode,
                    resource.RevisionCode);
            }
            catch (DomainException exception)
            {
                return new BadRequestResult<ReportReturnResource>(exception.Message);
            }

            return new SuccessResult<ReportReturnResource>(this.reportReturnResourceBuilder.Build(result));
        }

        public Stream GetBomCostReportPdf(string bomName, bool splitBySubAssembly, int levels, decimal labourHourlyRate)
        {
            var data = new BomCostReports
                           {
                               BomCosts = this.domainService.GetBomCostReport(
                                   bomName.ToUpper().Trim(),
                                   splitBySubAssembly,
                                   levels,
                                   labourHourlyRate)
                           };

            var html = this.bomCostReportHtmlService.GetHtml(data).Result;
            var pdf = this.pdfService.ConvertHtmlToPdf(html, true).Result;
            return pdf;
        }
    }
}
