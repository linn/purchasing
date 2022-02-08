namespace Linn.Purchasing.Proxy
{
    using System.Data;

    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;

    using Oracle.ManagedDataAccess.Client;

    public class AutocostPack : IAutocostPack
    {
        private readonly IDatabaseService databaseService;

        public AutocostPack(IDatabaseService databaseService)
        {
            this.databaseService = databaseService;
        }

        public decimal CalculateNewMaterialPrice(string partNumber, string newCurrency, decimal newCurrencyPrice)
        {
            using var connection = this.databaseService.GetConnection();
            connection.Open();
            var cmd = new OracleCommand("autocost_pack.new_material_price", connection)
                          {
                              CommandType = CommandType.StoredProcedure
                          };
            var part = new OracleParameter("p_part_number", OracleDbType.Varchar2)
                               {
                                   Direction = ParameterDirection.Input,
                                   Size = 50,
                                   Value = partNumber
                               };
            var currency = new OracleParameter("p_new_currency", OracleDbType.Varchar2)
                            {
                                Direction = ParameterDirection.Input,
                                Size = 50,
                                Value = newCurrency
                            };
            var value = new OracleParameter("p_new_currency_price", OracleDbType.Decimal)
                            {
                                Direction = ParameterDirection.Input,
                                Size = 50,
                                Value = newCurrencyPrice
                            };
            var result = new OracleParameter(null, OracleDbType.Varchar2)
                             {
                                 Direction = ParameterDirection.ReturnValue,
                                 Size = 2000
                             };

            cmd.Parameters.Add(result);
            cmd.Parameters.Add(part);
            cmd.Parameters.Add(currency);
            cmd.Parameters.Add(value);

            cmd.ExecuteNonQuery();
            connection.Close();

            return decimal.Parse(result.Value.ToString() ?? string.Empty);
        }
    }
}
