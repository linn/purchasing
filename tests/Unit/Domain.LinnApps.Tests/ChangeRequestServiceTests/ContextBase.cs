namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestServiceTests
{
    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NSubstitute;
    using NUnit.Framework;

    public class ContextBase
    {
        protected IChangeRequestService Sut { get; private set; }

        protected IRepository<ChangeRequest, int> Repository { get; private set; }

        protected IAuthorisationService AuthService { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.Repository = Substitute.For<IRepository<ChangeRequest, int>>();
            this.AuthService = Substitute.For<IAuthorisationService>();
            this.Sut = new ChangeRequestService(
                this.AuthService,
                this.Repository);
        }
    }
}
