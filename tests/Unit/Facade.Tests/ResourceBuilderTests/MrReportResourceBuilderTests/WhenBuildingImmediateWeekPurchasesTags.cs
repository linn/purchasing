﻿namespace Linn.Purchasing.Facade.Tests.ResourceBuilderTests.MrReportResourceBuilderTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Resources.MaterialRequirements;

    using NUnit.Framework;

    public class WhenBuildingImmediateWeekPurchasesTags : ContextBase
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
                                                                 LinnWeekNumber = -998,
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
                                                                 RecommendedStock = 250,
                                                                 QuantityAvailableAtSupplier = 20
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
                                     HasPurchaseOrders = "Y",
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
                                                                 PartNumber = "P2",
                                                                 LinnWeekNumber = -998,
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
                                                                 RecommendedStock = 250
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
        public void ShouldFlagImmediateAvailablePurchases()
        {
            var part1Result = this.result.Results.First(a => a.PartNumber == "P1");
            var part1Details1Resources = part1Result.Details.Where(a => a.Segment == 0).ToList();
            part1Details1Resources.First(a => a.Title == "Purchases").ImmediateItem.Tag.Should().Be("greenBoxOutline");
        }

        [Test]
        public void ShouldFlagImmediateUnavailablePurchases()
        {
            var part2Result = this.result.Results.First(a => a.PartNumber == "P2");
            var part2Details1Resources = part2Result.Details.Where(a => a.Segment == 0).ToList();
            part2Details1Resources.First(a => a.Title == "Purchases").ImmediateItem.Tag.Should().Be("blueBoxOutline");
        }
    }
}
