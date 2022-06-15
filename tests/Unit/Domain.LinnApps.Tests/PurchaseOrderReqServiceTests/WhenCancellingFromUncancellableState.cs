namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderReqServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrderReqs;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCancellingFromUncancellableState : ContextBase
    {
        private readonly int authoriserUserNumber = 33107;

        private readonly string fromState = "ORDER";

        private readonly int reqNumber = 5678;

        private PurchaseOrderReq entity;

        private Action action;

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
                                  Description = "Descrip",
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
                                  AuthorisedById = 33107,
                                  SecondAuthById = null,
                                  FinanceCheckById = null,
                                  TurnedIntoOrderById = null,
                                  NominalCode = "00001234",
                                  RemarksForOrder = "needed asap",
                                  InternalNotes = "pls approv",
                                  DepartmentCode = "00002345"
                              };

            this.MockReqsStateChangeRepository.FindBy(Arg.Any<Expression<Func<PurchaseOrderReqStateChange, bool>>>())
                .Returns((PurchaseOrderReqStateChange)null);

            this.EmployeeRepository.FindById(this.authoriserUserNumber).Returns(
                new Employee { FullName = "Big Jimbo", Id = this.authoriserUserNumber });

            this.action = () => this.Sut.Cancel(this.entity, new List<string>());
        }

        [Test]
        public void ShouldNotUpdateState()
        {
            this.entity.State.Should().Be(this.fromState);
        }

        [Test]
        public void ShouldThrowIllegalStateException()
        {
            this.action.Should().Throw<IllegalPoReqStateChangeException>();
        }
    }
}
