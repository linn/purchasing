﻿namespace Linn.Purchasing.Integration.Tests.PurchaseOrderModuleTests
{
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Integration.Tests.Extensions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSendingPdfEmail : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.MockDomainService.SendPdfEmail(
                Arg.Any<string>(),
                Arg.Any<int>(),
                false,
                Arg.Any<int>())
                .Returns(new ProcessResult(true, "email sent"));

            this.MockPurchaseOrderRepository.FindById(158962).Returns(
                new PurchaseOrder
                    {
                        OrderNumber = 158962, OverbookQty = 1, Supplier = new Supplier { SupplierId = 1224 }
                    });

            this.Response = this.Client.Post(
                "/purchasing/purchase-orders/email-pdf?bcc=false&emailAddress=iain.crawford@linn.co.uk&orderNumber=158962",
                with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resource = this.Response.DeserializeBody<ProcessResultResource>();
            resource.Should().NotBeNull();
            resource.Success.Should().BeTrue();
            resource.Message.Should().Be("email sent");
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldCallDomain()
        {
            this.MockDomainService.Received().SendPdfEmail(
                Arg.Any<string>(),
                Arg.Any<int>(),
                false,
                Arg.Any<int>());
        }
    }
}
