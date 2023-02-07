namespace Linn.Purchasing.Proxy
{
    using System.Data;

    using Linn.Common.Persistence;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;

    using Oracle.ManagedDataAccess.Client;

    public class StoresMatVarPack : IStoresMatVarPack
    {
        private readonly IDatabaseService databaseService;

        private readonly ITransactionManager transactionManager;

        public StoresMatVarPack(
            IDatabaseService databaseService,
            ITransactionManager transactionManager)
        {
            this.databaseService = databaseService;
            this.transactionManager = transactionManager;
        }

        public int MakeReqHead(int who)
        {
            using (var connection = this.databaseService.GetConnection())
            {
                connection.Open();

                var cmd = new OracleCommand("stores_matvar_pack.make_matvar_req_head", connection)
                              {
                                  CommandType = CommandType.StoredProcedure
                              };
                cmd.Parameters.Add(new OracleParameter("p_req_number", OracleDbType.Int32)
                                       {
                                           Direction = ParameterDirection.InputOutput,
                                           Size = 50,
                                           Value = 0
                                       });
                cmd.Parameters.Add(new OracleParameter("p_created_by", OracleDbType.Int32)
                                       {
                                           Direction = ParameterDirection.Input,
                                           Size = 50,
                                           Value = who
                                       });
                cmd.Parameters.Add(new OracleParameter("p_trans_ref", OracleDbType.Varchar2)
                                       {
                                           Direction = ParameterDirection.Input,
                                           Size = 50,
                                           Value = null
                                       });
                cmd.ExecuteNonQuery();
                if (int.TryParse(cmd.Parameters[0].ToString(), out var req))
                {
                    return req;
                }

                return 0;
            }
        }

        public void MakeReqLine(int reqNumber, string partNumber, int who)
        {
            using (var connection = this.databaseService.GetConnection())
            {
                connection.Open();
                var cmd = new OracleCommand("stores_matvar_pack.make_matvar_req_line", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new OracleParameter("p_req_number", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Size = 50,
                    Value = reqNumber
                });
               
                cmd.Parameters.Add(new OracleParameter("p_part_number", OracleDbType.Varchar2)
                {
                    Direction = ParameterDirection.Input,
                    Size = 50,
                    Value = partNumber
                });
                cmd.Parameters.Add(new OracleParameter("p_created_by", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Size = 50,
                    Value = who
                });
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
