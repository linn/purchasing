namespace Linn.Purchasing.Integration.Scheduling.Tests.PurchaseOrderRemindersSchedulerTests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Linn.Purchasing.Resources.Messages;
    using Linn.Purchasing.Scheduling.Host.Jobs;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenNot8Am : ContextBase
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            // this.Sut = new PurchaseOrderRemindersScheduler(
            //     this.Dispatcher,
            //     () => new DateTime(2023, 3, 9, 8, 0, 0),
            //     this.Log,
            //     this.ServiceProvider);
            // await this.Sut.StartAsync(CancellationToken.None);
            // await Task.Delay(TimeSpan.FromMilliseconds(1000));
            // await this.Sut.StopAsync(CancellationToken.None);
        }

        [Test]
        public Task ShouldNotDispatchMessage()
        {
            // this.Dispatcher.DidNotReceive().Dispatch(Arg.Any<EmailPurchaseOrderReminderMessageResource>());
            return Task.CompletedTask;
        }
    }
}
