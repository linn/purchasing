namespace Linn.Purchasing.Domain.LinnApps.Tests.PartSupplierServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;

    using NSubstitute;
    using NSubstitute.ReturnsExtensions;

    using NUnit.Framework;

    public class WhenBulkUpdatingLeadTimesAndPartSupplierRecordNotFound : ContextBase
    {
        private ProcessResult result;

        [SetUp]
        public void SetUp()
        {
            this.MockAuthService
                .HasPermissionFor(AuthorisedAction.PartSupplierUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.PartSupplierRepository.FindBy(Arg.Any<Expression<Func<PartSupplier, bool>>>())
                .ReturnsNull();

            this.result = this.Sut.BulkUpdateLeadTimes(
                1,
                new List<LeadTimeUpdateModel> { new LeadTimeUpdateModel("PART", "1") },
                new List<string>());
        }

        [Test]
        public void ShouldReturnError()
        {
            this.result.Success.Should().BeFalse();
            this.result.Message.Should().Be(
                "0 out of 1 records updated successfully. Updates for the following parts could not be processed: PART, ");
        }
    }
}
