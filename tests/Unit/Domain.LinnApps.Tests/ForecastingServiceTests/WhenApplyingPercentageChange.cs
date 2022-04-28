namespace Linn.Purchasing.Domain.LinnApps.Tests.ForecastingServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenApplyingPercentageChange : ContextBase
    {
        private ProcessResult result;

        private IEnumerable<LinnWeek> weeksEndingInStartMonth;

        private IEnumerable<LinnWeek> weeksStartingInEndMonth;

        [SetUp]
        public void SetUp()
        {
            this.weeksEndingInStartMonth = new List<LinnWeek>
                                               {
                                                   new LinnWeek { WeekNumber = 1 },
                                               };

            this.weeksStartingInEndMonth = new List<LinnWeek>
                                               {
                                                   new LinnWeek { WeekNumber = 2 },
                                               };

            this.MockAuthService.HasPermissionFor(
                AuthorisedAction.ForecastingApplyPercentageChange,
                Arg.Any<IEnumerable<string>>()).Returns(true);
            this.MockLedgerPeriodRepository.FindBy(Arg.Any<Expression<Func<LedgerPeriod, bool>>>()).Returns(
                new LedgerPeriod { PeriodNumber = 9 },
                new LedgerPeriod { PeriodNumber = 10 });

            this.MockLinnWeekRepository.FilterBy(Arg.Any<Expression<Func<LinnWeek, bool>>>()).Returns(
                this.weeksEndingInStartMonth.AsQueryable(),
                this.weeksStartingInEndMonth.AsQueryable());

            this.result = this.Sut.ApplyPercentageChange(10, 2, 2001, 1, 2001, new List<string>());
        }

        [Test]
        public void ShouldCallApplyAcrossBoardChangeProcedure()
        {
            this.MockForecastingPack.Received().ApplyAcrossBoardPlanChange(10, 9, 10);
        }

        [Test]
        public void ShouldCallSetAutoForecastChangeProcedureForCorrectWeeks()
        {
            this.MockForecastingPack.Received().SetAutoForecastChange(10, 1, 2);
        }

        [Test]
        public void ShouldReturnSuccessResult()
        {
            this.result.Success.Should().BeTrue();
            this.result.Message.Should().Be("Process complete");
        }
    }
}
