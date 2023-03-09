namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeStatusReportServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Domain.LinnApps.Reports;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IChangeStatusReportService Sut { get; private set; }

        protected IQueryRepository<ChangeRequest> ChangeRequestRepository { get; private set; }

        protected IQueryRepository<MrHeader> MrHeaderRepository { get; private set; }

        protected IRepository<Employee, int> EmployeeRepository { get; set; }

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
            this.ChangeRequestRepository = Substitute.For<IQueryRepository<ChangeRequest>>();
            this.EmployeeRepository = Substitute.For<IRepository<Employee, int>>();
            this.MrHeaderRepository = Substitute.For<IQueryRepository<MrHeader>>();

            this.ChangeRequestRepository.FilterBy(Arg.Any<Expression<Func<ChangeRequest, bool>>>())
                .Returns(this.Data.AsQueryable());

            this.Sut = new ChangeStatusReportService(this.ChangeRequestRepository, 
                                                     this.EmployeeRepository, 
                                                     this.MrHeaderRepository, 
                                                     new ReportingHelper());
        }
    }
}
