namespace Linn.Purchasing.Domain.LinnApps.Tests.SupplierAutoEmailsMailerTests
{
    using Linn.Common.Email;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Mailers;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected ISupplierAutoEmailsMailer Sut { get; private set; }

        protected IRepository<Supplier, int> SupplierRepository { get; private set; }

        protected IMrOrderBookReportService ReportService { get; private set; }

        protected IEmailService EmailService { get; private set; }

        protected ISingleRecordRepository<MrMaster> MrMaster { get; private set; }

        protected IForecastOrdersReportService ForecastOrdersReportService { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.SupplierRepository = Substitute.For<IRepository<Supplier, int>>();
            this.ReportService = Substitute.For<IMrOrderBookReportService>();
            this.ForecastOrdersReportService = Substitute.For<IForecastOrdersReportService>();
            this.EmailService = Substitute.For<IEmailService>();
            this.MrMaster = Substitute.For<ISingleRecordRepository<MrMaster>>();
            this.Sut = new SupplierAutoEmailsMailer(
                this.SupplierRepository, 
                this.ReportService, 
                this.EmailService, 
                this.MrMaster, 
                this.ForecastOrdersReportService);
        }
    }
}
