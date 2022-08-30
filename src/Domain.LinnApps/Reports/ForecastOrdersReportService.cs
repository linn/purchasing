namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    public class ForecastOrdersReportService : IForecastOrdersReportService
    {
        private readonly IQueryRepository<MonthlyForecastPart> monthlyForecastPartRepository;

        private readonly IQueryRepository<MonthlyForecastPartRequirement> monthlyForecastPartRequirementRepository;

        private readonly IQueryRepository<ForecastReportMonth> forecastReportMonthsRepository;

        public ForecastOrdersReportService(
            IQueryRepository<MonthlyForecastPart> monthlyForecastPartRepository,
            IQueryRepository<MonthlyForecastPartRequirement> monthlyForecastPartRequirementRepository,
            IQueryRepository<ForecastReportMonth> forecastReportMonthsRepository)
        {
            this.monthlyForecastPartRepository = monthlyForecastPartRepository;
            this.monthlyForecastPartRequirementRepository = monthlyForecastPartRequirementRepository;
            this.forecastReportMonthsRepository = forecastReportMonthsRepository;
        }

        public IEnumerable<IEnumerable<string>> GetMonthlyExport(int supplierId)
        {
            var parts = this.monthlyForecastPartRepository.FilterBy(x => x.PreferredSupplier == supplierId).ToList();
            var months = this.forecastReportMonthsRepository.FindAll();
            var monthlyRequirements = this.monthlyForecastPartRequirementRepository.FilterBy(
                x => parts.Select(p => p.MrPartNumber).Contains(x.PartNumber)).ToList()
                .GroupBy(r => r.PartNumber);

            var result = new List<List<string>>();
            var firstRow = new List<string>();
            firstRow.Add("Linn Part Number");
            firstRow.Add("Current Stock");

            firstRow.Add("Base Unit Price");
            firstRow.Add("MOQ");
            firstRow.Add("Stock @ Month End");

            firstRow.AddRange(months.Select(m => m.MmmYy));

            firstRow.Add("Total YEAR");
            result.Add(firstRow);

            monthlyRequirements.ToList().ForEach(
                partGroup =>
                    {
                        var usageRow = new List<string>();
                        usageRow.Add(partGroup.Key);
                        usageRow.Add("61");
                        usageRow.Add("131.2100");
                        usageRow.Add("40");
                        usageRow.Add("Usage");
                        foreach (var m in partGroup)
                        {
                            usageRow.Add("1"); // usages value
                        }
                        usageRow.Add("<total>");

                        result.Add(usageRow);

                        var stockRow = new List<string>();
                        stockRow.Add("DESC");
                        stockRow.Add(string.Empty);
                        stockRow.Add(string.Empty);
                        stockRow.Add(string.Empty);
                        stockRow.Add("Stock");

                        foreach (var m in partGroup)
                        {
                            stockRow.Add("1"); //stock value
                        }

                        stockRow.Add(string.Empty);

                        result.Add(stockRow);

                        var ordersRow = new List<string>();
                        ordersRow.Add(string.Empty);
                        ordersRow.Add(string.Empty);
                        ordersRow.Add(string.Empty);
                        ordersRow.Add(string.Empty);
                        ordersRow.Add("Orders");

                        foreach (var m in partGroup)
                        {
                            ordersRow.Add(string.Empty); // orders value
                        }

                        ordersRow.Add(string.Empty);
                        result.Add(ordersRow);

                        var forecastRow = new List<string>();
                        forecastRow.Add(string.Empty);
                        forecastRow.Add(string.Empty);
                        forecastRow.Add(string.Empty);
                        forecastRow.Add(string.Empty);
                        forecastRow.Add("Forecast");

                        foreach (var m in partGroup)
                        {
                            forecastRow.Add(m.NettRequirementK); // reqt value
                        }

                        forecastRow.Add(string.Empty);
                        result.Add(forecastRow);
                    });
            
            return result;
        }
    }
}
