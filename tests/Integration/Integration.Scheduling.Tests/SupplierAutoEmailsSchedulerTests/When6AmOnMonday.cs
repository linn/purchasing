namespace Linn.Purchasing.Integration.Scheduling.Tests.SupplierAutoEmailsSchedulerTests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Linn.Purchasing.Resources.Messages;
    using Linn.Purchasing.Scheduling.Host.Jobs;

    using NSubstitute;

    using NUnit.Framework;

    public class When6AmOnMonday : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new SupplierAutoEmailsScheduler(
                this.EmailOrderBookMessageDispatcher,
                this.EmailMonthlyForecastMessageDispatcher,
                () => new DateTime(2022, 9, 5, 6, 0, 0),
                this.ServiceProvider);
        }

        [Test]
        public async Task ShouldSendOrderBooks()
        {
            await Sut.StartAsync(CancellationToken.None);
            await Task.Delay(TimeSpan.FromSeconds(1));
            this.EmailOrderBookMessageDispatcher.Received().Dispatch(Arg.Is<EmailOrderBookMessageResource>(x => 
                x.ForSupplier == 1
                && x.ToAddress == "orderbookperson@gmail.com"));
        }

        [Test]
        public async Task ShouldSendweeklyForecasts()
        {
            await Sut.StartAsync(CancellationToken.None);
            await Task.Delay(TimeSpan.FromSeconds(1));
            this.EmailMonthlyForecastMessageDispatcher
                .Received().Dispatch(Arg.Is<EmailMonthlyForecastReportMessageResource>(x =>
                x.ForSupplier == 2
                && x.ToAddress == "weeklyforecastperson@gmail.com"));
        }

        [TearDown]
        public void Stop()
        {
            this.Sut.Dispose();
            this.Repository.ClearReceivedCalls();
        }
    }
}
