namespace Linn.Purchasing.Integration.Tests.SupplierModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenAddingPartSupplier : ContextBase
    {
        private PartSupplierResource createResource;

        private PartSupplier partSupplier;

        [SetUp]
        public void SetUp()
        {

            this.createResource = new PartSupplierResource
                                {
                                    CreatedBy = 33087,
                                    DateCreated = DateTime.Today.ToString("o"),
                                    PartNumber = "PART",
                                    SupplierId = 100,
                                    UnitOfMeasure = "NEW UOM",
                                    BaseOurUnitPrice = 100m,
                                    AddressId = 1,
                                    CurrencyCode = "USD",
                                    CurrencyUnitPrice = 150m,
                                    DamagesPercent = 2,
                                    DeliveryInstructions = "INSTR",
                                    Designation = "DESG",
                                    LeadTimeWeeks = 12,
                                    MadeInvalidBy = 33087,
                                    ManufacturerCode = "MAN",
                                    ManufacturerPartNumber = "M PART",
                                    MinimumDeliveryQty = 3m,
                                    MinimumOrderQty = 4m,
                                    NotesForBuyer = "NOTES",
                                    OrderIncrement = 5m,
                                    OrderMethodName = "METHOD",
                                    OurCurrencyPriceToShowOnOrder = 6m,
                                    ReelOrBoxQty = 7m,
                                    VendorPartNumber = "VPN"
                                };

            this.partSupplier = new PartSupplier
                                    {
                                        PartNumber = "PART",
                                        SupplierId = 100,
                                        Part = new Part { PartNumber = "PART" },
                                        Supplier = new Supplier { SupplierId = 100 }
                                    };

            this.Response = this.Client.PostAsJsonAsync(
                $"/purchasing/part-suppliers/record",
                this.createResource).Result;
        }

        [Test]
        public void ShouldPassCorrectDataToDomainService()
        {
            this.MockPartSupplierDomainService.Received()
                .CreatePartSupplier(
                Arg.Is<PartSupplier>(
                    u =>
                        u.UnitOfMeasure == this.createResource.UnitOfMeasure
                        && u.BaseOurUnitPrice == this.createResource.BaseOurUnitPrice
                        && u.DeliveryFullAddress.Id == this.createResource.AddressId
                        && u.Currency.Code == this.createResource.CurrencyCode
                        && u.CurrencyUnitPrice == this.createResource.CurrencyUnitPrice
                        && u.DamagesPercent == this.createResource.DamagesPercent
                        && u.DeliveryInstructions == this.createResource.DeliveryInstructions
                        && u.SupplierDesignation == this.createResource.Designation
                        && u.LeadTimeWeeks == this.createResource.LeadTimeWeeks
                        && u.CreatedBy.Id == this.createResource.CreatedBy
                        && u.Manufacturer.Code == this.createResource.ManufacturerCode
                        && u.ManufacturerPartNumber == this.createResource.ManufacturerPartNumber
                        && u.MinimumDeliveryQty == this.createResource.MinimumDeliveryQty
                        && u.MinimumOrderQty == this.createResource.MinimumOrderQty
                        && u.NotesForBuyer == this.createResource.NotesForBuyer
                        && u.OrderIncrement == this.createResource.OrderIncrement
                        && u.OrderMethod.Name == this.createResource.OrderMethodName
                        && u.OurCurrencyPriceToShowOnOrder == this.createResource.OurCurrencyPriceToShowOnOrder
                        && u.ReelOrBoxQty == this.createResource.ReelOrBoxQty
                        && u.VendorPartNumber == this.createResource.VendorPartNumber),
                Arg.Any<IEnumerable<string>>());
        }

        [Test]
        public void ShouldReturnCreated()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resultResource = this.Response.DeserializeBody<PartSupplierResource>();
            resultResource.Should().NotBeNull();
        }
    }
}
