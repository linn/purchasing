namespace Linn.Purchasing.Domain.LinnApps.Tests.BomStandardPriceServiceTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSettingStandardPrices : ContextBase
    {
        private SetStandardPriceResult result;

        [SetUp]
        public void SetUp()
        {
            var lines = new List<BomStandardPrice>
                            {
                                new BomStandardPrice
                                    {
                                        Depth = 7, 
                                        BomName = "BOMBOM", 
                                        MaterialPrice = 110m, 
                                        StandardPrice = 111m,
                                        StockMaterialVariance = -0.8m,
                                        LoanMaterialVariance = null,
                                        AllocLines = null
                                    },
                                new BomStandardPrice
                                    {
                                        Depth = 6,
                                        BomName = "BOMBOM2",
                                        MaterialPrice = 110m,
                                        StandardPrice = 111m,
                                        StockMaterialVariance = -0.8m,
                                        LoanMaterialVariance = null,
                                        AllocLines = null
                                    }
                            };
            this.StoresMatVarPack.MakeReqHead(33087).Returns(123);
            this.result = this.Sut.SetStandardPrices(lines, 33087, "REMARKABLE");
        }

        [Test]
        public void ShouldMakeReq()
        {
            this.StoresMatVarPack.Received(1).MakeReqHead(33087);
        }

        [Test]
        public void ShouldMakeLines()
        {
            this.StoresMatVarPack.Received(1).MakeReqLine(123, "BOMBOM", 33087);
            this.StoresMatVarPack.Received(1).MakeReqLine(123, "BOMBOM2", 33087);

        }

        [Test]
        public void ShouldReturnResult()
        {
            this.result.Success.Should().BeTrue();
            this.result.Message.Should().Be("2 records updated.");
        }
    }
}
