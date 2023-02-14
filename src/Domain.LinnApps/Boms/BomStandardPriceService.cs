namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Boms.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;

    public class BomStandardPriceService : IBomStandardPriceService
    {
        private IQueryRepository<BomStandardPrice> repository;

        private readonly IAutocostPack autocostPack;

        private readonly IStoresMatVarPack storesMatVarPack;

        public BomStandardPriceService(
            IQueryRepository<BomStandardPrice> repository,
            IStoresMatVarPack storesMatVarPack,
            IAutocostPack autocostPack)
        {
            this.repository = repository;
            this.storesMatVarPack = storesMatVarPack;
            this.autocostPack = autocostPack;
        }

        public IEnumerable<BomStandardPrice> GetPriceVarianceInfo(
            string searchExpression)
        {
            var result = this.repository.FindAll();
            searchExpression = searchExpression.ToUpper().Trim();

            if (!searchExpression.Contains("*"))
            {
                return result.Where(x => x.BomName == searchExpression);
            }
            
            if (searchExpression.StartsWith("*"))
            {
                var expr = searchExpression.Substring(1);
                result = result.Where(x => x.BomName.EndsWith(expr));
            }

            if (searchExpression.EndsWith("*"))
            {
                var expr = searchExpression.Split("*")[0];
                result = result.Where(x => x.BomName.StartsWith(expr));
            }

            return result;
        }

        public SetStandardPriceResult SetStandardPrices(IEnumerable<BomStandardPrice> lines, int who, string remarks)
        {
            var result = new SetStandardPriceResult();

            int? reqNumber = null;
            var count = 0;

            try
            {
                var bomStandardPrices = lines as BomStandardPrice[] ?? lines.ToArray();
                foreach (var line in bomStandardPrices)
                {
                    line.StockMaterialVariance ??= 0;
                    line.LoanMaterialVariance ??= 0;

                    if (line.LoanMaterialVariance + line.StockMaterialVariance != 0 & !reqNumber.HasValue)
                    {
                        reqNumber = this.storesMatVarPack.MakeReqHead(who);
                    }

                    if (line.LoanMaterialVariance + line.StockMaterialVariance != 0)
                    {
                        this.storesMatVarPack.MakeReqLine(reqNumber.GetValueOrDefault(), line.BomName, who);
                    }

                    this.autocostPack.AutoCostAssembly(line.BomName, "STANDARD", who, remarks);

                    count++;
                }

                result.Success = true;

                result.Message = $"{count} records updated.";

                return result;
            }
            catch (Exception x)
            {
                throw new SetStandardPriceException(
                    "An Error occurred updating the standard price:", x);
            }
        }
    }
}
