namespace Linn.Purchasing.Proxy
{
    using System.Data;

    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using Oracle.ManagedDataAccess.Client;

    public class PurchaseOrderReqsPack : IPurchaseOrderReqsPack
    {
        private readonly IDatabaseService databaseService;

        public PurchaseOrderReqsPack(IDatabaseService databaseService)
        {
            this.databaseService = databaseService;
        }

        public AllowedToAuthoriseReqResult AllowedToAuthorise(
            string stage,
            int userNumber,
            decimal value,
            string dept,
            string state)
        {
            using (var connection = this.databaseService.GetConnection())
            {
                connection.Open();
                var cmd = new OracleCommand("blue_req_pack.ok_to_authorise", connection)
                              {
                                  CommandType = CommandType.StoredProcedure
                              };

                var pstage = new OracleParameter("p_stage", OracleDbType.Varchar2)
                                 {
                                     Direction = ParameterDirection.Input, Size = 50, Value = stage
                                 };
                var puser = new OracleParameter("p_user", OracleDbType.Int32)
                                {
                                    Direction = ParameterDirection.Input, Size = 50, Value = userNumber
                                };
                var pvalue = new OracleParameter("p_value", OracleDbType.Int32)
                                 {
                                     Direction = ParameterDirection.Input, Size = 50, Value = (int) value
                                 };
                var pdept = new OracleParameter("p_dept", OracleDbType.Varchar2)
                                {
                                    Direction = ParameterDirection.Input, Size = 50, Value = dept
                                };
                var pstate = new OracleParameter("p_state", OracleDbType.Varchar2)
                                 {
                                     Direction = ParameterDirection.InputOutput, Size = 50, Value = state
                                 };
                var result = new OracleParameter(null, OracleDbType.Int32)
                                 {
                                     Direction = ParameterDirection.ReturnValue
                                 };

                cmd.Parameters.Add(result);
                cmd.Parameters.Add(pstate);
                cmd.Parameters.Add(pvalue);
                cmd.Parameters.Add(pstage);
                cmd.Parameters.Add(puser);
                cmd.Parameters.Add(pdept);

                cmd.ExecuteNonQuery();

                if (int.Parse(result.Value.ToString()) == 2)
                {
                    var newState = pstate.Value.ToString();
                    connection.Close();
                    return new AllowedToAuthoriseReqResult { Success = true, NewState = newState };
                }

                var packageMessageCmd = new OracleCommand("blue_req_pack.return_package_message", connection)
                                            {
                                                CommandType = CommandType.StoredProcedure
                                            };
                var messageResult = new OracleParameter(null, OracleDbType.Varchar2)
                                        {
                                            Direction = ParameterDirection.ReturnValue, Size = 2000
                                        };

                packageMessageCmd.Parameters.Add(messageResult);
                packageMessageCmd.ExecuteNonQuery();

                var returnMessage = messageResult.Value.ToString();
                connection.Close();

                return new AllowedToAuthoriseReqResult { Success = false, Message = returnMessage };
            }
        }
    }
}
