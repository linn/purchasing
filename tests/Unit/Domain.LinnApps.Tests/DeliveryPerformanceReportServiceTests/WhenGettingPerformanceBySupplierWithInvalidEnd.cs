namespace Linn.Purchasing.Domain.LinnApps.Tests.DeliveryPerformanceReportServiceTests
{
    using System;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingPerformanceBySupplierWithInvalidEnd : ContextBase
    {
        private readonly int startPeriod = 3;

        private readonly int endPeriod = 4;

        private readonly int? supplierId = null;

        private readonly string vendorManager = null;

        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.LedgerPeriodRepository.FindById(this.startPeriod).Returns((LedgerPeriod)null);
            this.action = () => this.Sut.GetDeliveryPerformanceBySupplier(this.startPeriod, this.endPeriod, this.supplierId, this.vendorManager);
        }

        [Test]
        public void ShouldThrowException()
        {
            this.action.Should().Throw<InvalidOptionException>();
        }
    }
}
