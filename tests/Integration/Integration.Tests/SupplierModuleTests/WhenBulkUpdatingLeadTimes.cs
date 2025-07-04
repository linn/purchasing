﻿namespace Linn.Purchasing.Integration.Tests.SupplierModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenBulkUpdatingLeadTimes : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.MockPartSupplierDomainService.BulkUpdateLeadTimes(
                    1,
                    Arg.Any<IEnumerable<LeadTimeUpdateModel>>(), 
                    Arg.Any<IEnumerable<string>>(),
                    null)
                .Returns(new BatchUpdateProcessResult
                             {
                                 Success = true,
                                 Message = "success",
                                 Errors = new List<Error>
                                              {
                                                  new Error("PART", null)
                                              }
                             });

            this.Response = this.Client.Post(
                $"/purchasing/suppliers/bulk-lead-times?supplierId=1",
                "PART, 8",
                with =>
                    {
                        with.Accept("application/json");
                    }).Result;
        }

        [Test]
        public void ShouldCallDomainService()
        {
            this.MockPartSupplierDomainService.Received()
                .BulkUpdateLeadTimes(
                    1,
                    Arg.Any<IEnumerable<LeadTimeUpdateModel>>(),
                    Arg.Any<IEnumerable<string>>(),
                    null);
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldCommitChanges()
        {
            this.TransactionManager.Received()
                .Commit();
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
            var resultResource = this.Response.DeserializeBody<BatchUpdateProcessResultResourceWithLinks>();
            resultResource.Success.Should().Be(true);
            resultResource.Message.Should().Be("success");
            resultResource.Errors.First().Descriptor.Should().Be("PART");
        }
    }
}
