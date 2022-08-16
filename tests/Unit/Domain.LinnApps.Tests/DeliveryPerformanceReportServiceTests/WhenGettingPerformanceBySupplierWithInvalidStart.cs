namespace Linn.Purchasing.Domain.LinnApps.Tests.DeliveryPerformanceReportServiceTests
{
    using System;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingPerformanceBySupplierWithInvalidStart : ContextBase
    {
        private readonly int startPeriod = 3;

        private readonly int endPeriod = 4;

        private readonly int? supplierId = null;

        private readonly string vendorManager = null;

        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.LedgerPeriodRepository.FindById(this.startPeriod).Returns(new LedgerPeriod { MonthName = "Jan2028" });
            this.LedgerPeriodRepository.FindById(this.endPeriod).Returns((LedgerPeriod)null);
            this.action = () => this.Sut.GetDeliveryPerformanceBySupplier(this.startPeriod, this.endPeriod, this.supplierId, this.vendorManager);
        }

        [Test]
        public void ShouldThrowException()
        {
            this.action.Should().Throw<InvalidOptionException>();
        }
    }
}
