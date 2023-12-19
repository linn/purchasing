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

    public class WhenFinanceAuthPOReq : ContextBase
    {
        private readonly int reqNumber = 2023022;

        private PurchaseOrderReqResource resource;

        [SetUp]
        public void SetUp()
        {
            this.resource = new PurchaseOrderReqResource
            {
                State = "Order Wait",
                ReqDate = 2.March(2022).ToString("o"),
                OrderNumber = 1234,
                PartNumber = "PCAS 007",
                Description = "Descrip",
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
                AuthorisedBy = new EmployeeResource { Id = 999, FullName = "someone responsible" },
                SecondAuthBy = null,
                FinanceCheckBy = new EmployeeResource { Id = 1111, FullName = "someone financially responsible" },
                TurnedIntoOrderBy = null,
                Nominal = new NominalResource { NominalCode = "00001234", Description = "hing" },
                RemarksForOrder = "needed asap",
                InternalNotes = "pls approv",
                Department = new DepartmentResource { DepartmentCode = "00002345", DepartmentDescription = "Team 1" },
            };

            var req = new PurchaseOrderReq
            {
                ReqNumber = this.reqNumber,
                State = "Order Wait",
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
                FinanceCheckById = 1111,
                FinanceCheckBy = new Employee { Id = 1111, FullName = "someone financially responsible" },

                TurnedIntoOrderBy = null,
                Nominal = new Nominal { NominalCode = "00001234", Description = "hing" },
                RemarksForOrder = "needed asap",
                InternalNotes = "pls approv",
                Department = new Department { DepartmentCode = "00002345", Description = "Team 1" },
            };

            this.MockPurchaseOrderReqRepository.FindById(this.reqNumber).Returns(req);

            this.MockReqDomainService.FinanceApprove(
                Arg.Any<PurchaseOrderReq>(),
                Arg.Any<IEnumerable<string>>(), Arg.Any<int>());

            this.Response = this.Client.Post(
                $"/purchasing/purchase-orders/reqs/{this.reqNumber}/finance-authorise",
                with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldCallCreate()
        {
            this.MockReqDomainService.Received().FinanceApprove(
                Arg.Any<PurchaseOrderReq>(),
                Arg.Any<IEnumerable<string>>(), Arg.Any<int>());
        }

        [Test]
        public void ShouldCallRepositoryFindById()
        {
            this.MockPurchaseOrderReqRepository.Received().FindById(this.reqNumber);
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
            resultResource.ReqDate.Should().Be(this.resource.ReqDate);
            resultResource.OrderNumber.Should().Be(this.resource.OrderNumber);
            resultResource.PartNumber.Should().Be(this.resource.PartNumber);
            resultResource.Description.Should().Be(this.resource.Description);
            resultResource.Qty.Should().Be(this.resource.Qty);
            resultResource.UnitPrice.Should().Be(this.resource.UnitPrice);
            resultResource.Carriage.Should().Be(this.resource.Carriage);
            resultResource.TotalReqPrice.Should().Be(this.resource.TotalReqPrice);
            resultResource.Currency.Code.Should().Be(this.resource.Currency.Code);
            resultResource.Supplier.Id.Should().Be(this.resource.Supplier.Id);
            resultResource.Supplier.Name.Should().Be(this.resource.Supplier.Name);
            resultResource.SupplierContact.Should().Be(this.resource.SupplierContact);
            resultResource.AddressLine1.Should().Be(this.resource.AddressLine1);
            resultResource.AddressLine2.Should().Be(this.resource.AddressLine2);
            resultResource.AddressLine3.Should().Be(this.resource.AddressLine3);
            resultResource.AddressLine4.Should().Be(this.resource.AddressLine4);
            resultResource.PostCode.Should().Be(this.resource.PostCode);
            resultResource.AddressLine1.Should().Be(this.resource.AddressLine1);
            resultResource.Country.CountryCode.Should().Be(this.resource.Country.CountryCode);
            resultResource.PhoneNumber.Should().Be(this.resource.PhoneNumber);
            resultResource.QuoteRef.Should().Be(this.resource.QuoteRef);
            resultResource.Email.Should().Be(this.resource.Email);
            resultResource.DateRequired.Should().Be(null);
            resultResource.RequestedBy.Id.Should().Be(this.resource.RequestedBy.Id);
            resultResource.AuthorisedBy.Id.Should().Be(999);
            resultResource.SecondAuthBy.Should().Be(null);
            resultResource.FinanceCheckBy.Id.Should().Be(this.resource.FinanceCheckBy.Id);
            resultResource.TurnedIntoOrderBy.Should().Be(null);
            resultResource.Nominal.NominalCode.Should().Be(this.resource.Nominal.NominalCode);
            resultResource.RemarksForOrder.Should().Be(this.resource.RemarksForOrder);
            resultResource.InternalNotes.Should().Be(this.resource.InternalNotes);
            resultResource.Department.DepartmentCode.Should().Be(this.resource.Department.DepartmentCode);
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
