namespace Linn.Purchasing.Facade.Tests.ResourceBuilderTests.MrReportResourceBuilderTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Resources.MaterialRequirements;

    using NUnit.Framework;

    public class WhenBuildingReportResourceForMr : ContextBase
    {
        private MrReportResource result;

        [SetUp]
        public void SetUp()
        {
            this.MrHeader1 = new MrHeader
                                 {
                                     JobRef = this.JobRef,
                                     PartNumber = "P1",
                                     PartDescription = "P1D",
                                     QuantityInStock = 34,
                                     QuantityForSpares = 1,
                                     QuantityInInspection = 2,
                                     QuantityFaulty = 3,
                                     QuantityAtSupplier = 4,
                                     PreferredSupplierId = 123,
                                     PreferredSupplierName = "Supplier Name",
                                     AnnualUsage = 15000,
                                     BaseUnitPrice = 1.24m,
                                     OurUnits = "ONES",
                                     OrderUnits = "TWOS",
                                     LeadTimeWeeks = 3,
                                     CurrencyCode = "EUR",
                                     CurrencyUnitPrice = 2.12m,
                                     MinimumOrderQuantity = 1,
                                     MinimumDeliveryQuantity = 2,
                                     OrderIncrement = 1,
                                     HasProductionRequirement = "Y",
                                     HasNonProductionRequirement = "Y",
                                     HasDeliveryForecast = "Y",
                                     HasSalesOrders = "Y",
                                     HasPurchaseOrders = "Y",
                                     HasAssumedPurchaseOrders = "Y",
                                     HasUnauthPurchaseOrders = "Y",
                                     HasTriggerBuild = "Y",
                                     HasFixedBuild = "Y",
                                     HasAssumedBuild = "Y",
                                     HasSparesRequirement = "Y",
                                     HasProductionRequirementForSpares = "Y",
                                     HasProductionRequirementForNonProduction = "Y",
                                     VendorManager = "S",
                                     VendorManagerInitials = "IP",
                                     PartId = 745654,
                                     Planner = 87,
                                     WeeksUntilDangerous = 3,
                                     MrDetails = new List<MrDetail>
                                                     {
                                                         new MrDetail
                                                             {
                                                                 JobRef = this.JobRef,
                                                                 PartNumber = "P1",
                                                                 LinnWeekNumber = 100,
                                                                 WeekEnding = "20Jun",
                                                                 Segment = 0,
                                                                 WeekAndYear = "25/34",
                                                                 TriggerBuild = 2,
                                                                 PurchaseOrders = 4,
                                                                 AssumedPurchaseOrders = 6,
                                                                 UnauthorisedPurchaseOrders = 0,
                                                                 SalesOrders = 2,
                                                                 DeliveryForecast = 4,
                                                                 ProductionRequirement = 5,
                                                                 NonProductionRequirement = 8,
                                                                 FixedBuild = 3,
                                                                 AssumedBuild = 7,
                                                                 SparesRequirement = 9,
                                                                 ProductionRequirementForSpares = 5,
                                                                 ProductionRequirementForNonProduction = 8,
                                                                 Status = "H",
                                                                 Stock = 300,
                                                                 MinRail = 200,
                                                                 MaxRail = 400,
                                                                 IdealStock = 23,
                                                                 RecommendedOrders = 1,
                                                                 RecommenedStock = 250
                                                             },
                                                         new MrDetail
                                                             {
                                                                 JobRef = this.JobRef,
                                                                 PartNumber = "P1",
                                                                 LinnWeekNumber = 103,
                                                                 WeekEnding = "20Jun",
                                                                 Segment = 0,
                                                                 WeekAndYear = "25/34",
                                                                 TriggerBuild = 2,
                                                                 PurchaseOrders = 4,
                                                                 AssumedPurchaseOrders = 6,
                                                                 UnauthorisedPurchaseOrders = 0,
                                                                 SalesOrders = 2,
                                                                 DeliveryForecast = 4,
                                                                 ProductionRequirement = 5,
                                                                 NonProductionRequirement = 8,
                                                                 FixedBuild = 3,
                                                                 AssumedBuild = 7,
                                                                 SparesRequirement = 9,
                                                                 ProductionRequirementForSpares = 5,
                                                                 ProductionRequirementForNonProduction = 8,
                                                                 Status = "H",
                                                                 Stock = 300,
                                                                 MinRail = 200,
                                                                 MaxRail = 400,
                                                                 IdealStock = 23,
                                                                 RecommendedOrders = 1,
                                                                 RecommenedStock = 250
                                                             },
                                                         new MrDetail
                                                             {
                                                                 JobRef = this.JobRef,
                                                                 PartNumber = "P1",
                                                                 LinnWeekNumber = 115,
                                                                 WeekEnding = "20Jun",
                                                                 Segment = 1,
                                                                 WeekAndYear = "25/34",
                                                                 TriggerBuild = 2,
                                                                 PurchaseOrders = 4,
                                                                 AssumedPurchaseOrders = 6,
                                                                 UnauthorisedPurchaseOrders = 0,
                                                                 SalesOrders = 2,
                                                                 DeliveryForecast = 4,
                                                                 ProductionRequirement = 5,
                                                                 NonProductionRequirement = 8,
                                                                 FixedBuild = 3,
                                                                 AssumedBuild = 7,
                                                                 SparesRequirement = 9,
                                                                 ProductionRequirementForSpares = 5,
                                                                 ProductionRequirementForNonProduction = 8,
                                                                 Status = "H",
                                                                 Stock = 300,
                                                                 MinRail = 200,
                                                                 MaxRail = 400,
                                                                 IdealStock = 23,
                                                                 RecommendedOrders = 1,
                                                                 RecommenedStock = 250
                                                             }
                                                     }
                                 };
            this.MrHeader2 = new MrHeader
                                 {
                                     JobRef = this.JobRef,
                                     PartNumber = "P2",
                                     PartDescription = "P2D",
                                     QuantityInStock = 340,
                                     QuantityForSpares = 1,
                                     QuantityInInspection = 2,
                                     QuantityFaulty = 3,
                                     QuantityAtSupplier = 4,
                                     PreferredSupplierId = 4415,
                                     PreferredSupplierName = "Linn",
                                     AnnualUsage = 15000,
                                     BaseUnitPrice = 1.24m,
                                     OurUnits = "ONES",
                                     OrderUnits = "TWOS",
                                     LeadTimeWeeks = 12,
                                     CurrencyCode = "EUR",
                                     CurrencyUnitPrice = 2.12m,
                                     MinimumOrderQuantity = 1,
                                     MinimumDeliveryQuantity = 2,
                                     OrderIncrement = 1,
                                     HasProductionRequirement = "N",
                                     HasNonProductionRequirement = "N",
                                     HasDeliveryForecast = "N",
                                     HasSalesOrders = "N",
                                     HasPurchaseOrders = "N",
                                     HasAssumedPurchaseOrders = "N",
                                     HasUnauthPurchaseOrders = "N",
                                     HasTriggerBuild = "N",
                                     HasFixedBuild = "N",
                                     HasAssumedBuild = "N",
                                     HasSparesRequirement = "N",
                                     HasProductionRequirementForSpares = "N",
                                     HasProductionRequirementForNonProduction = "N",
                                     VendorManager = "S",
                                     VendorManagerInitials = "IP",
                                     PartId = 53453,
                                     Planner = 34,
                                     MrDetails = new List<MrDetail>
                                                     {
                                                         new MrDetail
                                                             {
                                                                 JobRef = this.JobRef,
                                                                 PartNumber = "P1",
                                                                 LinnWeekNumber = 100,
                                                                 WeekEnding = "20Jun",
                                                                 Segment = 0,
                                                                 WeekAndYear = "25/34",
                                                                 TriggerBuild = 2,
                                                                 PurchaseOrders = 4,
                                                                 AssumedPurchaseOrders = 6,
                                                                 UnauthorisedPurchaseOrders = 0,
                                                                 SalesOrders = 2,
                                                                 DeliveryForecast = 4,
                                                                 ProductionRequirement = 5,
                                                                 NonProductionRequirement = 8,
                                                                 FixedBuild = 3,
                                                                 AssumedBuild = 7,
                                                                 SparesRequirement = 9,
                                                                 ProductionRequirementForSpares = 5,
                                                                 ProductionRequirementForNonProduction = 8,
                                                                 Status = "H",
                                                                 Stock = 300,
                                                                 MinRail = 200,
                                                                 MaxRail = 400,
                                                                 IdealStock = 23,
                                                                 RecommendedOrders = 1,
                                                                 RecommenedStock = 250
                                                             },
                                                         new MrDetail
                                                             {
                                                                 JobRef = this.JobRef,
                                                                 PartNumber = "P1",
                                                                 LinnWeekNumber = 116,
                                                                 WeekEnding = "20Jun",
                                                                 Segment = 1,
                                                                 WeekAndYear = "25/34",
                                                                 TriggerBuild = 2,
                                                                 PurchaseOrders = 4,
                                                                 AssumedPurchaseOrders = 6,
                                                                 UnauthorisedPurchaseOrders = 0,
                                                                 SalesOrders = 2,
                                                                 DeliveryForecast = 4,
                                                                 ProductionRequirement = 5,
                                                                 NonProductionRequirement = 8,
                                                                 FixedBuild = 3,
                                                                 AssumedBuild = 7,
                                                                 SparesRequirement = 9,
                                                                 ProductionRequirementForSpares = 5,
                                                                 ProductionRequirementForNonProduction = 8,
                                                                 Status = "H",
                                                                 Stock = 300,
                                                                 MinRail = 200,
                                                                 MaxRail = 400,
                                                                 IdealStock = 23,
                                                                 RecommendedOrders = 1,
                                                                 RecommenedStock = 250
                                                             }
                                                     }
                                 };

            this.Report = new MrReport
                              {
                                  JobRef = this.JobRef,
                                  RunWeekNumber = 100,
                                  Headers = new List<MrHeader> { this.MrHeader1, this.MrHeader2 }
                              };

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

            part1Details1Resources.First(a => a.Title == "Week").Week0Item.TextValue.Should().Be(originalPart1Details.WeekAndYear);
            part1Details1Resources.First(a => a.Title == "Ending").Week0Item.TextValue.Should().Be(originalPart1Details.WeekEnding);
            part1Details1Resources.First(a => a.Title == "Fixed Build").Week0Item.Value.Should().Be(originalPart1Details.FixedBuild);
            part1Details1Resources.First(a => a.Title == "Assumed Build").Week0Item.Value.Should().Be(originalPart1Details.AssumedBuild);
            part1Details1Resources.First(a => a.Title == "Trigger Build").Week0Item.Value.Should().Be(originalPart1Details.TriggerBuild);
            part1Details1Resources.First(a => a.Title == "Purchases").Week0Item.Value.Should().Be(originalPart1Details.PurchaseOrders);
            part1Details1Resources.First(a => a.Title == "Unauthorised POs").Week0Item.Value.Should().Be(originalPart1Details.UnauthorisedPurchaseOrders);
            part1Details1Resources.First(a => a.Title == "Assumed Purchases").Week0Item.Value.Should().Be(originalPart1Details.AssumedPurchaseOrders);
            part1Details1Resources.First(a => a.Title == "Sales Orders").Week0Item.Value.Should().Be(originalPart1Details.SalesOrders);
            part1Details1Resources.First(a => a.Title == "Sales Forecast").Week0Item.Value.Should().Be(originalPart1Details.DeliveryForecast);
            part1Details1Resources.First(a => a.Title == "Production Reqt").Week0Item.Value.Should().Be(originalPart1Details.ProductionRequirement);
            part1Details1Resources.First(a => a.Title == "Prod For Spares").Week0Item.Value.Should().Be(originalPart1Details.ProductionRequirementForSpares);
            part1Details1Resources.First(a => a.Title == "Prod For NonProd").Week0Item.Value.Should().Be(originalPart1Details.ProductionRequirementForNonProduction);
            part1Details1Resources.First(a => a.Title == "Non Prod Reqt").Week0Item.Value.Should().Be(originalPart1Details.NonProductionRequirement);
            part1Details1Resources.First(a => a.Title == "Spares Reqt").Week0Item.Value.Should().Be(originalPart1Details.SparesRequirement);
            part1Details1Resources.First(a => a.Title == "Status").Week0Item.TextValue.Should().Be(originalPart1Details.Status);
            part1Details1Resources.First(a => a.Title == "Stock").Week0Item.Value.Should().Be(originalPart1Details.Stock);
            part1Details1Resources.First(a => a.Title == "Min Rail").Week0Item.Value.Should().Be(originalPart1Details.MinRail);
            part1Details1Resources.First(a => a.Title == "Max Rail").Week0Item.Value.Should().Be(originalPart1Details.MaxRail);
            part1Details1Resources.First(a => a.Title == "Recom Orders").Week0Item.Value.Should().Be(originalPart1Details.RecommendedOrders);
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
            part1Resource.Links.Should().Contain(
                a => a.Rel == "part-supplier" && a.Href == $"/purchasing/part-suppliers/record?partId={this.MrHeader1.PartId}&supplierId={this.MrHeader1.PreferredSupplierId}");
        }

        [Test]
        public void ShouldSetTagDetailsForFirstPart()
        {
            var part1Result = this.result.Results.First(a => a.PartNumber == "P1");
            var part1Details1Resources = part1Result.Details.Where(a => a.Segment == 0).ToList();
            var originalPart1Details = this.MrHeader1.MrDetails.First(a => a.Segment == 0);
            part1Details1Resources.First(a => a.Title == "Ending").Week3Item.Tag.Should().Be("redBoxOutline");
            part1Details1Resources.First(a => a.Title == "Stock").Week3Item.Tag.Should().Be("redBoxOutline");
        }
    }
}
