namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System;

    public class PurchaseOrderPosting
    {
        //todo this has a nomacc id which corresponds to nominal_account, 
        //which in turn has the department and nominal. Will need to pull it all in
        //with .includes and show it on front end!

        //PLORP_ID NOT NULL NUMBER(10)
        //PLORL_ORDER_NUMBER NOT NULL NUMBER
        //PLORL_ORDER_LINE NOT NULL NUMBER(6)
        //QTY NOT NULL NUMBER(14,5)
        //NOMACC_ID NOT NULL NUMBER(6)
        //PRODUCT VARCHAR2(10)
        //PERSON NUMBER(6)
        //BUILDING VARCHAR2(10)
        //VEHICLE VARCHAR2(10)
        //NOTES VARCHAR2(200)
    }
}
