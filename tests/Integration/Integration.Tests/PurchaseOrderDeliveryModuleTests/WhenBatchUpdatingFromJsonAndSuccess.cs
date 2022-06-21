namespace Linn.Purchasing.Integration.Tests.PurchaseOrderDeliveryModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Common.Facade.Carter.Serialisers;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NUnit.Framework;

    public class WhenBatchUpdatingFromJsonAndSuccess : ContextBase
    {
        private IEnumerable<PurchaseOrderDeliveryUpdateResource> resource;

        [SetUp]
        public void SetUp()
        {
            this.resource = new List<PurchaseOrderDeliveryUpdateResource>
                                {
                                    new PurchaseOrderDeliveryUpdateResource
                                        {
                                            OrderNumber = 1, 
                                            OrderLine = 1, 
                                            DeliverySequence = 1, 
                                            DateRequested = DateTime.Now
                                        }
                                };
          
            this.Response = this.Client.Post(
                $"/purchasing/purchase-orders/deliveries",
                this.resource,
                with =>
                    {
                        with.Accept("application/json");
                    },
                "application/json").Result;
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
