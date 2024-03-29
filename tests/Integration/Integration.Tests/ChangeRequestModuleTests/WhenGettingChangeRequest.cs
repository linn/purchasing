﻿namespace Linn.Purchasing.Integration.Tests.ChangeRequestModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingChangeRequest : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            var changeRequest = new ChangeRequest
                             {
                                 DocumentType = "CRF",
                                 DocumentNumber = 1,
                                 DateEntered = new DateTime(2020, 1, 1),
                                 EnteredBy = new Employee { Id = 1, FullName = "David Bowie" },
                                 ReasonForChange = "Everything changes",
                                 DescriptionOfChange = "Ch ch changes",
                                 ChangeState = "ACCEPT",
                                 BomChanges = new List<BomChange>
                                                  {
                                                      new BomChange
                                                          {
                                                              ChangeId = 1,
                                                              BomName = "TOAST 001",
                                                              ChangeState = "ACCEPT",
                                                              AddedBomDetails = new List<BomDetail>
                                                                  {
                                                                      new BomDetail
                                                                          {
                                                                              DetailId = 1,
                                                                              PartNumber = "BUTTER 1",
                                                                              Qty = 1,
                                                                              GenerateRequirement = "N",
                                                                              AddChangeId = 1,
                                                                              AddReplaceSeq = 1,
                                                                              Part = new Part { DateLive = DateTime.Today }
                                                                          }
                                                                  },
                                                              DeletedBomDetails = new List<BomDetail>
                                                                  {
                                                                      new BomDetail
                                                                          {
                                                                              DetailId = 1,
                                                                              PartNumber = "BUTTER 2",
                                                                              Qty = 2,
                                                                              GenerateRequirement = "Y",
                                                                              DeleteChangeId = 1,
                                                                              DeleteReplaceSeq = 1
                                                                          }
                                                                  }
                                                          }
                                                  },
                                 PcasChanges = new List<PcasChange>
                                                  {
                                                      new PcasChange
                                                          {
                                                              ChangeId = 1,
                                                              BoardCode = "TOAST",
                                                              RevisionCode = "BREAD",
                                                              ChangeState = "ACCEPT"
                                                          }
                                                  }
            };
            this.Repository.FindById(1).Returns(changeRequest);

            this.Response = this.Client.Get(
                "/purchasing/change-requests/1",
                with => { with.Accept("application/json"); }).Result;
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
        public void ShouldCallRepository()
        {
            this.Repository.Received().FindById(1);
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var result = this.Response.DeserializeBody<ChangeRequestResource>();
            result.Should().NotBeNull();
            result.DocumentNumber.Should().Be(1);
            result.ChangeState.Should().Be("ACCEPT");
            result.BomChanges.Count().Should().Be(1);
            result.PcasChanges.Count().Should().Be(1);

            var bomChange = result.BomChanges.First();
            bomChange.Should().NotBeNull();
            bomChange.BomChangeDetails.Count().Should().Be(1);
        }
    }
}
