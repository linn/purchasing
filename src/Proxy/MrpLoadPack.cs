namespace Linn.Purchasing.Proxy
{
    using System.Data;

    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;

    using Oracle.ManagedDataAccess.Client;

    public class MrpLoadPack : IMrpLoadPack
    {
        private readonly IDatabaseService databaseService;

        public MrpLoadPack(IDatabaseService databaseService)
        {
            this.databaseService = databaseService;
        }

        public int GetNextRunLogId()
        {
            return this.databaseService.GetIdSequence("MR_RUNLOG_SEQ");
        }

        public ProcessResult ScheduleMrp(int runLogId)
        {
            using (var connection = this.databaseService.GetConnection())
            {
                connection.Open();
                var cmd = new OracleCommand("mr_load_pack_new.schedule_mrp ", connection)
                              {
                                  CommandType = CommandType.StoredProcedure
                              };
                cmd.Parameters.Add(
                    new OracleParameter("p_runlog_id", OracleDbType.Int32)
                        {
                            Direction = ParameterDirection.Input, Value = runLogId
                        });
                var messageParameter = new OracleParameter("p_message", OracleDbType.Varchar2)
                                           {
                                               Direction = ParameterDirection.Output,
                                               Size = 2000
                                           };
                cmd.Parameters.Add(messageParameter);

                var successParameter = new OracleParameter("p_success", OracleDbType.Int32)
                                           {
                                               Direction = ParameterDirection.Output
                                           };
                cmd.Parameters.Add(successParameter);

                cmd.ExecuteNonQuery();
                connection.Close();

                var success = int.Parse(successParameter.Value.ToString() ?? string.Empty) == 1;
                var message = messageParameter.Value.ToString();

                return new ProcessResult(
                    success,
                    success ? $"MRP scheduled successfully with run log id {runLogId}" : $"Failed to schedule MRP run. {message}");
            }
        }
    }
}
