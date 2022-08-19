namespace Linn.Purchasing.Proxy
{
    using System;
    using System.Data;

    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;

    using Oracle.ManagedDataAccess.Client;

    public class LedgerPeriodPack : ILedgerPeriodPack
    {
        private readonly IDatabaseService databaseService;

        public LedgerPeriodPack(IDatabaseService databaseService)
        {
            this.databaseService = databaseService;
        }

        public int GetPeriodNumber(DateTime date)
        {
            using (var connection = this.databaseService.GetConnection())
            {
                connection.Open();
                var cmd = new OracleCommand("ledger_periods_pack.period_number", connection)
                              {
                                  CommandType = CommandType.StoredProcedure
                              };
                var result = new OracleParameter(null, OracleDbType.Int32)
                                 {
                                     Direction = ParameterDirection.ReturnValue
                                 };
                cmd.Parameters.Add(result);

                var dateParam = new OracleParameter("p_date", OracleDbType.Date)
                                          {
                                              Direction = ParameterDirection.Input, 
                                              Size = 50,
                                              Value = date
                                          };
                cmd.Parameters.Add(dateParam);

                cmd.ExecuteNonQuery();
                connection.Close();
                var res = result.Value.ToString();
                return int.Parse(res);
            }
        }
    }
}
