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

    public class WhenGettingChangeStatusReport : ContextBase
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
            this.results = this.Sut.GetChangeStatusReport(100);
        }

        [Test]
        public void ShouldGetData()
        {
            this.ChangeRequestsRepository.Received().FindAll();
        }

        [Test]
        public void ShouldReturnData()
        {
            this.results.Rows.Count().Should().Be(4);
            this.results.GetGridTextValue(0, 1).Should().Be("ACCEPT ACCEPTED CHANGES");
            this.results.GetGridTextValue(1, 1).Should().Be("PROPOS PROPOSED CHANGES");
            this.results.GetGridTextValue(2, 1).Should().Be("TOTAL OUTSTANDING CHANGES");
            this.results.GetGridTextValue(3, 1).Should().Be("CHANGES WITH CURRENT PHASE IN WEEK");
        }
    }
}
