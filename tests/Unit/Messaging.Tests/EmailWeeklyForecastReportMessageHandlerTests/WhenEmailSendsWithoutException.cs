namespace Linn.Purchasing.Messaging.Tests.EmailWeeklyForecastReportMessageHandlerTests
{
    using FluentAssertions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenEmailSendsWithoutException : ContextBase
    {
        private bool result;

        [SetUp]
        public void SetUp()
        {
            this.result = this.Sut.Handle(this.Message);
        }

        [Test]
        public void ShouldReturnTrue()
        {
            this.result.Should().BeTrue();
        }

        [Test]
        public void ShouldCallMailer()
        {
            this.Mailer.Received()
                .SendWeeklyForecastEmail(this.Resource.ToAddress, this.Resource.ForSupplier, Arg.Any<string>());
        }
    }
}
