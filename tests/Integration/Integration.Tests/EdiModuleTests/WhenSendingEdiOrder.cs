namespace Linn.Purchasing.Integration.Tests.EdiModuleTests
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSendingEdiOrder : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            var request = new SendEdiEmailResource
                              {
                                  supplierId = 41193,
                                  altEmail = "boris@example.com",
                                  additionalEmail = "carrie@example.com",
                                  additionalText = "Put out the cat",
                                  test = false
                              };

            var result = new ProcessResult {Success = true, Message = "Test"};
            this.MockDomainService.SendEdiOrder(41193, "boris@example.com", "carrie@example.com", "Put out the cat", false)
                .Returns(result);

            this.Response = this.Client.PostAsJsonAsync(
                "/purchasing/edi/orders",
                request).Result;
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldCallDomainService()
        {
            this.MockDomainService.SendEdiOrder(41193, "boris@example.com", "carrie@example.com", "Put out the cat", false);
        }
        
        [Test]
        public void ShouldReturnJsonBody()
        {
            var resource = this.Response.DeserializeBody<ProcessResultResource>();
            resource.Should().NotBeNull();
        }
        
    }
}
