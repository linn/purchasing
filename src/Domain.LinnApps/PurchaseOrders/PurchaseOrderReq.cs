namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System;
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    public class PurchaseOrderReq
    {
        public int ReqNumber { get; set; }

        public string State { get; set; }

        public DateTime ReqDate { get; set; }

        public int? OrderNumber { get; set; }

        public string PartNumber { get; set; }

        public string PartDescription { get; set; }

        public decimal Qty { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal? Carriage { get; set; }

        public decimal? TotalReqPrice { get; set; }

        public string CurrencyCode { get; set; }

        public Currency Currency { get; set; }

        public int? SupplierId { get; set; }

        public string SupplierName { get; set; }

        public string SupplierContact { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

        public string AddressLine4 { get; set; }

        public string PostCode { get; set; }

        public string CountryCode { get; set; }

        public Country Country { get; set; }

        public string PhoneNumber { get; set; }

        public string QuoteRef { get; set; }

        public string Email { get; set; }

        public DateTime DateRequired { get; set; }

        public int RequestedBy { get; set; }

        public Employee RequestedByEmployee { get; set; }

        public int? AuthorisedBy { get; set; }

        public Employee AuthorisedByEmployee { get; set; }

        public int? SecondAuthBy { get; set; }
        //todo check if these are
        //AUTHORISED_2_BY not used since 2014
        // or more likely
        // SECONDARY_AUTH_BY yes 2022
        public Employee SecondAuthByEmployee { get; set; }

        public int? FinanceCheckBy { get; set; }

        public Employee FinanceCheckByEmployee { get; set; }

        public int? TurnedIntoOrderBy { get; set; }

        public Employee TurnedIntoOrderByEmployee { get; set; }

        public string Nominal { get; set; }


        public string RemarksForOrder { get; set; }

       //todo check which below field is:
       // NOTES_NOT_ON_ORDER VARCHAR2(300) not since 2011
        //INTERNAL_ONLY_ORDER_NOTES VARCHAR2(300) yes 2022
        public string InternalNotes { get; set; }

        public string Department { get; set; }


        //BLUE_REQ_NUMBER NOT NULL NUMBER(8)
        //BR_STATE NOT NULL VARCHAR2(20)
        //REQ_DATE NOT NULL DATE
        //ORDER_NUMBER NUMBER
        //QTY NOT NULL NUMBER(14,5)
        //PART_NUMBER VARCHAR2(14)
        //DESCRIPTION VARCHAR2(2000)
        //UNIT_PRICE NOT NULL NUMBER(14,4)
        //CARRIAGE NUMBER(14,4)
        //TOTAL_REQ_PRICE NUMBER(14,4)
        //CURRENCY VARCHAR2(4)
        //SUPPLIER_ID NUMBER(6)
        //SUPPLIER_NAME NOT NULL VARCHAR2(50)
        //SUPPLIER_CONTACT VARCHAR2(50)
        //ADDRESS_1 VARCHAR2(40)
        //ADDRESS_2 VARCHAR2(40)
        //ADDRESS_3 VARCHAR2(40)
        //ADDRESS_4 VARCHAR2(40)
        //POSTAL_CODE VARCHAR2(20)
        //COUNTRY_CODE VARCHAR2(2)
        //PHONE_NUMBER VARCHAR2(40)
        //QUOTE_REF VARCHAR2(200)
        //DATE_REQUIRED DATE
        //REQUESTED_BY NOT NULL NUMBER(6)
        //AUTHORISED_BY NUMBER(6)
        //REMARKS_FOR_ORDER VARCHAR2(200)
        //DEPARTMENT VARCHAR2(10)
        //NOMINAL VARCHAR2(10)
        //TURNED_INTO_ORDER_BY NUMBER(6)
        //FINANCE_CHECKED_BY NUMBER(6)
        //SECONDARY_AUTH_BY NUMBER(6)
        //EMAIL_ADDRESS VARCHAR2(50)
        //INTERNAL_ONLY_ORDER_NOTES VARCHAR2(300)
    }
}
