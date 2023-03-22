namespace Linn.Purchasing.Facade.Tests.ChangeRequestFacadeServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSearchingChangeRequests : ContextBase
    {
        private IResult<IEnumerable<ChangeRequestResource>> result;

        [SetUp]
        public void SetUp()
        {

            this.Repository.FindAll()
                .Returns(new List<ChangeRequest>
                             {
                                 new ChangeRequest
                                     {
                                         DocumentNumber = 1,
                                         OldPartNumber = "SOUP",
                                         NewPartNumber = "TEST1",
                                         DateEntered = DateTime.Today,
                                         ChangeState = "ACCEPT"
                                     },
                                 new ChangeRequest
                                     {
                                         DocumentNumber = 2,
                                         OldPartNumber = "SOUP2",
                                         NewPartNumber = "TOAST2",
                                         DateEntered = DateTime.Today,
                                         ChangeState = "ACCEPT"
                                     }
                             }.AsQueryable());

            this.result = this.Sut.SearchChangeRequests("TEST*", true, null);
        }

        [Test]
        public void ShouldReturnSuccessRequest()
        {
            this.result.Should().BeOfType<SuccessResult<IEnumerable<ChangeRequestResource>>>();
        }

        [Test]
        public void ShouldReturnOneMatchingChangeRequest()
        {
            var resources = ((SuccessResult<IEnumerable<ChangeRequestResource>>)this.result).Data.ToList();
            resources.Count.Should().Be(1);
            var request = resources.First();
            request.DocumentNumber.Should().Be(1);
            request.NewPartNumber.Should().Be("TEST1");
        }
    }
}
