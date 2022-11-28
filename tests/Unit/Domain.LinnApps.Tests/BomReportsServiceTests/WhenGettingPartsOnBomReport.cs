namespace Linn.Purchasing.Domain.LinnApps.Tests.BomReportsServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingPartsOnBomReport : ContextBase
    {
        private ResultsModel results;

        private string bomName;

        [SetUp]
        public void SetUp()
        {
            this.bomName = "SK HUB";
            this.ComponentSummaryRepository.FilterBy(Arg.Any<Expression<Func<BoardComponentSummary, bool>>>()).Returns(
                new List<BoardComponentSummary>
                    {
                        new BoardComponentSummary { Cref = "U001" },
                        new BoardComponentSummary { Cref = "U002" },
                        new BoardComponentSummary { Cref = "U003" }
                    }.AsQueryable());
            var data = new List<BomDetail>
                           {
                               new BomDetail
                                   {
                                       Part = new Part { PartNumber = "CAP 001" },
                                       PartNumber = "CAP 001",
                                       BomPartNumber = this.bomName
                                   }
                           };

            this.BomDetailRepository.FindAll().Returns(data.AsQueryable());

            this.results = this.Sut.GetPartsOnBomReport(this.bomName);
        }

        [Test]
        public void ShouldReturnReport()
        {
            this.results.Rows.Count().Should().Be(1);
            this.results.ReportTitle.DisplayValue.Should().Be(this.bomName);
        }

        [Test]
        public void ShouldAggregateCrefs()
        {
            this.results.GetGridTextValue(0, 9).Should().Be("U001, U002, U003, ");
        }
    }
}
