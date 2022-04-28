namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderReqServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrderReqs;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdating : ContextBase
    {
        private readonly string fromState = "DRAFT";

        private readonly int reqNumber = 5678;

        private readonly string toState = "AUTHORISE WAIT";

        private PurchaseOrderReq current;

        private PurchaseOrderReq updated;

        [SetUp]
        public void SetUp()
        {
            this.current =
                new PurchaseOrderReq { ReqNumber = this.reqNumber, RequestedById = 999, State = this.fromState };
            this.updated = new PurchaseOrderReq
                               {
                                   ReqNumber = 0,
                                   State = this.toState,
                                   ReqDate = 2.March(2022),
                                   OrderNumber = 1234,
                                   PartNumber = "PCAS 007",
                                   Description = "Descrip",
                                   Qty = 7,
                                   UnitPrice = 8m,
                                   Carriage = 99m,
                                   TotalReqPrice = 118m,
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
            this.Sut.Update(this.current, this.updated, new List<string>());
        }

        [Test]
        public void ShouldUpdate()
        {
            this.current.ReqDate.Should().Be(this.updated.ReqDate);
            this.current.OrderNumber.Should().Be(this.updated.OrderNumber);
            this.current.PartNumber.Should().Be(this.updated.PartNumber);
            this.current.Description.Should().Be(this.updated.Description);
            this.current.Qty.Should().Be(this.updated.Qty);
            this.current.UnitPrice.Should().Be(this.updated.UnitPrice);
            this.current.Carriage.Should().Be(this.updated.Carriage);
            this.current.TotalReqPrice.Should().Be(this.updated.TotalReqPrice);
            this.current.CurrencyCode.Should().Be(this.updated.CurrencyCode);
            this.current.SupplierId.Should().Be(this.updated.SupplierId);
            this.current.SupplierName.Should().Be(this.updated.SupplierName);
            this.current.SupplierContact.Should().Be(this.updated.SupplierContact);
            this.current.AddressLine1.Should().Be(this.updated.AddressLine1);
            this.current.AddressLine2.Should().Be(this.updated.AddressLine2);
            this.current.AddressLine3.Should().Be(this.updated.AddressLine3);
            this.current.AddressLine4.Should().Be(this.updated.AddressLine4);
            this.current.PostCode.Should().Be(this.updated.PostCode);
            this.current.AddressLine1.Should().Be(this.updated.AddressLine1);
            this.current.CountryCode.Should().Be(this.updated.CountryCode);
            this.current.PhoneNumber.Should().Be(this.updated.PhoneNumber);
            this.current.QuoteRef.Should().Be(this.updated.QuoteRef);
            this.current.Email.Should().Be(this.updated.Email);
            this.current.DateRequired.Should().Be(this.updated.DateRequired);
            this.current.RequestedById.Should().Be(999); // don't let requested by field by updated after create
            this.current.AuthorisedById.Should().Be(this.current.AuthorisedById);
            this.current.SecondAuthById.Should().Be(this.current.SecondAuthById);
            this.current.FinanceCheckById.Should().Be(this.current.FinanceCheckById);
            this.current.TurnedIntoOrderById.Should().Be(this.current.TurnedIntoOrderById);
            this.current.Nominal.Should().Be(this.updated.Nominal);
            this.current.RemarksForOrder.Should().Be(this.updated.RemarksForOrder);
            this.current.InternalNotes.Should().Be(this.updated.InternalNotes);
            this.current.Department.Should().Be(this.updated.Department);
        }

        [Test]
        public void ShouldNotUpdateReqNumber()
        {
            this.current.ReqNumber.Should().Be(this.reqNumber);
        }
    }
}
