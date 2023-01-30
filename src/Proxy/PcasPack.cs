namespace Linn.Purchasing.Proxy
{
    using System.Data;

    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;

    using Oracle.ManagedDataAccess.Client;

    public class PcasPack : IPcasPack
    {
        private readonly IDatabaseService databaseService;

        public PcasPack(IDatabaseService databaseService)
        {
            this.databaseService = databaseService;
        }

        public string DiscrepanciesOnChange(string boardCode, string revisionCode, int changeId)
        {
            {
                using var connection = this.databaseService.GetConnection();

                connection.Open();
                var cmd = new OracleCommand("pcas_pack.discrepancies_on_pcaschange", connection)
                              {
                                  CommandType = CommandType.StoredProcedure
                              };
                var result = new OracleParameter(null, OracleDbType.Varchar2)
                                 {
                                     Size = 4000,
                                     Direction = ParameterDirection.ReturnValue
                                 };

                cmd.Parameters.Add(result);

                cmd.Parameters.Add(
                    new OracleParameter("p_board_code", OracleDbType.Varchar2)
                        {
                            Direction = ParameterDirection.Input,
                            Value = boardCode
                        });
                cmd.Parameters.Add(
                    new OracleParameter("p_revision_code", OracleDbType.Varchar2)
                        {
                            Direction = ParameterDirection.Input,
                            Value = revisionCode
                        });
                cmd.Parameters.Add(
                    new OracleParameter("p_change_id", OracleDbType.Int32)
                        {
                            Direction = ParameterDirection.Input,
                            Value = changeId
                        });
                cmd.ExecuteNonQuery();
                connection.Close();


                if (result.Value is null || result.Value?.ToString() == "null")
                {
                    return string.Empty;
                }

                return result.Value.ToString();
            }
        }
    }
}
