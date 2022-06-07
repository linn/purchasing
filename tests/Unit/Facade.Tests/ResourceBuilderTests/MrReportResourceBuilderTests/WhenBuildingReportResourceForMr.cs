namespace Linn.Purchasing.Facade.Tests.ResourceBuilderTests.MrReportResourceBuilderTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Purchasing.Resources.MaterialRequirements;

    using NUnit.Framework;

    public class WhenBuildingReportResourceForMr : ContextBase
    {
        private MrReportResource result;

        [SetUp]
        public void SetUp()
        {
            this.result = (MrReportResource)this.Sut.Build(this.Report, new List<string>());
        }

        [Test]
        public void ShouldCreateHeaderResources()
        {
            this.result.Results.Should().HaveCount(2);
            this.result.Results.Should().Contain(a => a.PartNumber == "P1");
            this.result.Results.Should().Contain(a => a.PartNumber == "P2");
            var part1Result = this.result.Results.First(a => a.PartNumber == "P1");
            part1Result.PartDescription.Should().Be(this.MrHeader1.PartDescription);
            part1Result.AnnualUsage.Should().Be(this.MrHeader1.AnnualUsage);
            part1Result.BaseUnitPrice.Should().Be(this.MrHeader1.BaseUnitPrice);
            part1Result.CurrencyCode.Should().Be(this.MrHeader1.CurrencyCode);
            part1Result.CurrencyUnitPrice.Should().Be(this.MrHeader1.CurrencyUnitPrice);
            part1Result.LeadTimeWeeks.Should().Be(this.MrHeader1.LeadTimeWeeks);
            part1Result.MinimumOrderQuantity.Should().Be(this.MrHeader1.MinimumOrderQuantity);
            part1Result.MinimumDeliveryQuantity.Should().Be(this.MrHeader1.MinimumDeliveryQuantity);
            part1Result.OrderIncrement.Should().Be(this.MrHeader1.OrderIncrement);
            part1Result.OrderUnits.Should().Be(this.MrHeader1.OrderUnits);
            part1Result.OurUnits.Should().Be(this.MrHeader1.OurUnits);
            part1Result.VendorManager.Should().Be(this.MrHeader1.VendorManager);
            part1Result.VendorManagerInitials.Should().Be(this.MrHeader1.VendorManagerInitials);
            part1Result.QuantityAtSupplier.Should().Be(this.MrHeader1.QuantityAtSupplier);
            part1Result.QuantityFaulty.Should().Be(this.MrHeader1.QuantityFaulty);
            part1Result.QuantityForSpares.Should().Be(this.MrHeader1.QuantityForSpares);
            part1Result.QuantityInStock.Should().Be(this.MrHeader1.QuantityInStock);
            part1Result.QuantityInInspection.Should().Be(this.MrHeader1.QuantityInInspection);
            part1Result.Planner.Should().Be(this.MrHeader1.Planner);
            part1Result.PreferredSupplierId.Should().Be(this.MrHeader1.PreferredSupplierId);
            part1Result.PreferredSupplierName.Should().Be(this.MrHeader1.PreferredSupplierName);
        }

        [Test]
        public void ShouldCreateAllDetailsForFirstPart()
        {
            var part1Result = this.result.Results.First(a => a.PartNumber == "P1");
            part1Result.Details.Should().HaveCount(120);
            part1Result.Details.Should().Contain(a => a.Segment == 0);
            part1Result.Details.Should().Contain(a => a.Segment == 1);
            var part1Details1Resources = part1Result.Details.Where(a => a.Segment == 0).ToList();
            var originalPart1Details = this.MrHeader1.MrDetails.First(a => a.Segment == 0);
            part1Details1Resources.First(a => a.Title == "Week").Week0.Should().Be(originalPart1Details.WeekAndYear);
            part1Details1Resources.First(a => a.Title == "Ending").Week0.Should().Be(originalPart1Details.WeekEnding);
            part1Details1Resources.First(a => a.Title == "Fixed Build").Week0.Should().Be(originalPart1Details.FixedBuild.ToString());
            part1Details1Resources.First(a => a.Title == "Assumed Build").Week0.Should().Be(originalPart1Details.AssumedBuild.ToString());
            part1Details1Resources.First(a => a.Title == "Trigger Build").Week0.Should().Be(originalPart1Details.TriggerBuild.ToString());
            part1Details1Resources.First(a => a.Title == "Purchases").Week0.Should().Be(originalPart1Details.PurchaseOrders.ToString());
            part1Details1Resources.First(a => a.Title == "Unauthorised POs").Week0.Should().Be(originalPart1Details.UnauthorisedPurchaseOrders.ToString());
            part1Details1Resources.First(a => a.Title == "Assumed Purchases").Week0.Should().Be(originalPart1Details.AssumedPurchaseOrders.ToString());
            part1Details1Resources.First(a => a.Title == "Sales Orders").Week0.Should().Be(originalPart1Details.SalesOrders.ToString());
            part1Details1Resources.First(a => a.Title == "Sales Forecast").Week0.Should().Be(originalPart1Details.DeliveryForecast.ToString());
            part1Details1Resources.First(a => a.Title == "Production Reqt").Week0.Should().Be(originalPart1Details.ProductionRequirement.ToString());
            part1Details1Resources.First(a => a.Title == "Prod For Spares").Week0.Should().Be(originalPart1Details.ProductionRequirementForSpares.ToString());
            part1Details1Resources.First(a => a.Title == "Prod For NonProd").Week0.Should().Be(originalPart1Details.ProductionRequirementForNonProduction.ToString());
            part1Details1Resources.First(a => a.Title == "Non Prod Reqt").Week0.Should().Be(originalPart1Details.NonProductionRequirement.ToString());
            part1Details1Resources.First(a => a.Title == "Spares Reqt").Week0.Should().Be(originalPart1Details.SparesRequirement.ToString());
            part1Details1Resources.First(a => a.Title == "Status").Week0.Should().Be(originalPart1Details.Status);
            part1Details1Resources.First(a => a.Title == "Stock").Week0.Should().Be(originalPart1Details.Stock.ToString());
            part1Details1Resources.First(a => a.Title == "Min Rail").Week0.Should().Be(originalPart1Details.MinRail.ToString());
            part1Details1Resources.First(a => a.Title == "Max Rail").Week0.Should().Be(originalPart1Details.MaxRail.ToString());
            part1Details1Resources.Should().NotContain(a => a.Title == "Ideal Stock");
            part1Details1Resources.First(a => a.Title == "Recom Orders").Week0.Should().Be(originalPart1Details.RecommendedOrders.ToString());
            part1Details1Resources.Should().NotContain(a => a.Title == "Recom Stock");
        }

        [Test]
        public void ShouldOnlyCreateRelevantDetailsForSecondPart()
        {
            var part2ResultResource = this.result.Results.First(a => a.PartNumber == "P2");
            part2ResultResource.Details.Should().HaveCount(36);
            part2ResultResource.Details.Should().Contain(a => a.Segment == 0);
            part2ResultResource.Details.Should().Contain(a => a.Segment == 1);
            var part2Details1Resources = part2ResultResource.Details.Where(a => a.Segment == 1).ToList();
            var originalPart2Details = this.MrHeader2.MrDetails.First(a => a.Segment == 1);
            part2Details1Resources.First(a => a.Title == "Week").Week3.Should().Be(originalPart2Details.WeekAndYear);
            part2Details1Resources.First(a => a.Title == "Ending").Week3.Should().Be(originalPart2Details.WeekEnding);
            part2Details1Resources.Should().NotContain(a => a.Title == "Fixed Build");
            part2Details1Resources.Should().NotContain(a => a.Title == "Assumed Build");
            part2Details1Resources.Should().NotContain(a => a.Title == "Trigger Build");
            part2Details1Resources.Should().NotContain(a => a.Title == "Purchases");
            part2Details1Resources.Should().NotContain(a => a.Title == "Unauthorised POs");
            part2Details1Resources.Should().NotContain(a => a.Title == "Assumed Purchases");
            part2Details1Resources.Should().NotContain(a => a.Title == "Sales Orders");
            part2Details1Resources.Should().NotContain(a => a.Title == "Sales Forecast");
            part2Details1Resources.Should().NotContain(a => a.Title == "Production Reqt");
            part2Details1Resources.Should().NotContain(a => a.Title == "Prod For Spares");
            part2Details1Resources.Should().NotContain(a => a.Title == "Prod For NonProd");
            part2Details1Resources.Should().NotContain(a => a.Title == "Non Prod Reqt");
            part2Details1Resources.Should().NotContain(a => a.Title == "Spares Reqt");
            part2Details1Resources.First(a => a.Title == "Status").Week3.Should().Be(originalPart2Details.Status);
            part2Details1Resources.First(a => a.Title == "Stock").Week3.Should().Be(originalPart2Details.Stock.ToString());
            part2Details1Resources.First(a => a.Title == "Min Rail").Week3.Should().Be(originalPart2Details.MinRail.ToString());
            part2Details1Resources.First(a => a.Title == "Max Rail").Week3.Should().Be(originalPart2Details.MaxRail.ToString());
            part2Details1Resources.Should().NotContain(a => a.Title == "Ideal Stock");
            part2Details1Resources.Should().NotContain(a => a.Title == "Recom Orders");
            part2Details1Resources.Should().NotContain(a => a.Title == "Recom Stock");
        }

        [Test]
        public void ShouldReturnCorrectHeaderLinks()
        {
            var part1Resource = this.result.Results.First(a => a.PartNumber == "P1");
            part1Resource.Links.Should().Contain(
                a => a.Rel == "part-used-on" && a.Href
                     == $"/purchasing/material-requirements/used-on-report?partNumber={this.MrHeader1.PartNumber}");
            part1Resource.Links.Should().Contain(
                a => a.Rel == "part" && a.Href == $"/parts/{this.MrHeader1.PartId}");
        }
    }
}
