namespace Linn.Purchasing.Proxy
{
    using System.Data;

    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;

    using Oracle.ManagedDataAccess.Client;

    public class EdiEmailPack : IEdiEmailPack
    {
        private readonly IDatabaseService databaseService;

        public EdiEmailPack(IDatabaseService databaseService)
        {
            this.databaseService = databaseService;
        }

        public void GetEdiOrders(int supplierId)
        {
            using (var connection = this.databaseService.GetConnection())
            {
                connection.Open();
                var cmd = new OracleCommand("edi_email_pack.get_edi_orders", connection)
                              {
                                  CommandType = CommandType.StoredProcedure
                              };
                var pSupplierId = new OracleParameter("p_supplier_id", OracleDbType.Int32)
                                 {
                                     Direction = ParameterDirection.Input, Size = 50, Value = supplierId
                                 };

                cmd.Parameters.Add(pSupplierId);

                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public string SendEdiOrder(int supplierId, string altEmail, string additionalEmail, string additionalText, bool test)
        {
            using (var connection = this.databaseService.GetConnection())
            {
                connection.Open();
                var cmd = new OracleCommand("edi_email_pack.SEND_EDI_ORDER", connection)
                              {
                                  CommandType = CommandType.StoredProcedure
                              };

                // must be first
                var result = new OracleParameter("result", OracleDbType.Varchar2)
                                 {
                                     Direction = ParameterDirection.ReturnValue,
                                     Size = 1000
                                 };
                cmd.Parameters.Add(result);

                var supplierIdParam = new OracleParameter("p_supplier_id", OracleDbType.Int32)
                                      {
                                          Direction = ParameterDirection.Input,
                                          Size = 50,
                                          Value = supplierId
                                      };

                cmd.Parameters.Add(supplierIdParam);

                var altEmailParam = new OracleParameter("p_alt_email", OracleDbType.Varchar2)
                                          {
                                              Direction = ParameterDirection.Input,
                                              Size = 128,
                                              Value = altEmail
                };

                cmd.Parameters.Add(altEmailParam);

                var additionalEmailParam = new OracleParameter("p_additional_email", OracleDbType.Varchar2)
                                               {
                                                   Direction = ParameterDirection.Input,
                                                   Size = 128,
                                                   Value = additionalEmail
                };

                cmd.Parameters.Add(additionalEmailParam);

                var additionalTextParam = new OracleParameter("p_additional_text", OracleDbType.Varchar2)
                                        {
                                            Direction = ParameterDirection.Input,
                                            Size = 512,
                                            Value = additionalText
                                        };

                cmd.Parameters.Add(additionalTextParam);

                var testTextParam = new OracleParameter("p_test", OracleDbType.Varchar2)
                                              {
                                                  Direction = ParameterDirection.Input,
                                                  Size = 20,
                                                  Value = test ? "true" : "false"
                                              };

                cmd.Parameters.Add(testTextParam);

                cmd.ExecuteNonQuery();
                connection.Close();

                return result.Value.ToString();
            }
        }
    }
}
