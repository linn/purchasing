namespace Linn.Purchasing.Proxy
{
    using System;
    using System.Data;

    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrderReqs;

    using Oracle.ManagedDataAccess.Client;

    public class PurchaseOrderAutoOrderPack : IPurchaseOrderAutoOrderPack
    {
        private readonly IDatabaseService databaseService;

        public PurchaseOrderAutoOrderPack(IDatabaseService databaseService)
        {
            this.databaseService = databaseService;
        }

        public CreateOrderFromReqResult CreateMiniOrderFromReq(
              string nominal,
              string department,
              int requestedBy,
              int turnedIntoOrderBy,
              string description,
              string quoteRef,
              string remarksForOrder,
              string partNumber,
              int supplierId,
              decimal qty,
              DateTime? dateRequired,
              decimal ourUnitPrice,
              bool authAllowed)
        {
            using (var connection = this.databaseService.GetConnection())
            {
                connection.Open();
                var cmd = new OracleCommand("pl_auto_order.Set_Optionals", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                var pNominal = new OracleParameter("p_nominal", OracleDbType.Varchar2)
                {
                    Direction = ParameterDirection.Input,
                    Size = 50,
                    Value = nominal
                };
                var pDepartment = new OracleParameter("p_department", OracleDbType.Varchar2)
                {
                    Direction = ParameterDirection.Input,
                    Size = 50,
                    Value = department
                };
                var pRequestedBy = new OracleParameter("p_requested_by", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Size = 50,
                    Value = requestedBy
                };
                var pEnteredBy = new OracleParameter("p_entered_by", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Size = 50,
                    Value = turnedIntoOrderBy
                };
                var pSuppliersDesignation = new OracleParameter("p_suppliers_designation", OracleDbType.Varchar2)
                {
                    Direction = ParameterDirection.Input,
                    Size = 2000,
                    Value = description
                };

                var qQuoteRef = new OracleParameter("p_quotation_ref", OracleDbType.Varchar2)
                {
                    Direction = ParameterDirection.Input,
                    Size = 30,
                    Value = quoteRef
                };

                var p_remarks = new OracleParameter("p_remarks", OracleDbType.Varchar2)
                {
                    Direction = ParameterDirection.Input,
                    Size = 500,
                    Value = remarksForOrder
                };
                var pAuthBy = new OracleParameter("p_auth_by", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Size = 50,
                    Value = turnedIntoOrderBy
                };

                cmd.Parameters.Add(pNominal);
                cmd.Parameters.Add(pDepartment);
                cmd.Parameters.Add(pRequestedBy);
                cmd.Parameters.Add(pEnteredBy);
                cmd.Parameters.Add(pSuppliersDesignation);
                cmd.Parameters.Add(qQuoteRef);
                cmd.Parameters.Add(p_remarks);
                cmd.Parameters.Add(pAuthBy);

                cmd.ExecuteNonQuery();

                var createCmd = new OracleCommand("pl_auto_order.Create_Auto_Order_Wrapper", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                var p_part = new OracleParameter("p_part", OracleDbType.Varchar2)
                {
                    Direction = ParameterDirection.Input,
                    Size = 50,
                    Value = partNumber
                };

                var p_supplier = new OracleParameter("p_supplier", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Size = 50,
                    Value = supplierId
                };
                var p_qty = new OracleParameter("p_qty", OracleDbType.Decimal)
                {
                    Direction = ParameterDirection.Input,
                    Size = 50,
                    Value = qty
                };
                var p_date = new OracleParameter("p_date", OracleDbType.Date)
                {
                    Direction = ParameterDirection.Input,
                    Value = dateRequired,
                    IsNullable = true
                };

                var p_orderNumber = new OracleParameter("p_order_number", OracleDbType.Varchar2)
                {
                    Direction = ParameterDirection.InputOutput,
                    Size = 8,
                    Value = 0
                };

                var p_our_price = new OracleParameter("p_our_price", OracleDbType.Decimal)
                {
                    Direction = ParameterDirection.Input,
                    Size = 19,
                    Value = ourUnitPrice
                };
                var p_auth = new OracleParameter("p_auth", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Value = authAllowed ? 1 : 0
                };

                var result = new OracleParameter(null, OracleDbType.Int32)
                {
                    Direction = ParameterDirection.ReturnValue
                };

                createCmd.Parameters.Add(result);
                createCmd.Parameters.Add(p_part);
                createCmd.Parameters.Add(p_supplier);
                createCmd.Parameters.Add(p_qty);
                createCmd.Parameters.Add(p_date);
                createCmd.Parameters.Add(p_orderNumber);
                createCmd.Parameters.Add(p_our_price);
                createCmd.Parameters.Add(p_auth);
                createCmd.ExecuteNonQuery();

                if (int.Parse(result.Value.ToString()) != 0)
                {
                    var newOrderNumber = int.Parse(p_orderNumber.Value.ToString());
                    connection.Close();
                    return new CreateOrderFromReqResult { Success = true, OrderNumber = newOrderNumber };
                }

                var packageMessageCmd = new OracleCommand("pl_auto_order.return_package_message", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                var messageResult = new OracleParameter(null, OracleDbType.Varchar2)
                {
                    Direction = ParameterDirection.ReturnValue,
                    Size = 2000
                };

                packageMessageCmd.Parameters.Add(messageResult);
                packageMessageCmd.ExecuteNonQuery();

                var returnMessage = messageResult.Value.ToString();

                connection.Close();

                return new CreateOrderFromReqResult { Success = false, Message = returnMessage };
            }
        }
    }
}
