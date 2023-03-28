namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeStatusReportServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenGettingChangeRequestPhaseInWeeksReport : ContextBase
    {
        private ResultsModel results;

        [SetUp]
        public void SetUp()
        {
            var data = new List<ChangeRequestPhaseInWeeksView>
                           {
                               new ChangeRequestPhaseInWeeksView()
                                   {
                                       PhaseInWeek = "21/03",
                                       DocumentNumber = 123456,
                                       DisplayName = "LP RADIO UPGRADE",
                                       OldPartNumber = "CNTRYBENZO",
                                       OldPartStock = 80,
                                       NewPartNumber = "CNTRYBENZO/1",
                                       NewPartStock = 100,
                                       LinnWeekNumber = 112233,
                                       DescriptionOfChange = "UBFMB",
                                       Notes = "WPS to PRK RD",
                                       DateAccepted = new DateTime(2023, 1, 1, 6, 0, 0),
                                       LinnEndDate = new DateTime(2023, 12, 31, 6, 0, 0),
                                       CountOfBomChanges = 4,
                                   },
                               new ChangeRequestPhaseInWeeksView()
                                   {
                                       PhaseInWeek = "28/03",
                                       DocumentNumber = 6565656,
                                       DisplayName = "PART",
                                       OldPartNumber = "DSM",
                                       OldPartStock = 75,
                                       NewPartNumber = "DSM/13",
                                       NewPartStock = 150,
                                       LinnWeekNumber = 112233,
                                       DescriptionOfChange = "UBFMB",
                                       Notes = "WPS to PRK RD",
                                       DateAccepted = new DateTime(2023, 1, 1, 6, 0, 0),
                                       LinnEndDate = new DateTime(2023, 12, 31, 6, 0, 0),
                                       CountOfBomChanges = 4,
                                   }
                           };

            this.ChangeRequestPhaseInWeeksView.FindAll().Returns(data.AsQueryable());

            this.results = this.Sut.GetCurrentPhaseInWeeksReport(100);
        }

        [Test]
        public void ShouldReturnReport()
        {
            this.results.Rows.Count().Should().Be(2);
            this.results.GetGridTextValue(0, 0).Should().Be("21/03");
            this.results.GetGridTextValue(0, 1).Should().Be("123456");
            this.results.GetGridTextValue(1, 0).Should().Be("28/03");
            this.results.GetGridTextValue(1, 1).Should().Be("6565656");
        }
    }
}
