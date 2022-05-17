namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderDeliveryServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.PurchaseLedger;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IRepository<PurchaseOrderDelivery, PurchaseOrderDeliveryKey> Repository { get; private set; }

        protected IEnumerable<PurchaseOrderDelivery> Data { get; private set; }

        protected IPurchaseOrderDeliveryService Sut { get; private set; }

        protected IAuthorisationService AuthService { get; private set; }

        protected IRepository<RescheduleReason, string> RescheduleReasonRepository { get; private set; }

        protected ISingleRecordRepository<PurchaseLedgerMaster> PurchaseLedgerMaster { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.Repository = Substitute.For<IRepository<PurchaseOrderDelivery, PurchaseOrderDeliveryKey>>();
            this.AuthService = Substitute.For<IAuthorisationService>();
            this.RescheduleReasonRepository = Substitute.For<IRepository<RescheduleReason, string>>();
            this.PurchaseLedgerMaster = Substitute.For<ISingleRecordRepository<PurchaseLedgerMaster>>();
            this.Data = new List<PurchaseOrderDelivery>
                            {
                                new PurchaseOrderDelivery
                                    {
                                        OrderNumber = 123451,
                                        DateAdvised = null,
                                        PurchaseOrderDetail = new PurchaseOrderDetail
                                                                  {
                                                                      PurchaseOrder = new PurchaseOrder
                                                                          {
                                                                              SupplierId = 1,
                                                                              Supplier = new Supplier { Name = "SUPPLIER 1" }
                                                                          }
                                                                  }
                                    },
                                new PurchaseOrderDelivery
                                    {
                                        OrderNumber = 123452,
                                        DateAdvised = DateTime.Today,
                                        PurchaseOrderDetail = new PurchaseOrderDetail
                                                                  {
                                                                      PurchaseOrder = new PurchaseOrder
                                                                          {
                                                                              SupplierId = 2,
                                                                              Supplier = new Supplier { Name = "SUPPLIER 2" }
                                                                          }
                                                                  }
                                    },
                                new PurchaseOrderDelivery
                                    {
                                        OrderNumber = 123453,
                                        DateAdvised = null,
                                        PurchaseOrderDetail = new PurchaseOrderDetail
                                                                  {
                                                                      PurchaseOrder = new PurchaseOrder
                                                                          {
                                                                              SupplierId = 3,
                                                                              Supplier = new Supplier { Name = "SUPPLIER 3" }
                                                                          }
                                                                  }
                                    },
                                new PurchaseOrderDelivery
                                    {
                                        OrderNumber = 123455,
                                        DateAdvised = DateTime.Today,
                                        PurchaseOrderDetail = new PurchaseOrderDetail
                                                                  {
                                                                      PurchaseOrder = new PurchaseOrder
                                                                          {
                                                                              SupplierId = 5,
                                                                              Supplier = new Supplier { Name = "SUPPLIER 5" }
                                                                          }
                                                                  }
                                    },
                                new PurchaseOrderDelivery
                                    {
                                        OrderNumber = 123454,
                                        DateAdvised = null,
                                        PurchaseOrderDetail = new PurchaseOrderDetail
                                                                  {
                                                                      PurchaseOrder = new PurchaseOrder
                                                                          {
                                                                              SupplierId = 4,
                                                                              Supplier = new Supplier { Name = "SUPPLIER 4" }
                                                                          }
                                                                  }
                                    },
                                new PurchaseOrderDelivery
                                    {
                                        OrderNumber = 223451,
                                        DateAdvised = null,
                                        PurchaseOrderDetail = new PurchaseOrderDetail
                                                                  {
                                                                      PurchaseOrder = new PurchaseOrder
                                                                          {
                                                                              SupplierId = 1,
                                                                              Supplier = new Supplier { Name = "SUPPLIER 1" }
                                                                          }
                                                                  }
                                    },
                                new PurchaseOrderDelivery
                                    {
                                        OrderNumber = 223452,
                                        DateAdvised = DateTime.Today,
                                        PurchaseOrderDetail = new PurchaseOrderDetail
                                                                  {
                                                                      PurchaseOrder = new PurchaseOrder
                                                                          {
                                                                              SupplierId = 2,
                                                                              Supplier = new Supplier { Name = "SUPPLIER 2" }
                                                                          }
                                                                  }
                                    },
                                new PurchaseOrderDelivery
                                    {
                                        OrderNumber = 223453,
                                        DateAdvised = null,
                                        PurchaseOrderDetail = new PurchaseOrderDetail
                                                                  {
                                                                      PurchaseOrder = new PurchaseOrder
                                                                          {
                                                                              SupplierId = 3,
                                                                              Supplier = new Supplier { Name = "SUPPLIER 3" }
                                                                          }
                                                                  }
                                    },
                                new PurchaseOrderDelivery
                                    {
                                        OrderNumber = 223455,
                                        DateAdvised = DateTime.Today,
                                        PurchaseOrderDetail = new PurchaseOrderDetail
                                                                  {
                                                                      PurchaseOrder = new PurchaseOrder
                                                                          {
                                                                              SupplierId = 5,
                                                                              Supplier = new Supplier { Name = "SUPPLIER 5" }
                                                                          }
                                                                  }
                                    },
                                new PurchaseOrderDelivery
                                    {
                                        OrderNumber = 223454,
                                        DateAdvised = null,
                                        PurchaseOrderDetail = new PurchaseOrderDetail
                                                                  {
                                                                      PurchaseOrder = new PurchaseOrder
                                                                          {
                                                                              SupplierId = 44,
                                                                              Supplier = new Supplier { Name = "SUPPLIER 44" }
                                                                          }
                                                                  }
                                    },
                            };

            this.Repository.FindAll().Returns(this.Data.AsQueryable());

            this.Sut = new PurchaseOrderDeliveryService(
                this.Repository, 
                this.AuthService, 
                this.RescheduleReasonRepository,
                this.PurchaseLedgerMaster);
        }
    }
}
