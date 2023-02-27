namespace Linn.Purchasing.Domain.LinnApps.Tests.BomReportsServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingBomDifferencesReport : ContextBase
    {
        private ResultsModel result;

        [SetUp]
        public void Setup()
        {
            var firstBomDetails = new List<BomDetailViewEntry>
                                      {
                                          new BomDetailViewEntry { PartNumber = "C1", Qty = 1, Part = new Part { ExpectedUnitPrice = 12.3m } },
                                          new BomDetailViewEntry { PartNumber = "C2", Qty = 2, Part = new Part { ExpectedUnitPrice = 1.0m } },
                                          new BomDetailViewEntry { PartNumber = "A3", Qty = 2, Part = new Part { ExpectedUnitPrice = 12m } },
                                          new BomDetailViewEntry { PartNumber = "P4", Qty = 1, Part = new Part { ExpectedUnitPrice = 0.5m } },
                                          new BomDetailViewEntry { PartNumber = "C5", Qty = 12, Part = new Part { ExpectedUnitPrice = 0.1m } }
                                      }; 


            var secondBomDetails = new List<BomDetailViewEntry>
                                       {
                                           new BomDetailViewEntry { PartNumber = "A3", Qty = 2, Part = new Part { ExpectedUnitPrice = 12m } },
                                           new BomDetailViewEntry { PartNumber = "P4", Qty = 5, Part = new Part { ExpectedUnitPrice = 0.5m } },
                                           new BomDetailViewEntry { PartNumber = "C5", Qty = 2, Part = new Part { ExpectedUnitPrice = 1m } },
                                           new BomDetailViewEntry { PartNumber = "C6", Qty = 20, Part = new Part { ExpectedUnitPrice = 0.1m } },
                                       };
            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>()).Returns(
                new Part { MaterialPrice = 1001.1m},
                new Part { MaterialPrice = 2002.2m});
            this.BomDetailViewRepository.FilterBy(Arg.Any<Expression<Func<BomDetailViewEntry, bool>>>())
                .Returns(firstBomDetails.AsQueryable(), secondBomDetails.AsQueryable());

            this.result = this.Sut.GetBomDifferencesReport("BOM1", "BOM2");
        }

        [Test]
        public void ShouldReturnCorrectNumberOfRows()
        {
            this.result.Rows.Count().Should().Be(7);
        }

        [Test]
        public void ShouldIncludePartsExclusivelyOnBom1()
        {
            this.result.GetGridTextValue(0, 0).Should().Be("C1");
            this.result.GetGridValue(0, 6).Should().Be(-12.3m);
            this.result.GetGridTextValue(1, 0).Should().Be("C2");
        }

        [Test]
        public void ShouldIncludePartsOnBothWhereQtyIsDifferentAndCalculateCostDiffs()
        {
            this.result.GetGridTextValue(2, 0).Should().Be("P4");
            this.result.GetGridValue(2, 6).Should().Be(2m);
            this.result.GetGridTextValue(3, 0).Should().Be("C5");
            this.result.GetGridValue(3, 6).Should().Be(0.8m);
        }

        [Test]
        public void ShouldIncludePartExclusivelyOnBom2()
        {
            this.result.GetGridTextValue(4, 3).Should().Be("C6");
            this.result.GetGridValue(4, 6).Should().Be(-2m);
        }

        [Test]
        public void ShouldIncludeTotals()
        {
            this.result.GetGridValue(5, 6).Should().Be(-13.5m); 
            this.result.GetGridTextValue(6, 2).Should().Be("1001.1");
            this.result.GetGridTextValue(6, 5).Should().Be("2002.2");
        }
    }
}
