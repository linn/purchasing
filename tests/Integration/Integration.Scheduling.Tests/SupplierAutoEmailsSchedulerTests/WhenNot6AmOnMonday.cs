namespace Linn.Purchasing.Integration.Scheduling.Tests.SupplierAutoEmailsSchedulerTests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Linn.Purchasing.Resources.Messages;
    using Linn.Purchasing.Scheduling.Host.Jobs;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenNot6AmOnMonday : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new SupplierAutoEmailsScheduler(
                this.EmailOrderBookMessageDispatcher,
                this.EmailMonthlyForecastMessageDispatcher,
                () => new DateTime(2022, 9, 5, 7, 0, 0),
                this.Log,
                this.ServiceProvider);
        }

        [Test]
        public async Task ShouldNotSendOrderBooks()
        {
            await Sut.StartAsync(CancellationToken.None);
            await Task.Delay(TimeSpan.FromSeconds(1));
            this.EmailOrderBookMessageDispatcher
                .DidNotReceive().Dispatch(Arg.Any<EmailOrderBookMessageResource>());
        }

        [Test]
        public async Task ShouldNotSendWeeklyForecasts()
        {
            await Sut.StartAsync(CancellationToken.None);
            await Task.Delay(TimeSpan.FromSeconds(1));
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
