namespace Linn.Purchasing.Proxy
{
    using System.Data;

    using Linn.Common.Persistence;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;

    using Oracle.ManagedDataAccess.Client;

    public class BomPack : IBomPack
    {
        private readonly IDatabaseService databaseService;

        private readonly ITransactionManager transactionManager;

        public BomPack(IDatabaseService databaseService, ITransactionManager transactionManager)
        {
            this.databaseService = databaseService;
            this.transactionManager = transactionManager;
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

        public void ExplodeSubAssembly(int bomId, int changeId, string changeState, string subAssembly)
        {
            this.transactionManager.Commit(); // make sure any added BOM_CHANGE is committed
            using (var connection = this.databaseService.GetConnection())
            {
                connection.Open();
                var cmd = new OracleCommand("bom_pack.explode_sub_assembly", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new OracleParameter("p_bom_id", OracleDbType.Varchar2)
                {
                    Direction = ParameterDirection.Input,
                    Size = 50,
                    Value = bomId
                });
                cmd.Parameters.Add(new OracleParameter("p_change_id", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Size = 50,
                    Value = changeId
                });

                cmd.Parameters.Add(new OracleParameter("p_change_state", OracleDbType.Varchar2)
                {
                    Direction = ParameterDirection.Input,
                    Size = 50,
                    Value = changeState
                });
                cmd.Parameters.Add(new OracleParameter("p_sub_assembly", OracleDbType.Varchar2)
                {
                    Direction = ParameterDirection.Input,
                    Size = 50,
                    Value = subAssembly
                });
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void UndoBomChange(int changeId, int undoneBy)
        {
            using (var connection = this.databaseService.GetConnection())
            {
                connection.Open();
                var cmd = new OracleCommand("bom_pack.undo_bom_change", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(
                    new OracleParameter("p_change_id", OracleDbType.Int32)
                        {
                            Direction = ParameterDirection.Input,
                            Value = changeId
                        });

                cmd.Parameters.Add(
                    new OracleParameter("p_undone_by", OracleDbType.Int32)
                        {
                            Direction = ParameterDirection.Input,
                            Value = undoneBy
                        });
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
