namespace Linn.Purchasing.Domain.LinnApps.Tests.MrUsedOnReportServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingReport : ContextBase
    {
        private IEnumerable<MrUsedOnRecord> data;

        private ResultsModel result;

        private string partNumber;

        private string partDescription = "THE 426TH RESISTOR";

        [SetUp]
        public void SetUp()
        {
            this.partNumber = "RES 426";

            this.data = new List<MrUsedOnRecord>
                            {
                                new MrUsedOnRecord
                                    {
                                        PartNumber = this.partNumber,
                                        Description = this.partDescription,
                                        JobRef = "AAAAA",
                                        AssemblyUsedOn = "BOARD 1",
                                        AssemblyUsedOnDescription = "SOME BOARD",
                                        QtyUsed = 1,
                                        TCoded = null,
                                        AnnualUsage = 1,
                                    },
                                new MrUsedOnRecord
                                    {
                                        PartNumber = this.partNumber,
                                        Description = this.partDescription,
                                        JobRef = "AAAAA",
                                        AssemblyUsedOn = "BOARD 4",
                                        AssemblyUsedOnDescription = "SOME BOARD",
                                        QtyUsed = 1,
                                        TCoded = null,
                                        AnnualUsage = 4,
                                    },
                                new MrUsedOnRecord
                                    {
                                        PartNumber = this.partNumber,
                                        Description = this.partDescription,
                                        JobRef = "AAAAA",
                                        AssemblyUsedOn = "BOARD 2",
                                        AssemblyUsedOnDescription = "SOME BOARD",
                                        QtyUsed = 2,
                                        TCoded = null,
                                        AnnualUsage = 2,
                                    },
                                new MrUsedOnRecord
                                    {
                                        PartNumber = this.partNumber,
                                        Description = this.partDescription,
                                        JobRef = "AAAAA",
                                        AssemblyUsedOn = "BOARD 3",
                                        AssemblyUsedOnDescription = "SOME BOARD",
                                        QtyUsed = 3,
                                        TCoded = null,
                                        AnnualUsage = 3,
                                    }
                            };

            this.MockMrMasterRecordRepository.GetRecord().Returns(new MrMaster { JobRef = "AAAAA" });
            this.MockRepository.FilterBy(Arg.Any<Expression<Func<MrUsedOnRecord, bool>>>())
                .Returns(this.data.AsQueryable());
            this.result = this.Sut.GetUsedOn(this.partNumber, null);
        }

        [Test]
        public void ShouldReturnReport()
        {
            this.result.Rows.Count().Should().Be(4);
            this.result.ReportTitle.DisplayValue.Should().Be(
                    $"Part: {this.partNumber} - {this.partDescription}");
        }

        [Test]
        public void ShouldOrderByAnnualUsageDesc()
        {
            for (var i = 1; i < this.result.Rows.Count(); i++)
            {
                var prevAnnualUsage = this.result.GetGridValue(i - 1, 4);
                Assert.IsTrue(this.result.GetGridValue(i, 4) < prevAnnualUsage);
            }
        }
    }
}
