namespace Linn.Purchasing.Proxy
{
    using System.Data;

    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;

    using Oracle.ManagedDataAccess.Client;

    public class PurchaseOrdersPack : IPurchaseOrdersPack
    {
        private readonly IDatabaseService databaseService;

        public PurchaseOrdersPack(IDatabaseService databaseService)
        {
            this.databaseService = databaseService;
        }

        public bool OrderIsCompleteSql(int orderNumber, int lineNumber)
        {
            using (var connection = this.databaseService.GetConnection())
            {
                connection.Open();
                var cmd = new OracleCommand("pl_orders_pack.order_is_complete_sql ", connection)
                              {
                                  CommandType = CommandType.StoredProcedure
                              };
                var ordNo = new OracleParameter("p_order", OracleDbType.Int32)
                                {
                                    Direction = ParameterDirection.Input, Size = 50, Value = orderNumber
                                };
                var ordLine = new OracleParameter("p_line", OracleDbType.Int32)
                                  {
                                      Direction = ParameterDirection.Input, Size = 50, Value = lineNumber
                                  };
                var result = new OracleParameter(null, OracleDbType.Varchar2)
                                 {
                                     Direction = ParameterDirection.ReturnValue, Size = 2000
                                 };

                cmd.Parameters.Add(result);
                cmd.Parameters.Add(ordNo);
                cmd.Parameters.Add(ordLine);

                cmd.ExecuteNonQuery();
                connection.Close();

                return result.Value.ToString() == "TRUE";
            }
        }
    }
}
