﻿namespace Linn.Purchasing.Integration.Tests.PurchaseOrderModuleTests
{
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSendingDeptEmail : ContextBase
    {
        private int orderNumber;

        [SetUp]
        public void SetUp()
        {
            this.orderNumber = 123456;
            this.MockDomainService.EmailDept(this.orderNumber, Arg.Any<int>())
                .Returns(new ProcessResult(true, "email sent"));

            this.Response = this.Client.Post(
                $"/purchasing/purchase-orders/{this.orderNumber}/email-dept",
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
            this.MockDomainService.Received().EmailDept(this.orderNumber, Arg.Any<int>());
        }
    }
}
