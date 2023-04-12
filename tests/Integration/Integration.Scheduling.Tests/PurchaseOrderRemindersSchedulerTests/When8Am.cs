namespace Linn.Purchasing.Integration.Scheduling.Tests.PurchaseOrderRemindersSchedulerTests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Linn.Purchasing.Resources.Messages;
    using Linn.Purchasing.Scheduling.Host.Jobs;

    using NSubstitute;

    using NUnit.Framework;

    public class When8Am : ContextBase
    {
        [OneTimeSetUp]
        public async Task SetUp()
        {
            this.Sut = new PurchaseOrderRemindersScheduler(
                this.Dispatcher,
                () => new DateTime(2023, 3, 8, 11, 55, 0),
                this.Log,
                this.ServiceProvider);
            await this.Sut.StartAsync(CancellationToken.None);
            await Task.Delay(TimeSpan.FromMilliseconds(1000));
            await this.Sut.StopAsync(CancellationToken.None);
        }

        [Test]
        public void ShouldDispatchMessage()
        {
            // since this delivery has qty outstanding and advised to arrive in two days
            this.Dispatcher.Received().Dispatch(Arg.Is<EmailPurchaseOrderReminderMessageResource>(x =>
                x.OrderNumber == 1
                && x.OrderLine == 1
                && x.DeliverySeq == 1));


            // since this delivery is not advised to arrive in two days
            this.Dispatcher.DidNotReceive().Dispatch(
                Arg.Is<EmailPurchaseOrderReminderMessageResource>(
                    x => x.OrderNumber == 2 
                         && x.OrderLine == 2 
                         && x.DeliverySeq == 2));

            // since this delivery is for a supplier who is set not to receive emails
            this.Dispatcher.DidNotReceive().Dispatch(Arg.Is<EmailPurchaseOrderReminderMessageResource>(x =>
                x.OrderNumber == 3
                && x.OrderLine == 3
                && x.DeliverySeq == 1));

            // since this delivery is for not of a MANUAL order
            this.Dispatcher.DidNotReceive().Dispatch(Arg.Is<EmailPurchaseOrderReminderMessageResource>(x =>
                x.OrderNumber == 4
                && x.OrderLine == 4
                && x.DeliverySeq == 1));

            // since this delivery already has a reminder sent
            this.Dispatcher.DidNotReceive().Dispatch(Arg.Is<EmailPurchaseOrderReminderMessageResource>(x =>
                x.OrderNumber == 5
                && x.OrderLine == 5
                && x.DeliverySeq == 1));
        }
    }
}
