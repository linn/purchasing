﻿namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderReqServiceTests
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

    public class WhenCreatingAndNoDateRequiredSpecified : ContextBase
    {
        private readonly int authoriserUserNumber = 33107;

        private readonly string fromState = "ORDER WAIT";

        private readonly string toState = "ORDER";

        private Action action;

        private PurchaseOrderReq candidate;

        [SetUp]
        public void SetUp()
        {
            this.candidate = new PurchaseOrderReq
                                 {
                                     ReqNumber = 5678,
                                     State = "ORDER WAIT",
                                     ReqDate = 2.March(2022),
                                     OrderNumber = 1234,
                                     PartNumber = "PCAS 007",
                                     Description = "Description",
                                     Qty = 7,
                                     UnitPrice = 8m,
                                     TotalReqPrice = 145m,
                                     CurrencyCode = "SMC",
                                     SupplierId = 111,
                                     SupplierName = "P.Atel",
                                     SupplierContact = "Bobson Dugnutt",
                                     AddressLine1 = "Big Time Trading Co.",
                                     AddressLine2 = "2/2 44 West Princes Street",
                                     AddressLine3 = "The Wild West",
                                     AddressLine4 = "Glesga",
                                     PostCode = "G1 1AA",
                                     CountryCode = "GB",
                                     PhoneNumber = "+44 1234567780",
                                     QuoteRef = "blah",
                                     Email = "BigTimeDugnutt@gmail.com",
                                     DateRequired = null,
                                     RequestedById = 33107,
                                     AuthorisedById = 33107,
                                     SecondAuthById = null,
                                     FinanceCheckById = 33107,
                                     TurnedIntoOrderById = null,
                                     NominalCode = "00001234",
                                     RemarksForOrder = "Bobby need",
                                     InternalNotes = "pls approv",
                                     DepartmentCode = "00002345"
            };

            this.MockReqsStateChangeRepository.FindBy(Arg.Any<Expression<Func<PurchaseOrderReqStateChange, bool>>>())
                .Returns(
                    new PurchaseOrderReqStateChange
                    {
                        FromState = this.fromState,
                        ToState = this.toState,
                        ComputerAllowed = "Y",
                        UserAllowed = "N"
                    });

            this.MockAuthService.HasPermissionFor(
                AuthorisedAction.PurchaseOrderReqFinanceCheck,
                Arg.Any<List<string>>()).Returns(true);

            this.MockCurrencyPack.CalculateBaseValueFromCurrencyValue(
                this.candidate.CurrencyCode,
                this.candidate.TotalReqPrice.Value).Returns(145m);

            this.MockPurchaseOrdersPack.OrderCanBeAuthorisedBy(
                null,
                null,
                this.authoriserUserNumber,
                145m,
                this.candidate.PartNumber,
                "PO").Returns(true);

            this.MockPurchaseOrderAutoOrderPack.CreateMiniOrderFromReq(
                this.candidate.NominalCode,
                this.candidate.DepartmentCode,
                this.candidate.RequestedById,
                this.authoriserUserNumber,
                this.candidate.Description,
                this.candidate.QuoteRef,
                this.candidate.RemarksForOrder,
                this.candidate.PartNumber,
                this.candidate.SupplierId,
                this.candidate.Qty,
                this.candidate.DateRequired,
                this.candidate.UnitPrice,
                true,
                this.candidate.InternalNotes)
                .Returns(new CreateOrderFromReqResult { OrderNumber = 101137, Success = true });

            this.EmployeeRepository.FindById(this.authoriserUserNumber).Returns(
                new Employee { FullName = "KT", Id = this.authoriserUserNumber });

            this.action = () => this.Sut.CreateOrderFromReq(this.candidate, new List<string>(), this.authoriserUserNumber);
        }

        [Test]
        public void ShouldThrowExceptionAboutLackOfDate()
        {
            this.action.Should().Throw<PurchaseOrderReqException>().WithMessage("Cannot create order from a req without a future date specified");
        }
    }
}
