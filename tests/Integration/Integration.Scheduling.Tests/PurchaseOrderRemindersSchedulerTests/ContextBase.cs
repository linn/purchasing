namespace Linn.Purchasing.Integration.Scheduling.Tests.PurchaseOrderRemindersSchedulerTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions.Extensions;

    using Linn.Common.Logging;
    using Linn.Common.Messaging.RabbitMQ.Dispatchers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Resources.Messages;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IMessageDispatcher<EmailPurchaseOrderReminderMessageResource> Dispatcher
        {
            get;
            private set;
        }

        protected IServiceProvider ServiceProvider { get; private set; }

        protected BackgroundService Sut { get; set; }

        protected IPurchaseOrderDeliveryRepository Repository { get; private set; }

        protected ILog Log { get; set; }

        [OneTimeSetUp]
        public void SetUpContext()
        {
            this.Log = Substitute.For<ILog>();
            this.Repository = Substitute.For<IPurchaseOrderDeliveryRepository>();
            this.Dispatcher =
                Substitute.For<IMessageDispatcher<EmailPurchaseOrderReminderMessageResource>>();
            this.Repository.FindAll().Returns(new List<PurchaseOrderDelivery>
                                                  {
                                                      new PurchaseOrderDelivery
                                                          {
                                                              OrderNumber = 1,
                                                              OrderLine = 1,
                                                              DeliverySeq = 1,
                                                              QuantityOutstanding = 1,
                                                              DateAdvised = 10.March(2023),
                                                              PurchaseOrderDetail = new PurchaseOrderDetail
                                                                  {
                                                                      PurchaseOrder = new PurchaseOrder
                                                                          {
                                                                              Supplier = new Supplier
                                                                                  {
                                                                                      ReceivesOrderReminders = "Y"
                                                                                  }
                                                                          }
                                                                  }
                                                          },
                                                      new PurchaseOrderDelivery
                                                          {
                                                              OrderNumber = 2,
                                                              OrderLine = 2,
                                                              DeliverySeq = 2,
                                                              QuantityOutstanding = 1,
                                                              DateAdvised = 15.March(2023),
                                                              PurchaseOrderDetail = new PurchaseOrderDetail
                                                                  {
                                                                      PurchaseOrder = new PurchaseOrder
                                                                          {
                                                                              Supplier = new Supplier
                                                                                  {
                                                                                      ReceivesOrderReminders = "Y"
                                                                                  }
                                                                          }
                                                                  }
                                                          },
                                                      new PurchaseOrderDelivery
                                                          {
                                                              OrderNumber = 2,
                                                              OrderLine = 2,
                                                              DeliverySeq = 2,
                                                              QuantityOutstanding = 1,
                                                              DateAdvised = 10.March(2023),
                                                              PurchaseOrderDetail = new PurchaseOrderDetail
                                                                  {
                                                                      PurchaseOrder = new PurchaseOrder
                                                                          {
                                                                              Supplier = new Supplier
                                                                                  {
                                                                                      ReceivesOrderReminders = "N"
                                                                                  }
                                                                          }
                                                                  }
                                                          }
                                                  }.AsQueryable());
            IServiceCollection services = new ServiceCollection();

            services.AddHostedService<BackgroundService>(_ => this.Sut);
            services.AddTransient<IPurchaseOrderDeliveryRepository>(_ => this.Repository);
            this.ServiceProvider = services.BuildServiceProvider();
        }
    }
}
