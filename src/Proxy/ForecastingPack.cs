namespace Linn.Purchasing.Proxy
{
    using System.Data;

    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;

    using Oracle.ManagedDataAccess.Client;

    public class ForecastingPack : IForecastingPack
    {
        private readonly IDatabaseService databaseService;

        public ForecastingPack(IDatabaseService databaseService)
        {
            this.databaseService = databaseService;
        }

        public void ApplyAcrossBoardPlanChange(decimal change, int startPeriod, int endPeriod)
        {
            using var connection = this.databaseService.GetConnection();

            connection.Open();
            var cmd = new OracleCommand("forecasting_pack_p.apply_across_board_plan_change", connection)
                          {
                              CommandType = CommandType.StoredProcedure
                          };
            var pChange = new OracleParameter(" p_perc_change", OracleDbType.Decimal)
                            {
                                Direction = ParameterDirection.Input,
                                Size = 50,
                                Value = change
                            };
            var pStartPeriod = new OracleParameter("p_start_period", OracleDbType.Int32)
                              {
                                  Direction = ParameterDirection.Input,
                                  Size = 50,
                                  Value = startPeriod
                              };
            var pEndPeriod = new OracleParameter("p_end_period", OracleDbType.Int32)
                              {
                                  Direction = ParameterDirection.Input,
                                  Size = 50,
                                  Value = endPeriod
                              };


            cmd.Parameters.Add(pChange);
            cmd.Parameters.Add(pStartPeriod);
            cmd.Parameters.Add(pEndPeriod);

            cmd.ExecuteNonQuery();
            connection.Close();
        }

        public void SetAutoForecastChange(decimal change, int startWeek, int endWeek)
        {
            using var connection = this.databaseService.GetConnection();

            connection.Open();
            var cmd = new OracleCommand("forecasting_pack_p.set_auto_forecast_changes", connection)
                          {
                              CommandType = CommandType.StoredProcedure
                          };
            var pChange = new OracleParameter(" p_perc_change", OracleDbType.Decimal)
                              {
                                  Direction = ParameterDirection.Input,
                                  Size = 50,
                                  Value = change
                              };
            var pStartWeek = new OracleParameter("p_start_week", OracleDbType.Int32)
                                   {
                                       Direction = ParameterDirection.Input,
                                       Size = 50,
                                       Value = startWeek
                                   };
            var pEndWeek = new OracleParameter("p_end_week", OracleDbType.Int32)
                                 {
                                     Direction = ParameterDirection.Input,
                                     Size = 50,
                                     Value = endWeek
                                 };


            cmd.Parameters.Add(pChange);
            cmd.Parameters.Add(pStartWeek);
            cmd.Parameters.Add(pEndWeek);

            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }
}
