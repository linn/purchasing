namespace Linn.Purchasing.Integration.Tests.ChangeRequestModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Integration.Tests.Extensions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingPcasComponentChangesCsv : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.PcasChangeCompView.FilterBy(Arg.Any<Expression<Func<PcasChangeComponent, bool>>>())
                .Returns(new List<PcasChangeComponent>
                             {
                                 new PcasChangeComponent { Cref = "ABC" }
                             }.AsQueryable());

            this.Response = this.Client.Get(
                $"/purchasing/change-requests/pcas-component-changes?documentNumber=123",
                with => { with.Accept("text/csv"); }).Result;
        }

        [Test]
        public void ShouldReturnCsvContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("text/csv");
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}

