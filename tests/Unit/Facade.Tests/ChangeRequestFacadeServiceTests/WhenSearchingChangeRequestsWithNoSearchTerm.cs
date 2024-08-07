﻿namespace Linn.Purchasing.Facade.Tests.ChangeRequestFacadeServiceTests
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

    public class WhenSearchingChangeRequestsWithNoSearchTerm : ContextBase
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
                                         NewPartNumber = "TEST1",
                                         DateEntered = DateTime.Today,
                                         ChangeState = "ACCEPT"
                                     },
                                 new ChangeRequest
                                     {
                                         DocumentNumber = 2,
                                         NewPartNumber = "TOAST2",
                                         DateEntered = DateTime.Today,
                                         ChangeState = "ACCEPT"
                                     }
                             }.AsQueryable());

            this.result = this.Sut.SearchChangeRequests(string.Empty, true, null, false);
        }

        [Test]
        public void ShouldReturnSuccessRequest()
        {
            this.result.Should().BeOfType<SuccessResult<IEnumerable<ChangeRequestResource>>>();
        }

        [Test]
        public void ShouldReturnAllMatchingChangeRequests()
        {
            var resources = ((SuccessResult<IEnumerable<ChangeRequestResource>>)this.result).Data;
            resources.Count().Should().Be(2);
        }
    }
}
