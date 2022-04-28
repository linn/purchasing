namespace Linn.Purchasing.Domain.LinnApps.Tests.ForecastingServiceTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenApplyingPercentageChangeAndNotAuthorised : ContextBase
    {
        private ProcessResult result;

        [SetUp]
        public void SetUp()
        {
            this.MockAuthService.HasPermissionFor(
                AuthorisedAction.ForecastingApplyPercentageChange,
                Arg.Any<IEnumerable<string>>()).Returns(false);

            this.result = this.Sut.ApplyPercentageChange(10, 1, 2021, 2, 2021, Arg.Any<IEnumerable<string>>());
        }

        [Test]
        public void ShouldReturnFailResult()
        {
            this.result.Success.Should().BeFalse();
            this.result.Message.Should().Be("You are not authorised to apply forecast changes.");
        }
    }
}
