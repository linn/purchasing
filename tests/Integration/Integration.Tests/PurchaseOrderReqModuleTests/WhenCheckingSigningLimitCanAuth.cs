namespace Linn.Purchasing.Integration.Tests.PurchaseOrderReqModuleTests
{
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrderReqs;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCheckingSigningLimitCanAuth : ContextBase
    {
        private readonly int reqNumber = 2023022;

        [SetUp]
        public void SetUp()
        {
            var req = new PurchaseOrderReq
                          {
                              ReqNumber = this.reqNumber,
                              State = "Order",
                              ReqDate = 2.March(2022),
                              OrderNumber = 1234,
                              PartNumber = "PCAS 007",
                              Description = "Descrip",
                              Qty = 7,
                              UnitPrice = 8m,
                              Carriage = null,
                              TotalReqPrice = null,
                              Currency = new Currency { Code = "SMC", Name = "Smackeroonies" },
                              Supplier = new Supplier { SupplierId = 111, Name = "Shoap" },
                              SupplierContact = "Lawrence Chaney",
                              AddressLine1 = "The shop",
                              AddressLine2 = "1 Main Street",
                              AddressLine3 = string.Empty,
                              AddressLine4 = "Glesga",
                              PostCode = "G1 1AA",
                              Country = new Country { CountryCode = "GB", Name = "United Kingdolls" },
                              PhoneNumber = "+44 1234567780",
                              QuoteRef = "blah",
                              Email = "LC@gmail",
                              DateRequired = null,
                              RequestedBy = new Employee { Id = 33107, FullName = "me" },
                              AuthorisedBy = new Employee { Id = 999, FullName = "someone responsible" },
                              AuthorisedById = 999,
                              SecondAuthBy = null,
                              FinanceCheckBy = new Employee { Id = 33107, FullName = "me" },
                              TurnedIntoOrderBy = new Employee { Id = 33107, FullName = "me" },
                              Nominal = new Nominal { NominalCode = "00001234", Description = "hing" },
                              RemarksForOrder = "needed asap",
                              InternalNotes = "pls approv",
                              Department = new Department { DepartmentCode = "00002345", Description = "Team 1" },
                          };

            this.MockPurchaseOrderReqRepository.FindById(this.reqNumber).Returns(req);

            this.MockReqDomainService.CheckIfSigningLimitCanAuthorisePurchaseOrder(
                Arg.Any<PurchaseOrderReq>(), Arg.Any<int>()).Returns(new ProcessResult(true, "can auth"));

            this.Response = this.Client.Post(
                $"/purchasing/purchase-orders/reqs/check-signing-limit-covers-po-auth?reqNumber={this.reqNumber}",
                with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldCallCreate()
        {
            this.MockReqDomainService.Received().CheckIfSigningLimitCanAuthorisePurchaseOrder(
                Arg.Any<PurchaseOrderReq>(), Arg.Any<int>());
        }

        [Test]
         public void ShouldCallRepositoryFindById()
        {
            this.MockPurchaseOrderReqRepository.Received().FindById(this.reqNumber);
        }

         [Test]
        public void ShouldNotCommit()
        {
            this.TransactionManager.DidNotReceive().Commit();
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resultResource = this.Response.DeserializeBody<ProcessResult>();
            resultResource.Should().NotBeNull();
            resultResource.Success.Should().BeTrue();
            resultResource.Message.Should().Be("can auth");
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            var response = this.Response;

            response.Content.Headers.ContentType.Should().NotBeNull();
            response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
