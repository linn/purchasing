namespace Linn.Purchasing.Integration.Scheduling.Tests.SupplierAutoEmailsSchedulerTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Resources.Messages;
    using Linn.Purchasing.Scheduling.Host.Jobs;

    using NSubstitute;

    using NUnit.Framework;

    public class When6AmOnMonday : ContextBase
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            // this.OutstandingPosRepository.FilterBy(Arg.Any<Expression<Func<MrPurchaseOrderDetail, bool>>>())
            //     .Returns(new List<MrPurchaseOrderDetail>
            //                  {
            //                      new MrPurchaseOrderDetail()
            //                  }.AsQueryable());
            //
            // this.Sut = new SupplierAutoEmailsScheduler(
            //     this.EmailOrderBookMessageDispatcher,
            //     this.EmailMonthlyForecastMessageDispatcher,
            //     () => new DateTime(2022, 9, 5, 6, 0, 0),
            //     this.Log,
            //     this.ServiceProvider);
            // await this.Sut.StartAsync(CancellationToken.None);
            // await Task.Delay(TimeSpan.FromMilliseconds(1000));
            // await this.Sut.StopAsync(CancellationToken.None);
        }
        
        [Test]
        public Task ShouldDispatchOrderBookMessages()
        {
            // this.EmailOrderBookMessageDispatcher.Received().Dispatch(Arg.Is<EmailOrderBookMessageResource>(x => 
            //     x.ForSupplier == 1
            //     && x.ToAddress == "orderbookperson@gmail.com"));
            return Task.CompletedTask;
        }
        
        [Test]
        public Task ShouldDispatchWeeklyForecastMessages()
        {
            // this.EmailMonthlyForecastMessageDispatcher
            //     .Received().Dispatch(Arg.Is<EmailMonthlyForecastReportMessageResource>(x =>
            //     x.ForSupplier == 2
            //     && x.ToAddress == "weeklyforecastperson@gmail.com"));
            return Task.CompletedTask;
        }
    }
}
