namespace Linn.Purchasing.Facade.Tests.ChangeRequestFacadeServiceTests
{
    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;

    using NUnit.Framework;

    public class WhenChangingStatusToUnknownStatus : ContextBase
    {
        private IResult<ChangeRequestResource> result;

        [SetUp]
        public void SetUp()
        {
            var request = new ChangeRequestStatusChangeResource
                              {
                                  Id = 1,
                                  Status = "SLEEPY"
                              };
            this.result = this.Sut.ChangeStatus(request, 1);
        }

        [Test]
        public void ShouldReturnBadRequest()
        {
            this.result.Should().BeOfType<BadRequestResult<ChangeRequestResource>>();
            ((BadRequestResult<ChangeRequestResource>)this.result).Message.Should().Be("Cannot change status to SLEEPY");
        }
    }
}
