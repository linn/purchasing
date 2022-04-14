namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System;
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    public class PurchaseOrder
    {
        public string Cancelled { get; set; }

        public IEnumerable<PurchaseOrderDetail> Details { get; set; }

        public string DocumentTypeName { get; set; }

        public DocumentType DocumentType { get; set; }

        public DateTime OrderDate { get; set; }

        public int OrderNumber { get; set; }

        public Supplier Supplier { get; set; }

        public int SupplierId { get; set; }

        public string Overbook { get; set; }

        public decimal? OverbookQty { get; set; }

        public Currency Currency { get; set; }

        public string CurrencyCode { get; set; }


        //todo make sure this is added in domain from supplier contact when creating
        //or could show as field on front end and pass back
        public string OrderContactName { get; set; }

        public string OrderMethodName { get; set; }

        public OrderMethod OrderMethod { get; set; }

        public decimal? ExchangeRate { get; set; }

        public string IssuePartsToSupplier { get; set; }

        public int DeliveryAddressId { get; set; }

        public LinnDeliveryAddress DeliveryAddress { get; set; }

        public Employee RequestedBy { get; set; }

        public int RequestedById { get; set; }

        public Employee EnteredBy { get; set; }

        public int EnteredById { get; set; }

        public string QuotationRef { get; set; }

        public Employee AuthorisedBy { get; set; }

        public int? AuthorisedById { get; set; }

        public Employee CancelledBy { get; set; }

        public int? CancelledById { get; set; }

        public string ReasonCancelled { get; set; }

        public string SentByMethod { get; set; }

        public string FilCancelled { get; set; }

        public string Remarks { get; set; }

        public DateTime? DateFilCancelled { get; set; }

        public int? PeriodFilCancelled { get; set; }


      

    }
}


//ORDER_NUMBER NOT NULL NUMBER        
//CONT_CONTACT_ID                  NUMBER(8)     
//CONTACT_NAME VARCHAR2(52)
//CURR_CODE NOT NULL VARCHAR2(4)   
//DATE_OF_ORDER NOT NULL DATE          
//ORDER_ADDRESS_ID        NOT NULL NUMBER(38)    
//PL_ORDER_METHOD VARCHAR2(10)
//QUOTATION_REF VARCHAR2(50)
//REMARKS VARCHAR2(500)
//CANCELLED NOT NULL VARCHAR2(1)   
//OVERBOOK VARCHAR2(1)
//DOCUMENT_TYPE NOT NULL VARCHAR2(6)   
//ENTERED_BY NOT NULL NUMBER(6)     
//FIL_CANCELLED NOT NULL VARCHAR2(1)   
//REQUESTED_BY NOT NULL NUMBER(6)     
//SUPP_SUPPLIER_ID NOT NULL NUMBER(6)     
//INVOICE_ADDRESS_ID NOT NULL NUMBER(38)    
//DELIVERY_ADDRESS NOT NULL NUMBER(38)    
//AUTHORISED_BY NUMBER(6)
//CORRECTIVE_ACTION_ID NUMBER(6)
//ORDER_NET_TOTAL NUMBER(14,4)
//ORDER_VAT_TOTAL NUMBER(14,4)
//ORDER_TOTAL NUMBER(14,4)
//DAMAGES_PERCENT NUMBER(6,2)
//SENT_BY_METHOD VARCHAR2(20)
//AUTHORISATION_COMMENT VARCHAR2(200)
//ISSUE_PARTS_TO_SUPPLIER NOT NULL VARCHAR2(1)   
//BASE_CURRENCY NOT NULL VARCHAR2(4)   
//BASE_ORDER_NET_TOTAL NUMBER(14,5)
//BASE_ORDER_VAT_TOTAL NUMBER(14,4)
//BASE_ORDER_TOTAL NUMBER(14,4)
//EXCHANGE_RATE NUMBER(14,5)
//DATE_FIL_CANCELLED DATE          
//PERIOD_FIL_CANCELLED             NUMBER        
//OVERBOOK_QTY                     NUMBER(14,5)  
//ARCHIVE_ORDER VARCHAR2(1)
//SPECIAL_ORDER_TYPE VARCHAR2(20)
//ALLOW_EARLY_DELIVERY VARCHAR2(1)   