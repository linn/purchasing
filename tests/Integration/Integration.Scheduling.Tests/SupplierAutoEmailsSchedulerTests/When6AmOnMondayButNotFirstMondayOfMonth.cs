namespace Linn.Purchasing.Integration.Scheduling.Tests.SupplierAutoEmailsSchedulerTests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Linn.Purchasing.Resources.Messages;
    using Linn.Purchasing.Scheduling.Host.Jobs;

    using NSubstitute;

    using NUnit.Framework;

    public class When6AmOnMondayButNotFirstMondayOfMonth : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new SupplierAutoEmailsScheduler(
                this.EmailOrderBookMessageDispatcher,
                this.EmailMonthlyForecastMessageDispatcher,
                () => new DateTime(2022, 9, 12, 6, 0, 0),
                this.ServiceProvider);
        }

        [Test]
        public async Task ShouldNotSendMonthlyForecasts()
        {
            await Sut.StartAsync(CancellationToken.None);
            await Task.Delay(TimeSpan.FromMilliseconds(100));
            this.EmailMonthlyForecastMessageDispatcher
                .DidNotReceive().Dispatch(Arg.Any<EmailMonthlyForecastReportMessageResource>());
        }

        [TearDown]
        public void Stop()
        {
            this.Sut.Dispose();
            this.Repository.ClearReceivedCalls();
        }
    }
}

