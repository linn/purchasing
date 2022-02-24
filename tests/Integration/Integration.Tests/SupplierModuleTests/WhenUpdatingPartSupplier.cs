namespace Linn.Purchasing.Integration.Tests.SupplierModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingPartSupplier : ContextBase
    {
        private PartSupplierResource updateResource;

        [SetUp]
        public void SetUp()
        {
            this.updateResource = new PartSupplierResource
                                      {
                                          PartNumber = "PART",
                                          SupplierId = 100,
                                          UnitOfMeasure = "NEW UOM",
                                          BaseOurUnitPrice = 100m,
                                          AddressId = 1,
                                          CurrencyCode = "USD",
                                          CurrencyUnitPrice = 150m,
                                          DamagesPercent = 2,
                                          DateInvalid = DateTime.UnixEpoch.ToString("o"),
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

            this.MockPartSupplierRepository
                .FindById(Arg.Is<PartSupplierKey>(
                    k => k.PartNumber == this.updateResource.PartNumber && k.SupplierId == this.updateResource.SupplierId))
                .Returns(
                    new PartSupplier
                        {
                            PartNumber = this.updateResource.PartNumber, SupplierId = this.updateResource.SupplierId,
                            Part = new Part { PartNumber = this.updateResource.PartNumber },
                            Supplier = new Supplier { SupplierId = this.updateResource.SupplierId }
                        });

            this.Response = this.Client.Put(
                $"/purchasing/part-suppliers/record?partId={1}&supplierId={100}",
                this.updateResource,
                with =>
                {
                    with.Accept("application/json");
                }).Result;
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldPassCorrectDataToDomainService()
        {
            this.MockPartSupplierDomainService.Received()
                .UpdatePartSupplier(
                Arg.Any<PartSupplier>(), 
                Arg.Is<PartSupplier>(
                    u => 
                        u.UnitOfMeasure == this.updateResource.UnitOfMeasure
                        && u.BaseOurUnitPrice == this.updateResource.BaseOurUnitPrice
                        && u.DeliveryFullAddress.Id == this.updateResource.AddressId
                        && u.Currency.Code == this.updateResource.CurrencyCode
                        && u.CurrencyUnitPrice == this.updateResource.CurrencyUnitPrice
                        && u.DamagesPercent == this.updateResource.DamagesPercent
                        && u.DateInvalid == DateTime.Parse(this.updateResource.DateInvalid)
                        && u.DeliveryInstructions == this.updateResource.DeliveryInstructions
                        && u.SupplierDesignation == this.updateResource.Designation
                        && u.LeadTimeWeeks == this.updateResource.LeadTimeWeeks
                        && u.MadeInvalidBy.Id == this.updateResource.MadeInvalidBy
                        && u.Manufacturer.Code == this.updateResource.ManufacturerCode
                        && u.ManufacturerPartNumber == this.updateResource.ManufacturerPartNumber
                        && u.MinimumDeliveryQty == this.updateResource.MinimumDeliveryQty
                        && u.MinimumOrderQty == this.updateResource.MinimumOrderQty
                        && u.NotesForBuyer == this.updateResource.NotesForBuyer
                        && u.OrderIncrement == this.updateResource.OrderIncrement
                        && u.OrderMethod.Name == this.updateResource.OrderMethodName
                        && u.OurCurrencyPriceToShowOnOrder == this.updateResource.OurCurrencyPriceToShowOnOrder
                        && u.ReelOrBoxQty == this.updateResource.ReelOrBoxQty
                        && u.VendorPartNumber == this.updateResource.VendorPartNumber), 
                Arg.Any<IEnumerable<string>>());
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
