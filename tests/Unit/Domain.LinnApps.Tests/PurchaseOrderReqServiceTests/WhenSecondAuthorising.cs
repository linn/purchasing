namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderReqServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSecondAuthorising : ContextBase
    {
        private readonly string fromState = "AUTHORISE 2ND WAIT";

        private readonly int reqNumber = 5678;

        private readonly int authoriserUserNumber = 999;

        private readonly string toState = "FINANCE WAIT";

        private PurchaseOrderReq entity;

        [SetUp]
        public void SetUp()
        {
            this.entity = new PurchaseOrderReq
                              {
                                  ReqNumber = this.reqNumber,
                                  State = this.fromState,
                                  ReqDate = 2.March(2022),
                                  OrderNumber = 1234,
                                  PartNumber = "PCAS 007",
                                  PartDescription = "Descrip",
                                  Qty = 7,
                                  UnitPrice = 8m,
                                  Carriage = 99m,
                                  TotalReqPrice = 145m,
                                  CurrencyCode = "SMC",
                                  SupplierId = 111,
                                  SupplierName = "the things shop",
                                  SupplierContact = "Lawrence Chaney",
                                  AddressLine1 = "The shop",
                                  AddressLine2 = "1 Main Street",
                                  AddressLine3 = "town centre",
                                  AddressLine4 = "Glesga",
                                  PostCode = "G1 1AA",
                                  CountryCode = "GB",
                                  PhoneNumber = "+44 1234567780",
                                  QuoteRef = "blah",
                                  Email = "LC@gmail",
                                  DateRequired = 1.January(2023),
                                  RequestedById = 33107,
                                  AuthorisedById = 10111,
                                  SecondAuthById = null,
                                  FinanceCheckById = null,
                                  TurnedIntoOrderById = null,
                                  NominalCode = "00001234",
                                  RemarksForOrder = "needed asap",
                                  InternalNotes = "pls approv",
                                  DepartmentCode = "00002345"
                              };

            this.MockReqsStateChangeRepository.FindBy(Arg.Any<Expression<Func<PurchaseOrderReqStateChange, bool>>>())
                .Returns(
                    new PurchaseOrderReqStateChange
                        {
                            FromState = this.fromState,
                            ToState = this.toState,
                            UserAllowed = "Y"
                        });

            this.MockPurchaseOrderReqsPack.AllowedToAuthorise(
                "AUTH2",
                this.authoriserUserNumber,
                this.entity.TotalReqPrice.Value,
                this.entity.DepartmentCode,
                this.fromState).Returns(new AllowedToAuthoriseReqResult { Success = true, NewState = this.toState });

            this.Sut.Authorise(this.entity, new List<string>(), this.authoriserUserNumber);
        }

        [Test]
        public void ShouldUpdateStateAndAuthorisedBy()
        {
            this.entity.SecondAuthById.Should().Be(this.authoriserUserNumber);
            this.entity.State.Should().Be(this.toState);
        }
    }
}
