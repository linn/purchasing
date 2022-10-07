namespace Linn.Purchasing.Proxy
{
    using System.Data;

    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;

    using Oracle.ManagedDataAccess.Client;

    public class SalesTaxPack : ISalesTaxPack
    {
        private readonly IDatabaseService databaseService;

        public SalesTaxPack(IDatabaseService databaseService)
        {
            this.databaseService = databaseService;
        }

        public decimal GetVatRateSupplier(int supplierId)
        {
            using var connection = this.databaseService.GetConnection();

            connection.Open();
            var cmd = new OracleCommand("sales_tax_pack.get_vat_rate_supplier", connection)
                          {
                              CommandType = CommandType.StoredProcedure
                          };
            var result = new OracleParameter(null, OracleDbType.Decimal)
                             {
                                 Direction = ParameterDirection.ReturnValue,
                             };
            cmd.Parameters.Add(result);
            var param = new OracleParameter("p_supp", OracleDbType.Int32)
                                 {
                                     Direction = ParameterDirection.Input,
                                     Value = supplierId
                                 };
            cmd.Parameters.Add(param);

            cmd.ExecuteNonQuery();
            connection.Close();

            return decimal.Parse(result.Value.ToString() ?? string.Empty);
        }
    }
}
