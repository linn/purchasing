namespace Linn.Purchasing.Domain.LinnApps.Tests.CircuitBoardServiceTests
{
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected ICircuitBoardService Sut { get; private set; }
        
        protected IRepository<ChangeRequest, int> ChangeRequestRepository { get; private set; }
        
        protected IRepository<CircuitBoard, string> BoardRepository { get; private set; }
        
        protected IQueryRepository<Part> PartRepository { get; private set; }

        [SetUp]
        public void EstablishContext()
        {
            this.ChangeRequestRepository = Substitute.For<IRepository<ChangeRequest, int>>();
            this.BoardRepository = Substitute.For<IRepository<CircuitBoard, string>>();
            this.PartRepository = Substitute.For<IQueryRepository<Part>>();
            this.Sut = new CircuitBoardService(this.ChangeRequestRepository, this.BoardRepository, this.PartRepository);
        }
    }
}
