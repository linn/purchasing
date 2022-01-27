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

        public decimal CalculateBaseValueFromCurrencyValue(
            string newCurrency,
            decimal newPrice,
            string ledger = "SL",
            string round = "TRUE")
        {
            using var connection = this.databaseService.GetConnection();
            connection.Open();
            var cmd = new OracleCommand("cur_pack.cur_to_base_value", connection)
                          {
                              CommandType = CommandType.StoredProcedure
                          };
            var currencyParam = new OracleParameter("p_curr", OracleDbType.Varchar2)
                                    {
                                        Direction = ParameterDirection.Input,
                                        Size = 50,
                                        Value = newCurrency
                                    };
            var valueParam = new OracleParameter("p_curr_value", OracleDbType.Decimal)
                                 {
                                     Direction = ParameterDirection.Input,
                                     Size = 50,
                                     Value = newPrice
                                 };
            var ledgerParam = new OracleParameter("p_ledger", OracleDbType.Varchar2)
                            {
                                Direction = ParameterDirection.Input,
                                Size = 50,
                                Value = ledger
                            };
            var roundParam = new OracleParameter("p_round", OracleDbType.Varchar2)
                            {
                                Direction = ParameterDirection.Input,
                                Size = 50,
                                Value = round
                            };
            var result = new OracleParameter(null, OracleDbType.Varchar2)
                             {
                                 Direction = ParameterDirection.ReturnValue,
                                 Size = 2000
                             };

            cmd.Parameters.Add(result);
            cmd.Parameters.Add(currencyParam);
            cmd.Parameters.Add(valueParam);
            cmd.Parameters.Add(ledgerParam);
            cmd.Parameters.Add(roundParam);

            cmd.ExecuteNonQuery();
            connection.Close();

            return decimal.Parse(result.Value.ToString() ?? string.Empty);
        }
    }
}
