namespace Linn.Purchasing.Integration.Scheduling.Tests.PurchaseOrderRemindersSchedulerTests
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Linn.Purchasing.Resources.Messages;
    using Linn.Purchasing.Scheduling.Host.Jobs;

    using NSubstitute;

    using NUnit.Framework;

    public class When8Am : ContextBase
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            // this.Sut = new PurchaseOrderRemindersScheduler(
            //     this.Dispatcher,
            //     () => new DateTime(2023, 3, 8, 8, 0, 0),
            //     this.Log,
            //     this.ServiceProvider);
            // await this.Sut.StartAsync(CancellationToken.None);
            // await Task.Delay(TimeSpan.FromMilliseconds(1000));
            // await this.Sut.StopAsync(CancellationToken.None);
        }

        [Test]
        public Task ShouldDispatchMessage()
        {
            // // since this supplier has two deliveries with qty outstanding and advised to arrive in two days
            // this.Dispatcher.Received().Dispatch(Arg.Is<EmailPurchaseOrderReminderMessageResource>(x =>
            //     x.Deliveries.Count() == 2 && x.Deliveries.All(d => d.OrderNumber == 1 || d.OrderNumber == 12345)));
            //
            // // since this delivery is not advised to arrive in two days
            // this.Dispatcher.DidNotReceive().Dispatch(
            //     Arg.Is<EmailPurchaseOrderReminderMessageResource>(
            //         x => x.Deliveries.Any(d => d.OrderNumber == 2)));
            //
            // // since this delivery is for a supplier who is set not to receive emails
            // this.Dispatcher.DidNotReceive().Dispatch(
            //     Arg.Is<EmailPurchaseOrderReminderMessageResource>(
            //         x => x.Deliveries.Any(d => d.OrderNumber == 3)));
            //
            // // since this delivery is for not of a MANUAL order
            // this.Dispatcher.DidNotReceive().Dispatch(
            //     Arg.Is<EmailPurchaseOrderReminderMessageResource>(
            //         x => x.Deliveries.Any(d => d.OrderNumber == 4)));
            //
            // // since this delivery already has a reminder sent
            // this.Dispatcher.DidNotReceive().Dispatch(
            //     Arg.Is<EmailPurchaseOrderReminderMessageResource>(
            //         x => x.Deliveries.Any(d => d.OrderNumber == 5)));
            //
            // // since this order is cancelled 
            // this.Dispatcher.DidNotReceive().Dispatch(
            //     Arg.Is<EmailPurchaseOrderReminderMessageResource>(
            //         x => x.Deliveries.Any(d => d.OrderNumber == 789)));

            return Task.CompletedTask;
        }
    }
}
