namespace Linn.Purchasing.Integration.Tests.PurchaseOrderModuleTests
{
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingPurchaseOrderReq : ContextBase
    {
        private readonly int reqNumber = 2023022;

        private PurchaseOrderReqResource resource;

        [SetUp]
        public void SetUp()
        {
            this.resource = new PurchaseOrderReqResource
                                {
                                    ReqNumber = this.reqNumber,
                                    State = "purgatory",
                                    ReqDate = 2.March(2022).ToString("o"),
                                    OrderNumber = 1234,
                                    PartNumber = "PCAS 007",
                                    PartDescription = "Descrip",
                                    Qty = 7,
                                    UnitPrice = 8m,
                                    Carriage = null,
                                    TotalReqPrice = null,
                                    Currency = new CurrencyResource { Code = "SMC", Name = "Smackeroonies" },
                                    Supplier = new SupplierResource { Id = 111, Name = "Shoap" },
                                    SupplierContact = "Lawrence Chaney",
                                    AddressLine1 = "The shop",
                                    AddressLine2 = "1 Main Street",
                                    AddressLine3 = string.Empty,
                                    AddressLine4 = "Glesga",
                                    PostCode = "G1 1AA",
                                    Country =
                                        new CountryResource { CountryCode = "GB", CountryName = "United Kingdolls" },
                                    PhoneNumber = "+44 1234567780",
                                    QuoteRef = "blah",
                                    Email = "LC@gmail",
                                    DateRequired = string.Empty,
                                    RequestedBy = new EmployeeResource { Id = 33107, FullName = "me" },
                                    AuthorisedBy = null,
                                    SecondAuthBy = null,
                                    FinanceCheckBy = null,
                                    TurnedIntoOrderBy = null,
                                    Nominal = "dono",
                                    RemarksForOrder = "needed asap",
                                    InternalNotes = "pls approv",
                                    Department = "Team 1"
                                };

            var req = new PurchaseOrderReq
            {
                ReqNumber = this.reqNumber,
                State = "purgatory",
                ReqDate = 2.March(2022),
                OrderNumber = 1234,
                PartNumber = "PCAS 007",
                PartDescription = "Descrip",
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
                AuthorisedBy = null,
                SecondAuthBy = null,
                FinanceCheckBy = null,
                TurnedIntoOrderBy = null,
                Nominal = "dono",
                RemarksForOrder = "needed asap",
                InternalNotes = "pls approv",
                Department = "Team 1"
            };

            this.MockPurchaseOrderReqRepository.FindById(this.reqNumber).Returns(req);

            this.MockReqDomainService.Update(
                Arg.Any<PurchaseOrderReq>(),
                Arg.Any<PurchaseOrderReq>(),
                Arg.Any<IEnumerable<string>>()).Returns(true);

            this.Response = this.Client.Put(
                "/purchasing/purchase-orders/reqs/2023022",
                this.resource,
                with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldCallUpdate()
        {
            this.MockReqDomainService.Received().Update(
                Arg.Any<PurchaseOrderReq>(),
                Arg.Any<PurchaseOrderReq>(),
                Arg.Any<IEnumerable<string>>());
        }

        [Test]
        public void ShouldCallRepo()
        {
            this.MockPurchaseOrderReqRepository.Received().FindById(2023022);
        }

        [Test]
        public void ShouldCommit()
        {
            this.TransactionManager.Received().Commit();
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resultResource = this.Response.DeserializeBody<PurchaseOrderReqResource>();
            resultResource.Should().NotBeNull();
            resultResource.ReqNumber.Should().Be(this.reqNumber);
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            var response = this.Response;

            response.Content.Headers.ContentType.Should().NotBeNull();
            response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
