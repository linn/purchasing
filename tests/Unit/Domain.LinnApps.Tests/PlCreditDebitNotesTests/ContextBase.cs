namespace Linn.Purchasing.Domain.LinnApps.Tests.PlCreditDebitNotesTests
{
    using Linn.Common.Authorisation;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IPlCreditDebitNoteService Sut { get; private set; }

        protected IAuthorisationService MockAuthService { get; private set;  }

        [SetUp]
        public void SetUpContext()
        {
            this.MockAuthService = Substitute.For<IAuthorisationService>();
            this.Sut = new PlCreditDebitNoteService(this.MockAuthService);
        }
    }
}
