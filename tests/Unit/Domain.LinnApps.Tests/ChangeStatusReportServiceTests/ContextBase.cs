using Linn.Purchasing.Domain.LinnApps.Boms;

namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeStatusReportServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IChangeStatusReportService Sut { get; private set; }

        protected IQueryRepository<ChangeRequest> ChangeRequestRepository { get; private set; }

        protected IEnumerable<ChangeRequest> Data { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.ChangeRequestRepository = Substitute.For<IQueryRepository<ChangeRequest>>();

            this.Data = new List<ChangeRequest>
                            {
                                new ChangeRequest
                                {
                                    DocumentNumber = 1,
                                    ChangeState = "ACCEPT",
                                    DateEntered = new DateTime(2023, 3, 4, 6, 0, 0),
                                    EnteredBy = new Employee()
                                },
                                new ChangeRequest
                                {
                                    DocumentNumber = 2,
                                    ChangeState = "PROPOS",
                                    DateEntered = new DateTime(2023, 3, 4, 6, 0, 0),
                                    EnteredBy = new Employee()
                                },
                                new ChangeRequest
                                {
                                    DocumentNumber = 3,
                                    ChangeState = "ACCEPT",
                                    DateEntered = new DateTime(2023, 3, 4, 6, 0, 0),
                                    EnteredBy = new Employee()
                                },
                                new ChangeRequest
                                {
                                    DocumentNumber = 4,
                                    ChangeState = "PROPOS",
                                    DateEntered = new DateTime(2023, 3, 4, 6, 0, 0),
                                    EnteredBy = new Employee()
                                },
                                new ChangeRequest
                                {
                                    DocumentNumber = 5,
                                    ChangeState = "ACCEPT",
                                    DateEntered = new DateTime(2023, 3, 4, 6, 0, 0),
                                    EnteredBy = new Employee()
                                }
                            };
            this.ChangeRequestRepository = Substitute.For<IQueryRepository<ChangeRequest>>();
            this.ChangeRequestRepository.FilterBy(Arg.Any<Expression<Func<ChangeRequest, bool>>>())
                .Returns(this.Data.AsQueryable());

            this.Sut = new ChangeStatusReportService(this.ChangeRequestRepository, new ReportingHelper());
        }
    }
}
