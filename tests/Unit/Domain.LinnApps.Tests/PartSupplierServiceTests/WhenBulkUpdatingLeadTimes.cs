namespace Linn.Purchasing.Domain.LinnApps.Tests.PartSupplierServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenBulkUpdatingLeadTimes : ContextBase
    {
        private ProcessResult result;

        [SetUp]
        public void SetUp()
        {
            this.MockAuthService
                .HasPermissionFor(AuthorisedAction.PartSupplierUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.PartSupplierRepository.FilterBy(Arg.Any<Expression<Func<PartSupplier, bool>>>())
                .Returns(new List<PartSupplier>
                             {
                                 new PartSupplier 
                                     { 
                                         PartNumber = "PART"
                                     }
                             }.AsQueryable());

            this.result = this.Sut.BulkUpdateLeadTimes(
                1,
                new List<LeadTimeUpdateModel>
                    {
                        new LeadTimeUpdateModel("PART 1", "1"),
                        new LeadTimeUpdateModel("PART 2", "1"),
                        new LeadTimeUpdateModel("PART 3", "1")
                    },
                new List<string>());
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.result.Success.Should().BeTrue();
            this.result.Message.Should().Be(
                "3 records updated successfully");
        }
    }
}
