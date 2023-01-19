namespace Linn.Purchasing.Facade.Tests.ChangeRequestFacadeServiceTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;

    using NUnit.Framework;

    public class WhenPhasingChangeRequestInWithNoPhaseInWeek : ContextBase
    {
        private IResult<ChangeRequestResource> result;

        [SetUp]
        public void SetUp()
        {
            var phaseInRequest = new ChangeRequestPhaseInsResource
                                     {
                                         DocumentNumber = 1,
                                         SelectedBomChangeIds = new List<int> { 1 }
                                     };
            this.result = this.Sut.PhaseInChangeRequest(phaseInRequest, null);
        }

        [Test]
        public void ShouldReturnBadRequest()
        {
            this.result.Should().BeOfType<BadRequestResult<ChangeRequestResource>>();
            ((BadRequestResult<ChangeRequestResource>)this.result).Message.Should().Be("No phase in week supplied");
        }
    }
}
