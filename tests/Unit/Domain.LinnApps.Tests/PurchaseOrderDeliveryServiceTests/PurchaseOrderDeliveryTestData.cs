namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderDeliveryServiceTests
{
    using System;
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NUnit.Framework;

    public class PurchaseOrderDeliveryTestData
    {
        public static List<PurchaseOrderDelivery> BuildData()
        {
            return new List<PurchaseOrderDelivery>
                       {
                           new PurchaseOrderDelivery
                               {
                                   OrderNumber = 123451,
                                   AdvisedDate = null,
                                   PurchaseOrderDetail = new PurchaseOrderDetail
                                                             {
                                                                 PurchaseOrder = new PurchaseOrder
                                                                     {
                                                                         SupplierId = 1,
                                                                         Supplier = new Supplier
                                                                             {
                                                                                 Name = "SUPPLIER 1"
                                                                             }
                                                                     }
                                                             }
                               },
                           new PurchaseOrderDelivery
                               {
                                   OrderNumber = 123452,
                                   AdvisedDate = DateTime.Today,
                                   PurchaseOrderDetail = new PurchaseOrderDetail
                                                             {
                                                                 PurchaseOrder = new PurchaseOrder
                                                                     {
                                                                         SupplierId = 2,
                                                                         Supplier = new Supplier
                                                                             {
                                                                                 Name = "SUPPLIER 2"
                                                                             }
                                                                     }
                                                             }
                               },
                           new PurchaseOrderDelivery
                               {
                                   OrderNumber = 123453,
                                   AdvisedDate = null,
                                   PurchaseOrderDetail = new PurchaseOrderDetail
                                                             {
                                                                 PurchaseOrder = new PurchaseOrder
                                                                     {
                                                                         SupplierId = 3,
                                                                         Supplier = new Supplier
                                                                             {
                                                                                 Name = "SUPPLIER 3"
                                                                             }
                                                                     }
                                                             }
                               },
                           new PurchaseOrderDelivery
                               {
                                   OrderNumber = 123455,
                                   AdvisedDate = DateTime.Today,
                                   PurchaseOrderDetail = new PurchaseOrderDetail
                                                             {
                                                                 PurchaseOrder = new PurchaseOrder
                                                                     {
                                                                         SupplierId = 5,
                                                                         Supplier = new Supplier
                                                                             {
                                                                                 Name = "SUPPLIER 5"
                                                                             }
                                                                     }
                                                             }
                               },
                           new PurchaseOrderDelivery
                               {
                                   OrderNumber = 123454,
                                   AdvisedDate = null,
                                   PurchaseOrderDetail = new PurchaseOrderDetail
                                                             {
                                                                 PurchaseOrder = new PurchaseOrder
                                                                     {
                                                                         SupplierId = 4,
                                                                         Supplier = new Supplier
                                                                             {
                                                                                 Name = "SUPPLIER 4"
                                                                             }
                                                                     }
                                                             }
                               },
                           new PurchaseOrderDelivery
                               {
                                   OrderNumber = 223451,
                                   AdvisedDate = null,
                                   PurchaseOrderDetail = new PurchaseOrderDetail
                                                             {
                                                                 PurchaseOrder = new PurchaseOrder
                                                                     {
                                                                         SupplierId = 1,
                                                                         Supplier = new Supplier
                                                                             {
                                                                                 Name = "SUPPLIER 1"
                                                                             }
                                                                     }
                                                             }
                               },
                           new PurchaseOrderDelivery
                               {
                                   OrderNumber = 223452,
                                   AdvisedDate = DateTime.Today,
                                   PurchaseOrderDetail = new PurchaseOrderDetail
                                                             {
                                                                 PurchaseOrder = new PurchaseOrder
                                                                     {
                                                                         SupplierId = 2,
                                                                         Supplier = new Supplier
                                                                             {
                                                                                 Name = "SUPPLIER 2"
                                                                             }
                                                                     }
                                                             }
                               },
                           new PurchaseOrderDelivery
                               {
                                   OrderNumber = 223453,
                                   AdvisedDate = null,
                                   PurchaseOrderDetail = new PurchaseOrderDetail
                                                             {
                                                                 PurchaseOrder = new PurchaseOrder
                                                                     {
                                                                         SupplierId = 3,
                                                                         Supplier = new Supplier
                                                                             {
                                                                                 Name = "SUPPLIER 3"
                                                                             }
                                                                     }
                                                             }
                               },
                           new PurchaseOrderDelivery
                               {
                                   OrderNumber = 223455,
                                   AdvisedDate = DateTime.Today,
                                   PurchaseOrderDetail = new PurchaseOrderDetail
                                                             {
                                                                 PurchaseOrder = new PurchaseOrder
                                                                     {
                                                                         SupplierId = 5,
                                                                         Supplier = new Supplier
                                                                             {
                                                                                 Name = "SUPPLIER 5"
                                                                             }
                                                                     }
                                                             }
                               },
                           new PurchaseOrderDelivery
                               {
                                   OrderNumber = 223454,
                                   AdvisedDate = null,
                                   PurchaseOrderDetail = new PurchaseOrderDetail
                                                             {
                                                                 PurchaseOrder = new PurchaseOrder
                                                                     {
                                                                         SupplierId = 44,
                                                                         Supplier = new Supplier
                                                                             {
                                                                                 Name = "SUPPLIER 44"
                                                                             }
                                                                     }
                                                             }
                               },
                       };
        }
    }
}
