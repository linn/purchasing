namespace Linn.Purchasing.Integration.Tests.MaterialRequirementsModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources.MaterialRequirements;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingMrReportOptions : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.MaterialRequirementsReportFacadeService.GetOptions(Arg.Any<IEnumerable<string>>()).Returns(
                new SuccessResult<MrReportOptionsResource>(new MrReportOptionsResource
                                                               {
                                                                   PartSelectorOptions = new List<ReportOptionResource>
                                                                       {
                                                                           new ReportOptionResource
                                                                               {
                                                                                   DisplayText = "Option 1",
                                                                                   Option = "1"
                                                                               }
                                                                       }
                                                               }));
            this.Response = this.Client.Get(
                $"/purchasing/material-requirements/options",
                with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldCallFacadeService()
        {
            this.MaterialRequirementsReportFacadeService.Received().GetOptions(Arg.Any<IEnumerable<string>>());
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
            var resource = this.Response.DeserializeBody<MrReportOptionsResource>();
            resource.PartSelectorOptions.Should().HaveCount(1);
            resource.PartSelectorOptions.First().DisplayText.Should().Be("Option 1");
            resource.PartSelectorOptions.First().Option.Should().Be("1");
        }
    }
}
