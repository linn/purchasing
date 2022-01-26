namespace Linn.Purchasing.Proxy
{
    using System.Data;

    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;

    using Oracle.ManagedDataAccess.Client;

    public class CurrencyPack : ICurrencyPack
    {

        private readonly IDatabaseService databaseService;

        public CurrencyPack(IDatabaseService databaseService)
        {
            this.databaseService = databaseService;
        }

        public decimal CalculateBaseValueFromCurrencyValue(string newCurrency, decimal newPrice)
        {
            using var connection = this.databaseService.GetConnection();
            connection.Open();
            var cmd = new OracleCommand("cur_pack.cur_to_base_value", connection)
                          {
                              CommandType = CommandType.StoredProcedure
                          };
            var currency = new OracleParameter("P_CURR", OracleDbType.Varchar2)
                            {
                                Direction = ParameterDirection.Input,
                                Size = 50,
                                Value = newCurrency
                            };
            var value = new OracleParameter("P_CURR_VALUE", OracleDbType.Decimal)
                              {
                                  Direction = ParameterDirection.Input,
                                  Size = 50,
                                  Value = newPrice
                              };
            var result = new OracleParameter(null, OracleDbType.Varchar2)
                             {
                                 Direction = ParameterDirection.ReturnValue,
                                 Size = 2000
                             };

            cmd.Parameters.Add(result);
            cmd.Parameters.Add(currency);
            cmd.Parameters.Add(value);

            cmd.ExecuteNonQuery();
            connection.Close();

            return decimal.Parse(result.Value.ToString() ?? string.Empty);
        }
    }
}
