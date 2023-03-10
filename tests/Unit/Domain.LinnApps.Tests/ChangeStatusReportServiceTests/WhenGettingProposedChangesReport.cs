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

    public class WhenGettingProposedChangesReport : ContextBase
    {
        private ResultsModel results;

        [SetUp]
        public void SetUp()
        {
            var data = new List<ChangeRequest>
                           {
                               new ChangeRequest
                                   {
                                       DocumentNumber = 1,
                                       ChangeState = "ACCEPT",
                                       DateEntered = new DateTime(2023, 3, 4, 6, 0, 0),
                                   },
                               new ChangeRequest
                                   {
                                       DocumentNumber = 2,
                                       ChangeState = "PROPOS",
                                       DateEntered = new DateTime(2023, 3, 4, 6, 0, 0),
                                   },
                               new ChangeRequest
                                   {
                                       DocumentNumber = 3,
                                       ChangeState = "ACCEPT",
                                       DateEntered = new DateTime(2023, 3, 4, 6, 0, 0),
                                   },
                               new ChangeRequest
                                   {
                                       DocumentNumber = 4,
                                       ChangeState = "PROPOS",
                                       DateEntered = new DateTime(2023, 3, 4, 6, 0, 0),
                                   },
                               new ChangeRequest
                                   {
                                       DocumentNumber = 5,
                                       ChangeState = "ACCEPT",
                                       DateEntered = new DateTime(2023, 3, 4, 6, 0, 0),
                                   }
                           };

            this.ChangeRequestsRepository.FindAll().Returns(data.AsQueryable());

            this.results = this.Sut.GetProposedChangesReport(100);
        }

        [Test]
        public void ShouldReturnReport()
        {
            this.results.Rows.Count().Should().Be(2);
            this.results.GetGridTextValue(0, 0).Should().Be("2");
            this.results.GetGridTextValue(0, 1).Should().Be("PROPOS");
            this.results.GetGridTextValue(1, 0).Should().Be("4");
            this.results.GetGridTextValue(1, 1).Should().Be("PROPOS");
        }
    }
}
