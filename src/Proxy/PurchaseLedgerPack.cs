namespace Linn.Purchasing.Proxy
{
    using System.Data;

    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;

    using Oracle.ManagedDataAccess.Client;

    public class PurchaseLedgerPack : IPurchaseLedgerPack
    {
        private readonly IDatabaseService databaseService;

        public PurchaseLedgerPack(IDatabaseService databaseService)
        {
            this.databaseService = databaseService;
        }

        public int GetLedgerPeriod()
        {
            using (var connection = this.databaseService.GetConnection())
            {
                connection.Open();
                var cmd = new OracleCommand("pl_pack.get_pl_ledger_period", connection)
                              {
                                  CommandType = CommandType.StoredProcedure
                              };
                var result = new OracleParameter("document_type", OracleDbType.Varchar2)
                                 {
                                     Direction = ParameterDirection.ReturnValue, Size = 50
                                 };
                cmd.Parameters.Add(result);

                cmd.ExecuteNonQuery();
                connection.Close();
                var res = result.Value.ToString();
                return int.Parse(res);
            }
        }

        public int GetYearStartLedgerPeriod()
        {
            using (var connection = this.databaseService.GetConnection())
            {
                connection.Open();
                var cmd = new OracleCommand("pl_pack.get_year_start_period", connection)
                              {
                                  CommandType = CommandType.StoredProcedure
                              };
                var result = new OracleParameter("document_type", OracleDbType.Varchar2)
                                 {
                                     Direction = ParameterDirection.ReturnValue,
                                     Size = 50
                                 };
                cmd.Parameters.Add(result);

                cmd.ExecuteNonQuery();
                connection.Close();
                var res = result.Value.ToString();
                return int.Parse(res);
            }
        }
    }
}
