namespace Linn.Purchasing.Domain.LinnApps.Tests.WhatsDueInReportServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IPurchaseOrderDeliveryRepository Repository { get; private set; }

        protected IReportingHelper ReportingHelper { get; private set; }

        protected IWhatsDueInReportService Sut { get; private set; }

        protected IEnumerable<PurchaseOrderDelivery> Data { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.Data = new List<PurchaseOrderDelivery>
                            {
                                new PurchaseOrderDelivery
                                    {
                                        OrderNumber = 1,
                                        OrderLine = 1,
                                        DeliverySeq = 1,
                                        DateAdvised = new DateTime(1995, 03, 28),
                                        DateRequested = new DateTime(1995, 12, 25),
                                        BaseOurUnitPrice = 1,
                                        QuantityOutstanding = 1,
                                        PurchaseOrderDetail = new PurchaseOrderDetail
                                                                  {
                                                                      Part = new Part { PartNumber = "PART" },
                                                                      PurchaseOrder = new PurchaseOrder
                                                                          {
                                                                              OrderNumber = 111,
                                                                              SupplierId = 2,
                                                                              Supplier = new Supplier
                                                                                  {
                                                                                      Name = "SUPPLIER 2",
                                                                                      VendorManager = new VendorManager
                                                                                          {
                                                                                              Id = "A"
                                                                                          }
                                                                                  }
                                                                          }
                                                                  }
                                    },
                                new PurchaseOrderDelivery
                                    {
                                        OrderNumber = 2,
                                        OrderLine = 2,
                                        DeliverySeq = 1,
                                        DateAdvised = null,
                                        DateRequested = new DateTime(1994, 12, 25),
                                        BaseOurUnitPrice = 3,
                                        QuantityOutstanding = 8,
                                        PurchaseOrderDetail = new PurchaseOrderDetail
                                                                  {
                                                                      Part = new Part { PartNumber = "PART" },
                                                                      PurchaseOrder = new PurchaseOrder
                                                                          {
                                                                              OrderNumber = 567,
                                                                              SupplierId = 3,
                                                                              Supplier = new Supplier
                                                                                  {
                                                                                      Name = "SUPPLIER with Vendor Manager B",
                                                                                      VendorManager = new VendorManager
                                                                                          {
                                                                                              Id = "B"
                                                                                          }
                                                                                  }
                                                                          }
                                                                  }
                                    },
                                new PurchaseOrderDelivery
                                    {
                                        OrderNumber = 3,
                                        OrderLine = 3,
                                        DeliverySeq = 1,
                                        DateAdvised = new DateTime(1995, 03, 28),
                                        DateRequested = new DateTime(1995, 12, 25),
                                        BaseOurUnitPrice = 1,
                                        QuantityOutstanding = 12,
                                        PurchaseOrderDetail = new PurchaseOrderDetail
                                                                  {
                                                                      Part = new Part { PartNumber = "PART" },
                                                                      PurchaseOrder = new PurchaseOrder
                                                                          {
                                                                              OrderNumber = 111,
                                                                              SupplierId = 5,
                                                                              Supplier = new Supplier
                                                                                  {
                                                                                      Name = "SUPPLIER 5",
                                                                                      VendorManager = new VendorManager
                                                                                          {
                                                                                              Id = "C"
                                                                                          }
                                                                                  }
                                                                          }
                                                                  }
                                    },
                                new PurchaseOrderDelivery
                                    {
                                        OrderNumber = 4,
                                        OrderLine = 4,
                                        DeliverySeq = 1,
                                        DateAdvised = new DateTime(1995, 03, 28),
                                        DateRequested = new DateTime(1995, 12, 25),
                                        BaseOurUnitPrice = 1,
                                        QuantityOutstanding = 1,
                                        PurchaseOrderDetail = new PurchaseOrderDetail
                                                                  {
                                                                      Part = new Part { PartNumber = "PART" },
                                                                      PurchaseOrder = new PurchaseOrder
                                                                          {
                                                                              OrderNumber = 777,
                                                                              SupplierId = 7,
                                                                              Supplier = new Supplier
                                                                                  {
                                                                                      Name = "SUPPLIER 7",
                                                                                      VendorManager = new VendorManager
                                                                                          {
                                                                                              Id = "A"
                                                                                          }
                                                                                  }
                                                                          }
                                                                  }
                                    }
                            };
            this.Repository = Substitute.For<IPurchaseOrderDeliveryRepository>();

            this.Repository.FilterBy(Arg.Any<Expression<Func<PurchaseOrderDelivery, bool>>>())
                .Returns(this.Data.AsQueryable());
            this.ReportingHelper = new ReportingHelper();
            this.Sut = new WhatsDueInReportService(this.ReportingHelper, this.Repository);
        }
    }
}
