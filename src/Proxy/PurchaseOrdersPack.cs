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
                var cmd = new OracleCommand("pl_orders_pack.order_is_complete_sql", connection)
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

        public decimal GetVatAmountSupplier(decimal total, int supplierId)
        {
            using var connection = this.databaseService.GetConnection();
            connection.Open();
            var cmd = new OracleCommand("pl_orders_pack.get_vat_amount_supplier", connection)
                          {
                              CommandType = CommandType.StoredProcedure
                          };

            var paramTotal = new OracleParameter("p_tot", OracleDbType.Decimal)
                                 {
                                     Direction = ParameterDirection.Input, Value = total
                                 };
            var paramSupplierId = new OracleParameter("p_supp", OracleDbType.Int32)
                                      {
                                          Direction = ParameterDirection.Input, Value = supplierId
                                      };
            var result = new OracleParameter(null, OracleDbType.Decimal) { Direction = ParameterDirection.ReturnValue };

            cmd.Parameters.Add(result);
            cmd.Parameters.Add(paramTotal);
            cmd.Parameters.Add(paramSupplierId);

            cmd.ExecuteNonQuery();
            connection.Close();

            return decimal.Parse(result.Value.ToString());
        }

        public bool IssuePartsToSupplier(string partNumber, int supplierId)
        {
            using (var connection = this.databaseService.GetConnection())
            {
                connection.Open();
                var cmd = new OracleCommand("pl_orders_pack.issue_parts_to_supplier_sql", connection)
                              {
                                  CommandType = CommandType.StoredProcedure
                              };

                var result = new OracleParameter(null, OracleDbType.Int32)
                                 {
                                     Direction = ParameterDirection.ReturnValue
                                 };
                cmd.Parameters.Add(result);

                cmd.Parameters.Add(
                    new OracleParameter("p_part_number", OracleDbType.Varchar2)
                        {
                            Direction = ParameterDirection.Input, Size = 14, Value = partNumber
                        });
                cmd.Parameters.Add(
                    new OracleParameter("p_supplier_id", OracleDbType.Int32)
                        {
                            Direction = ParameterDirection.Input, Value = supplierId
                        });

                cmd.ExecuteNonQuery();
                connection.Close();

                return int.Parse(result.Value.ToString() ?? string.Empty) == 1;
            }
        }

        public bool OrderCanBeAuthorisedBy(
            int? orderNumber,
            int? lineNumber,
            int userNumber,
            decimal totalValueBase,
            string part,
            string documentType)
        {
            using (var connection = this.databaseService.GetConnection())
            {
                connection.Open();
                var cmd = new OracleCommand("pl_orders_pack.Order_Can_Be_Auth_By_Wrapper", connection)
                              {
                                  CommandType = CommandType.StoredProcedure
                              };
                var ordNo = new OracleParameter("p_order_number", OracleDbType.Int32)
                                {
                                    Direction = ParameterDirection.Input, Size = 50, Value = orderNumber
                                };
                var ordLine = new OracleParameter("p_order_line", OracleDbType.Int32)
                                  {
                                      Direction = ParameterDirection.Input, Size = 50, Value = lineNumber
                                  };
                var puser = new OracleParameter("p_user", OracleDbType.Int32)
                                {
                                    Direction = ParameterDirection.Input, Size = 50, Value = userNumber
                                };

                var ptotal = new OracleParameter("p_total", OracleDbType.Decimal)
                                 {
                                     Direction = ParameterDirection.Input, Size = 50, Value = totalValueBase
                                 };

                var ppart = new OracleParameter("p_part", OracleDbType.Varchar2)
                                {
                                    Direction = ParameterDirection.Input, Size = 50, Value = part
                                };

                var pdocumenttype = new OracleParameter("p_document_type", OracleDbType.Varchar2)
                                        {
                                            Direction = ParameterDirection.Input, Size = 50, Value = documentType
                                        };

                var result = new OracleParameter(null, OracleDbType.Int32)
                                 {
                                     Direction = ParameterDirection.ReturnValue
                                 };

                cmd.Parameters.Add(result);
                cmd.Parameters.Add(ordNo);
                cmd.Parameters.Add(ordLine);
                cmd.Parameters.Add(puser);
                cmd.Parameters.Add(ptotal);
                cmd.Parameters.Add(ppart);
                cmd.Parameters.Add(pdocumenttype);

                cmd.ExecuteNonQuery();
                connection.Close();

                return int.Parse(result.Value.ToString()) == 1;
            }
        }
    }
}
