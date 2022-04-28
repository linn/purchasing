namespace Linn.Purchasing.Domain.LinnApps.Tests.ForecastingServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;
    using FluentAssertions.Extensions;

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
                                                   new LinnWeek { WeekNumber = 1, StartsOn = 30.June(2001) },
                                                   new LinnWeek { WeekNumber = 2, StartsOn = 07.July(2021) },
                                                   new LinnWeek { WeekNumber = 3, StartsOn = 14.July(2021) },
                                                   new LinnWeek { WeekNumber = 4, StartsOn = 21.July(2021) },
                                                   new LinnWeek { WeekNumber = 5, StartsOn = 28.July(2021) }
                                               };

            this.weeksStartingInEndMonth = new List<LinnWeek>
                                               {
                                                   new LinnWeek { WeekNumber = 6, StartsOn = 4.August(2001) },
                                                   new LinnWeek { WeekNumber = 7, StartsOn = 11.August(2001) },
                                                   new LinnWeek { WeekNumber = 8, StartsOn = 18.August(2001) },
                                                   new LinnWeek { WeekNumber = 8, StartsOn = 25.August(2001) }
                                               };

            this.MockAuthService.HasPermissionFor(
                AuthorisedAction.ForecastingApplyPercentageChange,
                Arg.Any<IEnumerable<string>>()).Returns(true);
            this.MockLedgerPeriodRepository.FindBy(Arg.Any<Expression<Func<LedgerPeriod, bool>>>()).Returns(
                new LedgerPeriod { PeriodNumber = 9, MonthName = "JUL2001" },
                new LedgerPeriod { PeriodNumber = 10, MonthName = "AUG2001" });

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
            this.MockForecastingPack.Received().SetAutoForecastChange(10, 1, 8);
        }

        [Test]
        public void ShouldReturnSuccessResult()
        {
            this.result.Success.Should().BeTrue();
            this.result.Message.Should().Be("Process complete");
        }
    }
}
