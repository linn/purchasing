namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderReqServiceTests
{
    using System.Collections.Generic;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdating : ContextBase
    {
        private readonly int reqNumber = 5678;

        private PurchaseOrderReq current;

        private PurchaseOrderReq updated;

        [SetUp]
        public void SetUp()
        {
            this.current = new PurchaseOrderReq { ReqNumber = this.reqNumber };
            this.updated = new PurchaseOrderReq
                               {
                                   ReqNumber = this.reqNumber,
                                   State = "purgatory",
                                   ReqDate = 2.March(2022),
                                   OrderNumber = 1234,
                                   PartNumber = "PCAS 007",
                                   PartDescription = "Descrip",
                                   Qty = 7,
                                   UnitPrice = 8m,
                                   Carriage = 99m,
                                   TotalReqPrice = 118m,
                                   Currency = new Currency { Code = "SMC", Name = "Smackeroonies" },
                                   Supplier = new Supplier { SupplierId = 111, Name = "Shoap" },
                                   SupplierContact = "Lawrence Chaney",
                                   AddressLine1 = "The shop",
                                   AddressLine2 = "1 Main Street",
                                   AddressLine3 = "town centre",
                                   AddressLine4 = "Glesga",
                                   PostCode = "G1 1AA",
                                   Country = new Country { CountryCode = "GB", Name = "United Kingdolls" },
                                   PhoneNumber = "+44 1234567780",
                                   QuoteRef = "blah",
                                   Email = "LC@gmail",
                                   DateRequired = 1.January(2023),
                                   RequestedBy = new Employee { Id = 33107, FullName = "me" },
                                   AuthorisedBy = new Employee { Id = 123, FullName = "not me" },
                                   SecondAuthBy = new Employee { Id = 234, FullName = "big dog" },
                                   FinanceCheckBy = new Employee { Id = 999, FullName = "finance person" },
                                   TurnedIntoOrderBy = new Employee { Id = 876, FullName = "some one" },
                                   Nominal = "dono",
                                   RemarksForOrder = "needed asap",
                                   InternalNotes = "pls approv",
                                   Department = "Team 1"
                               };
            this.MockAuthService.HasPermissionFor(AuthorisedAction.PurchaseOrderReqUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.Sut.Update(this.current, this.updated, new List<string>());
        }

        [Test]
        public void ShouldUpdate()
        {
            this.current.ReqNumber.Should().Be(this.reqNumber);
            this.current.ReqDate.Should().Be(this.updated.ReqDate);
            this.current.OrderNumber.Should().Be(this.updated.OrderNumber);
            this.current.PartNumber.Should().Be(this.updated.PartNumber);
            this.current.PartDescription.Should().Be(this.updated.PartDescription);
            this.current.Qty.Should().Be(this.updated.Qty);
            this.current.UnitPrice.Should().Be(this.updated.UnitPrice);
            this.current.Carriage.Should().Be(this.updated.Carriage);
            this.current.TotalReqPrice.Should().Be(this.updated.TotalReqPrice);
            this.current.Currency.Code.Should().Be(this.updated.Currency.Code);
            this.current.Supplier.SupplierId.Should().Be(this.updated.Supplier.SupplierId);
            this.current.Supplier.Name.Should().Be(this.updated.Supplier.Name);
            this.current.SupplierContact.Should().Be(this.updated.SupplierContact);
            this.current.AddressLine1.Should().Be(this.updated.AddressLine1);
            this.current.AddressLine2.Should().Be(this.updated.AddressLine2);
            this.current.AddressLine3.Should().Be(this.updated.AddressLine3);
            this.current.AddressLine4.Should().Be(this.updated.AddressLine4);
            this.current.PostCode.Should().Be(this.updated.PostCode);
            this.current.AddressLine1.Should().Be(this.updated.AddressLine1);
            this.current.Country.CountryCode.Should().Be(this.updated.Country.CountryCode);
            this.current.PhoneNumber.Should().Be(this.updated.PhoneNumber);
            this.current.QuoteRef.Should().Be(this.updated.QuoteRef);
            this.current.Email.Should().Be(this.updated.Email);
            this.current.DateRequired.Should().Be(this.updated.DateRequired);
            this.current.RequestedBy.Id.Should().Be(this.updated.RequestedBy.Id);
            this.current.AuthorisedBy.Id.Should().Be(this.current.AuthorisedBy.Id);
            this.current.SecondAuthBy.Id.Should().Be(this.current.SecondAuthBy.Id);
            this.current.FinanceCheckBy.Id.Should().Be(this.current.FinanceCheckBy.Id);
            this.current.TurnedIntoOrderBy.Id.Should().Be(this.current.TurnedIntoOrderBy.Id);
            this.current.Nominal.Should().Be(this.updated.Nominal);
            this.current.RemarksForOrder.Should().Be(this.updated.RemarksForOrder);
            this.current.InternalNotes.Should().Be(this.updated.InternalNotes);
            this.current.Department.Should().Be(this.updated.Department);
        }
    }
}
