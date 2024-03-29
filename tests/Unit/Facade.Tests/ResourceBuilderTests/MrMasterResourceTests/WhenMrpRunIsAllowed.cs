﻿namespace Linn.Purchasing.Facade.Tests.ResourceBuilderTests.MrMasterResourceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Resources.MaterialRequirements;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenMrpRunIsAllowed : ContextBase
    {
        private MrMaster master;

        private MrMasterResource result;

        [SetUp]
        public void SetUp()
        {
            this.master = new MrMaster { JobRef = "abc", RunDate = 1.May(2030), RunLogIdCurrentlyInProgress = null };
            this.AuthService.HasPermissionFor(AuthorisedAction.MrpRun, Arg.Any<IEnumerable<string>>())
                .Returns(true);
            this.result = (MrMasterResource)this.Sut.Build(this.master, new List<string>());
        }

        [Test]
        public void ShouldPopulateFields()
        {
            this.result.JobRef.Should().Be(this.master.JobRef);
            this.result.RunDate.Should().Be(this.master.RunDate.ToString("o"));
            this.result.RunLogIdCurrentlyInProgress.Should().Be(this.master.RunLogIdCurrentlyInProgress);
        }

        [Test]
        public void ShouldReturnCorrectLinks()
        {
            this.result.Links.Length.Should().Be(3);
            this.result.Links.First(a => a.Rel == "self").Href.Should()
                .Be("/purchasing/material-requirements/last-run");
            this.result.Links.First(a => a.Rel == "last-run-log").Href.Should()
                .Be("/purchasing/material-requirements/run-logs?jobRef=abc");
            this.result.Links.First(a => a.Rel == "run-mrp").Href.Should()
                .Be("/purchasing/material-requirements/run-mrp");
        }
    }
}
