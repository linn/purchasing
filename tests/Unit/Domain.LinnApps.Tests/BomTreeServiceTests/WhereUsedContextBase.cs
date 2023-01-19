namespace Linn.Purchasing.Domain.LinnApps.Tests.BomTreeServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NSubstitute;

    using NUnit.Framework;

    public class WhereUsedContextBase
    {
        protected IRepository<Bom, int> Repository { get; private set; }

        protected IBomDetailViewRepository BomDetailViewRepository { get; private set; }

        protected IBomTreeService Sut { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.Repository = Substitute.For<IRepository<Bom, int>>();
            this.BomDetailViewRepository = Substitute.For<IBomDetailViewRepository>();
            this.MockData();
            this.Sut = new BomTreeService(this.Repository, this.BomDetailViewRepository);
        }

        protected void MockData()
        {
            /* build a tree structure that looks like the following:
                   __root__
                  /    \   \
                _n1_    n2  n3
               /  |  \        
             n11 n12 n13     
            /   \      \
          n111   n112   n131
          /       |
        n1111      n1121
         
        */
            var rootChildren = new List<BomDetailViewEntry>
                                      {
                                          new BomDetailViewEntry { ChangeState = "LIVE", BomPart = new Part { PartNumber = "n1" } },
                                          new BomDetailViewEntry { ChangeState = "LIVE", BomPart = new Part { PartNumber = "n2" } },
                                          new BomDetailViewEntry { ChangeState = "LIVE", BomPart = new Part { PartNumber = "n3" } }
                                      };

            var n1Children = new List<BomDetailViewEntry>
                                 {
                                     new BomDetailViewEntry { ChangeState = "LIVE", BomPart = new Part { PartNumber = "n11" } },
                                     new BomDetailViewEntry { ChangeState = "LIVE", BomPart = new Part { PartNumber = "n12" } },
                                     new BomDetailViewEntry { ChangeState = "LIVE", BomPart = new Part { PartNumber = "n13" } }
                                 };

            var n11Children = new List<BomDetailViewEntry>
                                  {
                                      new BomDetailViewEntry { ChangeState = "LIVE", BomPart = new Part { PartNumber = "n111" } },
                                      new BomDetailViewEntry { ChangeState = "LIVE", BomPart = new Part { PartNumber = "n112" } }
                                  };

            var n13Children = new List<BomDetailViewEntry>
                                  {
                                      new BomDetailViewEntry { ChangeState = "LIVE", BomPart = new Part { PartNumber = "n131" } }
                                  };

            var n111Children = new List<BomDetailViewEntry>
                                  {
                                      new BomDetailViewEntry { ChangeState = "LIVE", BomPart = new Part { PartNumber = "n1111" } }
                                  };

            var n112Children = new List<BomDetailViewEntry>
                                   {
                                       new BomDetailViewEntry { ChangeState = "LIVE", BomPart = new Part { PartNumber = "n1121" } }
                                   };

            this.BomDetailViewRepository.FilterBy(Arg.Any<Expression<Func<BomDetailViewEntry, bool>>>()).Returns(
                rootChildren.AsQueryable(),
                n1Children.AsQueryable(),
                new List<BomDetailViewEntry>().AsQueryable(), // n12 has no children
                new List<BomDetailViewEntry>().AsQueryable(), // n3 has no children
                n11Children.AsQueryable(),
                new List<BomDetailViewEntry>().AsQueryable(), // etc,
                n13Children.AsQueryable(),
                n111Children.AsQueryable(),
                n112Children.AsQueryable(),
                new List<BomDetailViewEntry>().AsQueryable());
        }
    }
}
