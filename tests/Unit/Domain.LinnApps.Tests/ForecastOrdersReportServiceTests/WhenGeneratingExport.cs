namespace Linn.Purchasing.Domain.LinnApps.Tests.ForecastOrdersReportServiceTests
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using FluentAssertions;

    using NUnit.Framework;

    public class WhenGeneratingExport : ContextBase
    {
        private IEnumerable<IEnumerable<string>> result;

        [SetUp]
        public void SetUp()
        {
            this.result = this.Sut.GetMonthlyExport(this.Parts.First().PreferredSupplier);
        }

        [Test]
        public void ShouldBuildCsvDataCorrectly()
        {
            var topRow = this.result.ElementAt(0).ToList();
            topRow.ElementAt(0).Should().Be("Linn Part Number");
            topRow.ElementAt(1).Should().Be("Current Stock");
            topRow.ElementAt(2).Should().Be("Base Unit Price");
            topRow.ElementAt(3).Should().Be("MOQ");
            topRow.ElementAt(4).Should().Be("Stock @ Month End");
            var index = 5;

            foreach (var m in this.MonthStrings)
            {
                topRow.ElementAt(index).Should().Be(m.MmmYy);
                index++;
            }

            var spacerRow = this.result.ElementAt(1).ToList();
            spacerRow.ForEach(s => s.Should().Be(string.Empty));

            var usagesRow = this.result.ElementAt(2).ToList();

            usagesRow.ElementAt(0).Should().Be(this.Parts.First().MrPartNumber);
            usagesRow.ElementAt(1).Should().Be(this.Parts.First()
                .StartingQty.ToString(CultureInfo.InvariantCulture));
            usagesRow.ElementAt(2).Should().Be(this.Parts.First()
                .UnitPrice.ToString(CultureInfo.InvariantCulture));
            usagesRow.ElementAt(3).Should().Be(this.Parts.First()
                .MinimumOrderQty.ToString(CultureInfo.InvariantCulture));
            usagesRow.ElementAt(4).Should().Be("Usage");

            var stockRow = this.result.ElementAt(3).ToList();
            stockRow.ElementAt(4).Should().Be("Stock");

            var ordersRow = this.result.ElementAt(4).ToList();
            ordersRow.ElementAt(4).Should().Be("Orders");

            var forecastRow = this.result.ElementAt(5).ToList();
            forecastRow.ElementAt(4).Should().Be("Forecast");

            index = 5;
            foreach (var v in this.Values)
            {
                usagesRow.ElementAt(index).Should().Be(v.Usages.ToString());
                ordersRow.ElementAt(index).Should().Be(v.Orders.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
                stockRow.ElementAt(index).Should().Be(v.Stock.ToString());
                forecastRow.ElementAt(index).Should().Be(v.ForecastOrders.ToString());
                index++;
            }

            usagesRow.ElementAt(index).Should().Be("140"); // total of all usages
            forecastRow.ElementAt(index).Should().Be("80"); // total of all forecasts
        }
    }
}
