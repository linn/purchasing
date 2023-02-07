namespace Linn.Purchasing.Proxy
{
    using System;
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

        public void AutoCostAssembly(string partNumber, string changeType, int changedBy, string remarks)
        {
            using (var connection = this.databaseService.GetConnection())
            {
                connection.Open();
                var cmd = new OracleCommand("autocost_pack.autocost_assembly", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new OracleParameter("p_part_number", OracleDbType.Varchar2)
                {
                    Direction = ParameterDirection.Input,
                    Size = 50,
                    Value = partNumber
                });
                cmd.Parameters.Add(new OracleParameter("p_change_type", OracleDbType.Varchar2)
                {
                    Direction = ParameterDirection.Input,
                    Size = 50,
                    Value = changeType
                });

                cmd.Parameters.Add(new OracleParameter("p_changed_by", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Size = 50,
                    Value = changedBy
                });
                cmd.Parameters.Add(new OracleParameter("p_bom_change_id", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Size = 50,
                    Value = DBNull.Value
                });
                cmd.Parameters.Add(new OracleParameter("p_remarks", OracleDbType.Varchar2)
                {
                    Direction = ParameterDirection.Input,
                    Size = 50,
                    Value = remarks
                });
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
