namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.PurchaseLedger;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders.MiniOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingNominalAccountAndQtyAlreadyBookedIn : ContextBase
    {
        private PurchaseOrder current;

        private PurchaseOrder updated;

        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.current = new PurchaseOrder
            {
                DocumentType = new DocumentType
                {
                    Description = "Regular Purchase Order",
                    Name = "PO"
                },
                OrderDate = 10.January(2021),
                SupplierId = 1224,
                Details =
                    new List<PurchaseOrderDetail>
                        {
                            new PurchaseOrderDetail
                                {
                                    OrderPosting = new PurchaseOrderPosting
                                                       {
                                                           NominalAccount = new NominalAccount 
                                                                                {
                                                                                    AccountId = 911
                                                                                }
                                                       },
                                    PurchaseDeliveries = new List<PurchaseOrderDelivery> { new PurchaseOrderDelivery { QtyNetReceived = 1 } }
                                }
                        },
            };

            this.updated = new PurchaseOrder
            {
                ExchangeRate = 1m,
                DocumentType = new DocumentType
                {
                    Name = "PO"
                },
                OrderDate = 10.January(2021),
                SupplierId = 1224,
                Details = new List<PurchaseOrderDetail>
                              {
                                  new PurchaseOrderDetail
                                      {
                                          OrderPosting = new PurchaseOrderPosting
                                                             {
                                                                 NominalAccount = new NominalAccount
                                                                         {
                                                                             AccountId = 3940,
                                                                             DepartmentCode = "0001111",
                                                                             NominalCode = "00002222",
                                                                             Department =
                                                                                 new Department
                                                                                     {
                                                                                         Description = "DEP",
                                                                                         DepartmentCode = "0001111"
                                                                                     },
                                                                             Nominal = new Nominal
                                                                                 {
                                                                                     Description = "NOM",
                                                                                     NominalCode = "00002222"
                                                                                 }
                                                                         }
                                                             },
                                          PurchaseDeliveries = new List<PurchaseOrderDelivery> { new PurchaseOrderDelivery { QtyNetReceived = 1 } }
                                      }
                              },
            };

            this.MockAuthService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.PurchaseLedgerMaster.GetRecord().Returns(new PurchaseLedgerMaster { OkToRaiseOrder = "Y" });

            this.NominalAccountRepository.FilterBy(Arg.Any<Expression<Func<NominalAccount, bool>>>()).Returns(
                new List<NominalAccount>
                    {
                        new NominalAccount
                            {
                                AccountId = 3940,
                                NominalCode = "0001111",
                                DepartmentCode = "0001111"
                            }
                    }.AsQueryable());

            this.action = () => this.Sut.UpdateOrder(this.current, this.updated, new List<string>());
        }

        [Test]
        public void ShouldUpdatePurchaseOrderFields()
        {
            this.action.Should().Throw<PurchaseOrderException>()
                .WithMessage("Cannot update nominal account after some qty of the order has been booked in");
        }
    }
}
