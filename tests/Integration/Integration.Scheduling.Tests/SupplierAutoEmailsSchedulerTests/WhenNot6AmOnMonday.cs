﻿namespace Linn.Purchasing.Integration.Scheduling.Tests.SupplierAutoEmailsSchedulerTests
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
        [OneTimeSetUp]
        public void SetUp()
        {
            // this.EmailMonthlyForecastMessageDispatcher.ClearReceivedCalls();
            // this.EmailOrderBookMessageDispatcher.ClearReceivedCalls();
            //
            // this.Sut = new SupplierAutoEmailsScheduler(
            //     this.EmailOrderBookMessageDispatcher,
            //     this.EmailMonthlyForecastMessageDispatcher,
            //     () => new DateTime(2022, 9, 5, 7, 0, 0),
            //     this.Log,
            //     this.ServiceProvider);
            // await this.Sut.StartAsync(CancellationToken.None);
            // await Task.Delay(TimeSpan.FromSeconds(1));
            // await this.Sut.StopAsync(CancellationToken.None);
        }
        
        [Test]
        public Task ShouldNotDispatchOrderBookMessages()
        {
            // this.EmailOrderBookMessageDispatcher
            // .DidNotReceive().Dispatch(Arg.Any<EmailOrderBookMessageResource>());
            return Task.CompletedTask;
        }
        
        [Test]
        public Task ShouldNotDispatchForecastMessages()
        {
            // this.EmailMonthlyForecastMessageDispatcher
            //     .DidNotReceive().Dispatch(Arg.Any<EmailMonthlyForecastReportMessageResource>());
            return Task.CompletedTask;
        }
    }
}
