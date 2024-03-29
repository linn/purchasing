﻿namespace Linn.Purchasing.Integration.Scheduling.Tests.SupplierAutoEmailsSchedulerTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Logging;
    using Linn.Common.Messaging.RabbitMQ.Dispatchers;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Resources.Messages;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IMessageDispatcher<EmailOrderBookMessageResource> EmailOrderBookMessageDispatcher 
        { 
            get;
            private set;
        }

        protected IMessageDispatcher<EmailMonthlyForecastReportMessageResource> EmailMonthlyForecastMessageDispatcher
        {
            get;
            private set;
        }

        protected IServiceProvider ServiceProvider { get; private set; }

        protected BackgroundService Sut { get; set; }
        
        protected IRepository<SupplierAutoEmails, int> Repository { get; private set; }

        protected IQueryRepository<MrPurchaseOrderDetail> OutstandingPosRepository { get; private set; }

        protected ISingleRecordRepository<MrMaster> MrMaster { get; private set; }

        protected ILog Log { get; set; }

        [OneTimeSetUp]
        public void SetUpContext()
        {
            this.EmailOrderBookMessageDispatcher =
                Substitute.For<IMessageDispatcher<EmailOrderBookMessageResource>>();
            this.EmailMonthlyForecastMessageDispatcher = Substitute.For<IMessageDispatcher<EmailMonthlyForecastReportMessageResource>>();
            this.Repository = Substitute.For<IRepository<SupplierAutoEmails, int>>();
            this.OutstandingPosRepository = Substitute.For<IQueryRepository<MrPurchaseOrderDetail>>();
            this.MrMaster = Substitute.For<ISingleRecordRepository<MrMaster>>();

            this.MrMaster.GetRecord().Returns(new MrMaster { JobRef = "AABBCC" });
            
            this.Log = Substitute.For<ILog>();
            this.Repository.FindAll().Returns(
                new List<SupplierAutoEmails>
                    {
                        new SupplierAutoEmails
                            {
                                Forecast = "REPORT",
                                ForecastInterval = "WEEKLY",
                                SupplierId = 1,
                                OrderBook = "Y",
                                EmailAddresses = "orderbookperson@gmail.com"
                            },
                        new SupplierAutoEmails
                            {
                                Forecast = "REPORT",
                                ForecastInterval = "WEEKLY",
                                SupplierId = 2,
                                OrderBook = "N",
                                EmailAddresses = "weeklyforecastperson@gmail.com"
                            },
                        new SupplierAutoEmails
                            {
                                Forecast = "REPORT",
                                ForecastInterval = "MONTHLY",
                                SupplierId = 3,
                                OrderBook = "N",
                                EmailAddresses = "monthlyforecastperson@gmail.com"
                            }
                    }.AsQueryable());

            IServiceCollection services = new ServiceCollection();

            services.AddHostedService<BackgroundService>(_ => this.Sut);
            services.AddTransient<IRepository<SupplierAutoEmails, int>>(_ => this.Repository);
            services.AddTransient<IQueryRepository<MrPurchaseOrderDetail>>(_ => this.OutstandingPosRepository);
            services.AddTransient<ISingleRecordRepository<MrMaster>>(_ => this.MrMaster);

            this.ServiceProvider = services.BuildServiceProvider();
        }
    }
}
