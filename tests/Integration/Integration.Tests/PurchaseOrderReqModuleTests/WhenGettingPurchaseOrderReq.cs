namespace Linn.Purchasing.Integration.Tests.PurchaseOrderReqModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
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

    public class WhenGettingPurchaseOrderReq : ContextBase
    {
        private readonly int reqNumber = 2023022;

        private PurchaseOrderReq req;

        [SetUp]
        public void SetUp()
        {
            this.req = new PurchaseOrderReq
                           {
                               ReqNumber = this.reqNumber,
                               State = "purgatory",
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
                               AuthorisedBy = null,
                               SecondAuthBy = null,
                               FinanceCheckBy = null,
                               TurnedIntoOrderBy = null,
                               Nominal = new Nominal { NominalCode = "00001234", Description = "hing" },
                               RemarksForOrder = "needed asap",
                               InternalNotes = "pls approv",
                               Department = new Department { DepartmentCode = "00002345", Description = "Team 1" }
                           };

            this.MockPurchaseOrderReqRepository.FindById(2023022).Returns(this.req);

            this.MockAuthService.HasPermissionFor(Arg.Any<string>(), Arg.Any<IEnumerable<string>>()).Returns(false);

            this.Response = this.Client.Get(
                $"/purchasing/purchase-orders/reqs/{this.reqNumber}",
                with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldBuildLinks()
        {
            var resource = this.Response.DeserializeBody<PurchaseOrderReqResource>();
            resource.Links.Single(x => x.Rel == "self").Href.Should()
                .Be($"/purchasing/purchase-orders/reqs/{this.reqNumber}");
            resource.Links.Single(x => x.Rel == "print").Href.Should()
                .Be($"/purchasing/purchase-orders/reqs/{this.reqNumber}/print");
        }

        [Test]
        public void ShouldCallRepo()
        {
            this.MockPurchaseOrderReqRepository.Received().FindById(2023022);
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resultResource = this.Response.DeserializeBody<PurchaseOrderReqResource>();
            resultResource.ReqNumber.Should().Be(this.reqNumber);
            resultResource.ReqDate.Should().Be(this.req.ReqDate.ToString("o"));
            resultResource.OrderNumber.Should().Be(this.req.OrderNumber);
            resultResource.PartNumber.Should().Be(this.req.PartNumber);
            resultResource.Description.Should().Be(this.req.Description);
            resultResource.Qty.Should().Be(this.req.Qty);
            resultResource.UnitPrice.Should().Be(this.req.UnitPrice);
            resultResource.Carriage.Should().Be(this.req.Carriage);
            resultResource.TotalReqPrice.Should().Be(this.req.TotalReqPrice);
            resultResource.Currency.Code.Should().Be(this.req.Currency.Code);
            resultResource.Supplier.Id.Should().Be(this.req.Supplier.SupplierId);
            resultResource.Supplier.Name.Should().Be(this.req.Supplier.Name);
            resultResource.SupplierContact.Should().Be(this.req.SupplierContact);
            resultResource.AddressLine1.Should().Be(this.req.AddressLine1);
            resultResource.AddressLine2.Should().Be(this.req.AddressLine2);
            resultResource.AddressLine3.Should().Be(this.req.AddressLine3);
            resultResource.AddressLine4.Should().Be(this.req.AddressLine4);
            resultResource.PostCode.Should().Be(this.req.PostCode);
            resultResource.AddressLine1.Should().Be(this.req.AddressLine1);
            resultResource.Country.CountryCode.Should().Be(this.req.Country.CountryCode);
            resultResource.PhoneNumber.Should().Be(this.req.PhoneNumber);
            resultResource.QuoteRef.Should().Be(this.req.QuoteRef);
            resultResource.Email.Should().Be(this.req.Email);
            resultResource.DateRequired.Should().Be(null);
            resultResource.RequestedBy.Id.Should().Be(this.req.RequestedBy.Id);
            resultResource.AuthorisedBy.Should().Be(null);
            resultResource.SecondAuthBy.Should().Be(null);
            resultResource.FinanceCheckBy.Should().Be(null);
            resultResource.TurnedIntoOrderBy.Should().Be(null);
            resultResource.Nominal.NominalCode.Should().Be(this.req.Nominal.NominalCode);
            resultResource.RemarksForOrder.Should().Be(this.req.RemarksForOrder);
            resultResource.InternalNotes.Should().Be(this.req.InternalNotes);
            resultResource.Department.DepartmentCode.Should().Be(this.req.Department.DepartmentCode);
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
