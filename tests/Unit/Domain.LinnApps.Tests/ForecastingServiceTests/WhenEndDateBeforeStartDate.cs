namespace Linn.Purchasing.Domain.LinnApps.Tests.ForecastingServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentAssertions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenEndDateBeforeStartDate : ContextBase
    {
        private ProcessResult result;

        [SetUp]
        public void SetUp()
        {
            this.MockAuthService.HasPermissionFor(
                AuthorisedAction.ForecastingApplyPercentageChange,
                Arg.Any<IEnumerable<string>>()).Returns(true);
            this.MockLedgerPeriodRepository.FindBy(Arg.Any<Expression<Func<LedgerPeriod, bool>>>()).Returns(
                new LedgerPeriod { PeriodNumber = 10 },
                new LedgerPeriod { PeriodNumber = 9 });

            this.result = this.Sut.ApplyPercentageChange(10, 2, 2001, 1, 2001, new List<string>());
        }

        [Test]
        public void ShouldReturnFailResult()
        {
            this.result.Success.Should().BeFalse();
            this.result.Message.Should().Be("End month cannot be before start month.");
        }
    }
}
