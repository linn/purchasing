namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests.SwitchQtyPriceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders.MiniOrders;

    using NSubstitute;

    using NUnit.Framework;

    public abstract class SwitchQtyPriceContextBase : ContextBase
    {
        public PurchaseOrder PurchaseOrder { get; set; }

        public int OrderNumber { get; set; }

        public int EmployeeId { get; set; }

        public Action Action { get; set; }

        [SetUp]
        public void SetUpSwitchContext()
        {
            this.OrderNumber = 249235;
            this.EmployeeId = 94849;
            this.MiniOrderRepository.FindById(this.OrderNumber).Returns(new MiniOrder());
            this.NominalAccountRepository.FindById(Arg.Any<int>()).Returns(
                new NominalAccount { AccountId = 911, NominalCode = "00009222", DepartmentCode = "0000911" });
            this.PurchaseOrder = new PurchaseOrder
                                     {
                                         OrderNumber = this.OrderNumber,
                                         Details = new List<PurchaseOrderDetail>
                                                       {
                                                           new PurchaseOrderDetail
                                                               {
                                                                   OrderNumber = this.OrderNumber,
                                                                   Line = 1,
                                                                   Part = new Part { PartNumber = "P1", StockControlled = "N" },
                                                                   PartNumber = "P1",
                                                                   OrderQty = 1,
                                                                   OurQty = 1,
                                                                   OurUnitPriceCurrency = 3800,
                                                                   OrderUnitPriceCurrency = 3800,
                                                                   OrderConversionFactor = 1,
                                                                   BaseOurUnitPrice = 3800,
                                                                   BaseOrderUnitPrice = 3800,
                                                                   DetailTotalCurrency = 3800,
                                                                   NetTotalCurrency = 3800,
                                                                   PurchaseDeliveries = new List<PurchaseOrderDelivery>
                                                                       {
                                                                           new PurchaseOrderDelivery
                                                                               {
                                                                                   OurDeliveryQty = 1,
                                                                                   OrderDeliveryQty = 1,
                                                                                   OurUnitPriceCurrency = 3800,
                                                                                   OrderUnitPriceCurrency = 3800,
                                                                                   BaseOurUnitPrice = 3800,
                                                                                   BaseOrderUnitPrice = 3800,
                                                                                   NetTotalCurrency = 3800,
                                                                                   DeliveryTotalCurrency = 3800
                                                                               }
                                                                       },
                                                                   OrderPosting = new PurchaseOrderPosting
                                                                       {
                                                                           Qty = 1,
                                                                           NominalAccountId = 123
                                                                       }
                                                               }
                                                       }
                                     };

            this.MockAuthService.HasPermissionFor(AuthorisedAction.PurchaseLedgerAdmin, Arg.Any<List<string>>())
                .Returns(true);
            this.PurchaseOrderRepository.FindById(this.OrderNumber).Returns(this.PurchaseOrder);
        }
    }
}
