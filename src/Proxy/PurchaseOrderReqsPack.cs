namespace Linn.Purchasing.Proxy
{
    using System.Data;

    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;

    using Oracle.ManagedDataAccess.Client;

    public class PurchaseOrderReqsPack : IPurchaseOrderReqsPack
    {
        private readonly IDatabaseService databaseService;

        public PurchaseOrderReqsPack(IDatabaseService databaseService)
        {
            this.databaseService = databaseService;
        }

        public bool StateChangeAllowed(string fromState, string toState)
        {
            using (var connection = this.databaseService.GetConnection())
            {
                connection.Open();
                var cmd = new OracleCommand("blue_req_pack.check_br_state_change", connection)
                              {
                                  CommandType = CommandType.StoredProcedure
                              };
                var from = new OracleParameter("p_original_state", OracleDbType.Varchar2)
                                {
                                    Direction = ParameterDirection.Input, Size = 50, Value = fromState
                };
                var to = new OracleParameter("p_new_state", OracleDbType.Varchar2)
                                  {
                                      Direction = ParameterDirection.Input, Size = 50, Value = toState
                                  };
                var result = new OracleParameter(null, OracleDbType.Varchar2)
                                 {
                                     Direction = ParameterDirection.ReturnValue, Size = 2000
                                 };

                cmd.Parameters.Add(result);
                cmd.Parameters.Add(from);
                cmd.Parameters.Add(from);

                cmd.ExecuteNonQuery();
                connection.Close();

                return result.Value.ToString() == "TRUE";
            }
        }
    }
}
