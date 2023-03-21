namespace Linn.Purchasing.Integration.Tests.ChangeRequestModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingPcasComponentChangesCsv : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.PcasChangeCompView.FilterBy(Arg.Any<Expression<Func<PcasChangeComponent, bool>>>())
                .Returns(new List<PcasChangeComponent>
                             {
                                 new PcasChangeComponent { Cref = "ABC" }
                             }.AsQueryable());
        }

        // todo
    }
}

