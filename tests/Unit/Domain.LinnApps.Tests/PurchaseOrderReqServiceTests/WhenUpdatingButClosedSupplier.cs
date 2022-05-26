namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderReqServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrderReqs;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingButSupplierOnHold : ContextBase
    {
        private readonly string fromState = "DRAFT";

        private readonly string partNo = "Pesto jar";

        private readonly int reqNumber = 5678;

        private readonly int supplierId = 77442;

        private readonly string toState = "AUTHORISE WAIT";

        private Action action;

        private PurchaseOrderReq current;

        private PurchaseOrderReq updated;

        [SetUp]
        public void SetUp()
        {
            this.current = new PurchaseOrderReq
                               {
                                   ReqNumber = this.reqNumber,
                                   RequestedById = 999,
                                   State = this.fromState,
                                   SupplierId = this.supplierId,
                                   PartNumber = this.partNo
                               };
            this.updated = new PurchaseOrderReq
                               {
                                   ReqNumber = 0,
                                   State = this.toState,
                                   ReqDate = 2.March(2022),
                                   OrderNumber = 1234,
                                   PartNumber = this.partNo,
                                   Description = "Descrip",
                                   Qty = 7,
                                   UnitPrice = 8m,
                                   Carriage = 99m,
                                   TotalReqPrice = 118m,
                                   CurrencyCode = "SMC",
                                   SupplierId = this.supplierId,
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
                                   AuthorisedById = 123,
                                   SecondAuthById = 234,
                                   FinanceCheckById = 999,
                                   TurnedIntoOrderById = 876,
                                   NominalCode = "00001234",
                                   RemarksForOrder = "needed asap",
                                   InternalNotes = "pls approv",
                                   DepartmentCode = "00002345"
                               };

            this.MockReqsStateChangeRepository.FindBy(Arg.Any<Expression<Func<PurchaseOrderReqStateChange, bool>>>())
                .Returns(
                    new PurchaseOrderReqStateChange
                        {
                            FromState = this.fromState, ToState = this.toState, UserAllowed = "Y"
                        });

            this.MockPartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(new Part { StockControlled = "N" });

            this.MockSupplierRepository.FindById(this.supplierId).Returns(
                new Supplier { SupplierId = this.supplierId, Name = "pesto shop", DateClosed = null, OrderHold = "Y" });
            this.action = () => this.Sut.Update(this.current, this.updated, new List<string>());
        }

        [Test]
        public void ShouldThrowIllegalStateException()
        {
            this.action.Should().Throw<UnauthorisedActionException>();
        }
    }
}
