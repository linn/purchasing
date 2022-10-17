﻿namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderReqServiceTests
{
    using System.Collections.Generic;
    
    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrderReqs;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenEmailingForAuthorisation : ContextBase
    {
        private readonly int currentUserNumber = 5512;

        private readonly string fromState = "FINANCE WAIT";

        private readonly int reqNumber = 5678;

        private PurchaseOrderReq entity;

        private ProcessResult result;

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
                                  RequestedBy = new Employee { Id = 33107, FullName = "me" },
                                  AuthorisedById = null,
                                  SecondAuthById = null,
                                  FinanceCheckById = null,
                                  TurnedIntoOrderById = null,
                                  NominalCode = "00001234",
                                  RemarksForOrder = "needed asap",
                                  InternalNotes = "pls approv",
                                  DepartmentCode = "00002345"
                              };

            this.MockAuthService.HasPermissionFor(
                AuthorisedAction.PurchaseOrderReqFinanceCheck,
                Arg.Any<List<string>>()).Returns(true);

            this.EmployeeRepository.FindById(this.currentUserNumber).Returns(
                new Employee
                    {
                        FullName = "Big Jimbo",
                        Id = this.currentUserNumber,
                        PhoneListEntry = new PhoneListEntry { EmailAddress = "bigjim@gmail " }
                    });
            this.EmployeeRepository.FindById(213).Returns(
                new Employee
                    {
                        FullName = "stormZee",
                        Id = this.currentUserNumber,
                        PhoneListEntry = new PhoneListEntry { EmailAddress = "bigstormz@gmail" }
                    });

            this.result = this.Sut.SendAuthorisationRequestEmail(this.currentUserNumber, 213, this.entity);
        }

        [Test]
        public void ShouldCallProxyWithCorrectParams()
        {
            this.EmailService.Received().SendEmail(
                "bigstormz@gmail",
                "stormZee",
                Arg.Any<IEnumerable<Dictionary<string, string>>>(),
                Arg.Any<IEnumerable<Dictionary<string, string>>>(),
                "bigjim@gmail",
                "Big Jimbo",
                $"Purchase Order Req {this.entity.ReqNumber} requires authorisation",
                Arg.Any<string>());
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.result.Success.Should().BeTrue();
            this.result.Message.Should().Be("Email Sent");
        }
    }
}
