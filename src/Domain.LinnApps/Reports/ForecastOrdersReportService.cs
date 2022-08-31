namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    public class ForecastOrdersReportService : IForecastOrdersReportService
    {
        private readonly IQueryRepository<MonthlyForecastPart> monthlyForecastPartsRepository;

        private readonly IQueryRepository<MonthlyForecastPartValues> monthlyForecastRepository;

        private readonly IQueryRepository<ForecastReportMonth> forecastReportMonthsRepository;

        public ForecastOrdersReportService(
            IQueryRepository<MonthlyForecastPart> monthlyForecastPartRepository,
            IQueryRepository<MonthlyForecastPartValues> monthlyForecastRepository,
            IQueryRepository<ForecastReportMonth> forecastReportMonthsRepository)
        {
            this.monthlyForecastPartsRepository = monthlyForecastPartRepository;
            this.monthlyForecastRepository = monthlyForecastRepository;
            this.forecastReportMonthsRepository = forecastReportMonthsRepository;
        }

        public IEnumerable<IEnumerable<string>> GetMonthlyExport(int supplierId)
        {
            var parts = this.monthlyForecastPartsRepository.FilterBy(x => x.PreferredSupplier == supplierId).ToList();
            var months = this.forecastReportMonthsRepository.FindAll();
            var monthlyForecasts = this.monthlyForecastRepository.FilterBy(
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
            result.Add(new List<string>());

            monthlyForecasts.ToList().ForEach(
                partGroup =>
                    {
                        var usageRow = new List<string>();
                        usageRow.Add(partGroup.Key);
                        usageRow.Add("61");
                        usageRow.Add("131.2100");
                        usageRow.Add("40");
                        usageRow.Add("Usage");

                        var stockRow = new List<string>();
                        stockRow.Add("DESC");
                        stockRow.Add(string.Empty);
                        stockRow.Add(string.Empty);
                        stockRow.Add(string.Empty);
                        stockRow.Add("Stock");

                        var ordersRow = new List<string>();
                        ordersRow.Add(string.Empty);
                        ordersRow.Add(string.Empty);
                        ordersRow.Add(string.Empty);
                        ordersRow.Add(string.Empty);
                        ordersRow.Add("Orders");

                        var forecastRow = new List<string>();
                        forecastRow.Add(string.Empty);
                        forecastRow.Add(string.Empty);
                        forecastRow.Add(string.Empty);
                        forecastRow.Add(string.Empty);
                        forecastRow.Add("Forecast");

                        foreach (var m in partGroup)
                        {
                            usageRow.Add(m.Usages);
                            stockRow.Add(m.Stock);
                            ordersRow.Add(m.Orders);
                            forecastRow.Add(m.ForecastOrders);
                        }

                        usageRow.Add("<usages-total>");
                        stockRow.Add(string.Empty);
                        ordersRow.Add(string.Empty);
                        forecastRow.Add("<forecast-total>");

                        result.Add(usageRow);
                        result.Add(stockRow);
                        result.Add(ordersRow);
                        result.Add(forecastRow);

                        result.Add(new List<string>());
                    });
            
            return result;
        }
    }
}
