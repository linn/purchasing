namespace Linn.Purchasing.Integration.Scheduling.Tests.SupplierAutoEmailsSchedulerTests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Linn.Purchasing.Resources.Messages;
    using Linn.Purchasing.Scheduling.Host.Jobs;

    using NSubstitute;

    using NUnit.Framework;

    public class When6AmOnMondayButSupplierHasNoOutstandingOrders : ContextBase
    {
        [OneTimeSetUp]
        public async Task SetUp()
        {
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
        public Task ShouldNotDispatchMessages()
        {
            this.EmailOrderBookMessageDispatcher.DidNotReceive().Dispatch(Arg.Any<EmailOrderBookMessageResource>());

            return Task.CompletedTask;
        }
    }
}
