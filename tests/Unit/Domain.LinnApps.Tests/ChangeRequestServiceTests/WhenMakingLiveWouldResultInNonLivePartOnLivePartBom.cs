namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenMakingLiveWouldResultInNonLivePartOnLivePartBom : ContextBase
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            var request = new ChangeRequest
            {
                DocumentNumber = 1,
                BomChanges = new List<BomChange>
                                                   {
                                                       new BomChange
                                                           {
                                                               BomId = 123,
                                                               BomName = "NEW PRODUCT 8",
                                                               AddedBomDetails = new List<BomDetail>
                                                                   {
                                                                       new BomDetail
                                                                           {
                                                                               PartNumber = "PACK 117",
                                                                               Part = new Part { DateLive = null }
                                                                           }
                                                                   }
                                                           }
                                                   },
                ChangeState = "ACCEPT",
                DateEntered = new DateTime(2022, 1, 1),
                DescriptionOfChange = "Brilliant Change Request"
            };
            this.Repository.FindById(1).Returns(request);

            var employee = new Employee { Id = 7, FullName = "Dougie Howser" };
            this.EmployeeRepository.FindById(7).Returns(employee);

            this.AuthService
                .HasPermissionFor(AuthorisedAction.MakeLiveChangeRequest, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.BomDetailRepository.FilterBy(Arg.Any<Expression<Func<BomDetail, bool>>>()).Returns(
               new List<BomDetail>().AsQueryable());

            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>()).Returns(
                new Part { PartNumber = "NEW PRODUCT 8", DateLive = 28.March(2023) });
            this.action = () => this.Sut.MakeLive(1, 7, null, null, new List<string>());
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<InvalidBomChangeException>()
                .WithMessage("Cannot add NON-LIVE PACK 117 onto BOM of NEW PRODUCT 8, which IS LIVE!!");
        }
    }
}
