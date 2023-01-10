namespace Linn.Purchasing.Proxy
{
    using System.Data;

    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;

    using Oracle.ManagedDataAccess.Client;

    public class BomPack : IBomPack
    {
        private readonly IDatabaseService databaseService;

        public BomPack(IDatabaseService databaseService)
        {
            this.databaseService = databaseService;
        }

        public void CopyBom(
            string srcPartNumber, int destBomId, int destChangeId, string destChangeState, string addOrOverWrite)
        {
            using (var connection = this.databaseService.GetConnection())
            {
                connection.Open();
                var cmd = new OracleCommand("bom_pack.copy_bom_from_part", connection)
                              {
                                  CommandType = CommandType.StoredProcedure
                              };
                cmd.Parameters.Add(new OracleParameter("p_source_part_number", OracleDbType.Varchar2)
                                       {
                                           Direction = ParameterDirection.Input,
                                           Size = 50,
                                           Value = srcPartNumber
                                       });
                cmd.Parameters.Add(new OracleParameter("p_dest_bom_id", OracleDbType.Int32)
                                       {
                                           Direction = ParameterDirection.Input,
                                           Size = 50,
                                           Value = destBomId
                                       });

                cmd.Parameters.Add(new OracleParameter("p_dest_change_id", OracleDbType.Int32)
                                       {
                                           Direction = ParameterDirection.Input,
                                           Size = 50,
                                           Value = destChangeId
                                       });
                cmd.Parameters.Add(new OracleParameter("p_dest_change_state", OracleDbType.Varchar2)
                                       {
                                           Direction = ParameterDirection.Input,
                                           Size = 50,
                                           Value = destChangeState
                                       });
                cmd.Parameters.Add(new OracleParameter("p_add_or_overwrite", OracleDbType.Varchar2)
                                       {
                                           Direction = ParameterDirection.Input,
                                           Size = 50,
                                           Value = addOrOverWrite
                                       });
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
