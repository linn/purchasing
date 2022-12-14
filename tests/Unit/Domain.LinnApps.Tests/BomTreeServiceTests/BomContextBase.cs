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

    public class BomContextBase
    {
        protected IRepository<Bom, int> Repository { get; private set; }

        protected IBomDetailRepository BomDetailRepository { get; private set; }

        protected IBomTreeService Sut { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.Repository = Substitute.For<IRepository<Bom, int>>();
            this.BomDetailRepository = Substitute.For<IBomDetailRepository>();
            this.MockData();
            this.Sut = new BomTreeService(this.Repository, this.BomDetailRepository);
        }

        protected void MockData()
        {
            var addChange = new BomChange { DocumentNumber = 123 };
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
            this.Repository.FindBy(Arg.Any<Expression<Func<Bom, bool>>>()).Returns(
                new Bom
                {
                    BomName = "root",
                    Details = new List<BomDetailViewEntry>
                                      {
                                          new BomDetailViewEntry { ChangeState = "LIVE", Part = new Part { PartNumber = "n1" }, AddChange = addChange},
                                          new BomDetailViewEntry { ChangeState = "LIVE", Part = new Part { PartNumber = "n2" }, AddChange = addChange },
                                          new BomDetailViewEntry { ChangeState = "LIVE", Part = new Part { PartNumber = "n3" }, AddChange = addChange }
                                      }
                });

            var n1Children = new List<BomDetailViewEntry>
                                 {
                                     new BomDetailViewEntry { ChangeState = "LIVE", Part = new Part { PartNumber = "n11" }, AddChange = addChange },
                                     new BomDetailViewEntry { ChangeState = "LIVE", Part = new Part { PartNumber = "n12" }, AddChange = addChange },
                                     new BomDetailViewEntry { ChangeState = "LIVE", Part = new Part { PartNumber = "n13" }, AddChange = addChange }
                                 };

            var n11Children = new List<BomDetailViewEntry>
                                  {
                                      new BomDetailViewEntry { ChangeState = "LIVE", Part = new Part { PartNumber = "n111" }, AddChange = addChange },
                                      new BomDetailViewEntry { ChangeState = "LIVE", Part = new Part { PartNumber = "n112" }, AddChange = addChange }
                                  };

            var n13Children = new List<BomDetailViewEntry>
                                  {
                                      new BomDetailViewEntry { ChangeState = "LIVE", Part = new Part { PartNumber = "n131" }, AddChange = addChange }
                                  };

            var n111Children = new List<BomDetailViewEntry>
                                  {
                                      new BomDetailViewEntry { ChangeState = "LIVE", Part = new Part { PartNumber = "n1111" }, AddChange = addChange }
                                  };

            var n112Children = new List<BomDetailViewEntry>
                                   {
                                       new BomDetailViewEntry { ChangeState = "LIVE", Part = new Part { PartNumber = "n1121" }, AddChange = addChange }
                                   };

            this.BomDetailRepository.FilterBy(Arg.Any<Expression<Func<BomDetailViewEntry, bool>>>()).Returns(
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
