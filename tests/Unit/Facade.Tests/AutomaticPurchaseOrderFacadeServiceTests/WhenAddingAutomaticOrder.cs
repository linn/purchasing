namespace Linn.Purchasing.Facade.Tests.AutomaticPurchaseOrderFacadeServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.AutomaticPurchaseOrders;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenAddingAutomaticOrder : ContextBase
    {
        private IResult<AutomaticPurchaseOrderResource> result;

        private AutomaticPurchaseOrderResource resource;

        private AutomaticPurchaseOrderDetailResource detail;

        [SetUp]
        public void SetUp()
        {
            this.detail = new AutomaticPurchaseOrderDetailResource
                              {
                                  Id = 0,
                                  Sequence = 0,
                                  PartNumber = "P1",
                                  SupplierId = 123,
                                  OrderNumber = 484935,
                                  Quantity = 5.4m,
                                  QuantityRecommended = 5.4m,
                                  RecommendationCode = "WHAT",
                                  OrderLog = null,
                                  CurrencyCode = "USD",
                                  CurrencyPrice = 56.45m,
                                  BasePrice = 0,
                                  RequestedDate = 1.June(2050).ToString("o"),
                                  OrderMethod = "METHOD",
                                  IssuePartsToSupplier = null,
                                  IssueSerialNumbers = null
                              };
            this.resource = new AutomaticPurchaseOrderResource
                                {
                                    Id = 0,
                                    StartedBy = 123,
                                    JobRef = "ABC",
                                    DateRaised = null,
                                    SupplierId = null,
                                    Planner = null,
                                    Details = new List<AutomaticPurchaseOrderDetailResource>
                                                  {
                                                      this.detail
                                                  }
                                };
            this.AutomaticPurchaseOrderService.CreateAutomaticPurchaseOrder(Arg.Any<AutomaticPurchaseOrder>())
                .Returns(new AutomaticPurchaseOrder { Id = 123 });
            this.result = this.Sut.Add(this.resource, new List<string>());
        }

        [Test]
        public void ShouldCallService()
        {
            this.AutomaticPurchaseOrderService.Received().CreateAutomaticPurchaseOrder(Arg.Is<AutomaticPurchaseOrder>(
                    a => a.JobRef == this.resource.JobRef && a.Details.First().PartNumber == this.detail.PartNumber
                                                          && a.Details.First().CurrencyCode == this.detail.CurrencyCode
                                                          && a.Details.First().Quantity == this.detail.Quantity
                                                          && a.Details.First().SupplierId == this.detail.SupplierId
                                                          && a.Details.First().RequestedDate == DateTime.Parse(this.detail.RequestedDate)));
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.result.Should().BeOfType<CreatedResult<AutomaticPurchaseOrderResource>>();
            var dataResult = ((CreatedResult<AutomaticPurchaseOrderResource>)this.result).Data;
            dataResult.Id.Should().Be(123);
        }
    }
}
