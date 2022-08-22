namespace Linn.Purchasing.Messaging.Tests.EmailWeeklyForecastReportMessageHandlerTests
{
    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenMailerFailsToSendEmail : ContextBase
    {
        private bool result;

        [SetUp]
        public void SetUp()
        {
            this.Mailer.When(s => s.SendWeeklyForecastEmail(
                this.Resource.ToAddress, this.Resource.ForSupplier, Arg.Any<string>())).Do(
                _ => throw new SupplierAutoEmailsException("An error occurred!"));
            this.result = this.Sut.Handle(this.Message);
        }

        [Test]
        public void ShouldReturnFalse()
        {
            this.result.Should().BeFalse();
        }
    }
}
