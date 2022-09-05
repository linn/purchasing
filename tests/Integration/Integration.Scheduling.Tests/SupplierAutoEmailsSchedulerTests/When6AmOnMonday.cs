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
        [OneTimeSetUp]
        public async Task SetUp()
        {
            this.EmailMonthlyForecastMessageDispatcher.ClearReceivedCalls();
            this.EmailOrderBookMessageDispatcher.ClearReceivedCalls();
            
            this.Sut = new SupplierAutoEmailsScheduler(
                this.EmailOrderBookMessageDispatcher,
                this.EmailMonthlyForecastMessageDispatcher,
                () => new DateTime(2022, 9, 5, 6, 0, 0),
                this.Log,
                this.ServiceProvider);
            await this.Sut.StartAsync(CancellationToken.None);
            await Task.Delay(TimeSpan.FromMilliseconds(100));
            await this.Sut.StopAsync(CancellationToken.None);
        }
        
        [Test]
        public void ShouldSendOrderBooks()
        {
            this.EmailOrderBookMessageDispatcher.Received().Dispatch(Arg.Is<EmailOrderBookMessageResource>(x => 
                x.ForSupplier == 1
                && x.ToAddress == "orderbookperson@gmail.com"));
        }
        
        [Test]
        public void ShouldSendWeeklyForecasts()
        {
            this.EmailMonthlyForecastMessageDispatcher
                .Received().Dispatch(Arg.Is<EmailMonthlyForecastReportMessageResource>(x =>
                x.ForSupplier == 2
                && x.ToAddress == "weeklyforecastperson@gmail.com"));
        }
    }
}
