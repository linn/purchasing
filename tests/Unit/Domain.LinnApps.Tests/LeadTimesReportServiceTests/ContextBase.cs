namespace Linn.Purchasing.Domain.LinnApps.Tests.LeadTimesReportServiceTests
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
        protected IQueryRepository<SuppliersLeadTimesEntry> Repository { get; private set; }

        protected IReportingHelper ReportingHelper { get; private set; }

        protected ILeadTimesReportService Sut { get; private set; }

        protected IEnumerable<SuppliersLeadTimesEntry> Data { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.Data = new List<SuppliersLeadTimesEntry>
                            {
                                new SuppliersLeadTimesEntry
                                    {
                                        PartNumber = "RES 311",
                                        LeadTimeWeeks = 42,
                                        SupplierId = 999
                                    },
                                new SuppliersLeadTimesEntry
                                    {
                                        PartNumber = "RES 312",
                                        LeadTimeWeeks = 45,
                                        SupplierId = 999
                                    },
                                new SuppliersLeadTimesEntry
                                    {
                                        PartNumber = "RES 313",
                                        LeadTimeWeeks = 62,
                                        SupplierId = 999
                                    },
                                new SuppliersLeadTimesEntry
                                    {
                                        PartNumber = "RES 314",
                                        LeadTimeWeeks = 69,
                                        SupplierId = 999
                                    },
                            };

            this.Repository = Substitute.For<IQueryRepository<SuppliersLeadTimesEntry>>();
            this.Repository.FilterBy(Arg.Any<Expression<Func<SuppliersLeadTimesEntry, bool>>>())
                .Returns(this.Data.AsQueryable());
            this.ReportingHelper = new ReportingHelper();
            this.Sut = new LeadTimesReportService(this.Repository, this.ReportingHelper);
        }
    }
}
