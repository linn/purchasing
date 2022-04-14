namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.Parts;

    public class PurchaseOrderDetail
    {
        public string Cancelled { get; set; }

        public int Line { get; set; }

        public decimal BaseNetTotal { get; set; }

        public decimal NetTotalCurrency { get; set; }

        public int OrderNumber { get; set; }

        public int? OurQty { get; set; }

        public Part Part { get; set; }

        public string PartNumber { get; set; }

        public IEnumerable<PurchaseOrderDelivery> PurchaseDeliveries { get; set; }

        public string RohsCompliant { get; set; }

        public string SuppliersDesignation { get; set; }

        public string StockPoolCode { get; set; }

        public PurchaseOrder PurchaseOrder { get; set; }

        public int? OriginalOrderNumber { get; set; }

        public int? OriginalOrderLine { get; set; }

        public string OurUnitOfMeasure { get; set; }

        public string OrderUnitOfMeasure { get; set; }

        public decimal? Duty { get; set; }

        public decimal? OurUnitPriceCurrency { get; set; } //our price
        //from mini order trigger:
        //next_our_unit_price = :new.our_price,
        //next_order_unit_price = :new.order_price,
        public decimal? OrderUnitPriceCurrency { get; set; } //order  price

        public decimal? BaseOurUnitPrice { get; set; }

        public decimal? BaseOrderUnitPrice { get; set; }

        public decimal? VatTotalCurrency { get; set; }

        public decimal? BaseVatTotal { get; set; }

        public decimal? DetailTotalCurrency { get; set; }

        public decimal? BaseDetailTotal { get; set; }

        public string DeliveryInstructions { get; set; }

        public Employee DeliveryConfirmedBy { get; set; }

        public int DeliveryConfirmedById { get; set; }

        public CancelledPODetail CancelledDetail { get; set; }

        public string InternalComments { get; set; }

        public MrOrder MrOrder { get; set; }

    }
}


//ORDER_LINE NOT NULL NUMBER(6)      
//DRAWING_REF VARCHAR2(100)
//SUPPLIERS_DESIGNATION VARCHAR2(2000)
//OUR_QTY NUMBER(14,5)
//ORDER_QTY NUMBER(14,5)
//VAT_TOTAL NUMBER(14,4)
//OUR_UNIT_OF_MEASURE NOT NULL VARCHAR2(14)   
//ORDER_UNIT_OF_MEASURE NOT NULL VARCHAR2(14)   
//ORDER_CONV_FACTOR NUMBER(14,5)
//PART_NUMBER NOT NULL VARCHAR2(14)   
//PRICE_TYPE NOT NULL VARCHAR2(10)   
//QUOTATION_REF VARCHAR2(50)
//CANCELLED NOT NULL VARCHAR2(1)    
//FIL_CANCELLED NOT NULL VARCHAR2(1)    
//UPDATE_PARTSUP_PRICE NOT NULL VARCHAR2(1)    
//ISSUE_PARTS_TO_SUPPLIER NOT NULL VARCHAR2(1)    
//WAS_PREFERRED_SUPPLIER NOT NULL VARCHAR2(1)    
//ORDER_NUMBER NOT NULL NUMBER         
//DELIVERY_INSTRUCTIONS              VARCHAR2(200)  
//NET_TOTAL NUMBER(14,4)
//DETAIL_TOTAL NUMBER(14,4)
//ISSUED_SERIAL_NUMBERS VARCHAR2(1)
//STOCK_POOL_CODE NOT NULL VARCHAR2(10)   
//ORIGINAL_ORDER_NUMBER NUMBER(8)
//ORIGINAL_ORDER_LINE NUMBER(6)
//DAMAGES_PERCENT NUMBER(6,2)
//BASE_OUR_UNIT_PRICE NUMBER(14,5)
//BASE_ORDER_UNIT_PRICE NUMBER(14,5)
//BASE_NET_TOTAL NUMBER(14,4)
//BASE_DETAIL_TOTAL NUMBER(14,4)
//BASE_VAT_TOTAL NUMBER(14,4)
//NEXT_OUR_UNIT_PRICE NUMBER(14,5)
//NEXT_ORDER_UNIT_PRICE NUMBER(14,5)
//MANUFACTURER_PART_NUMBER VARCHAR2(20)
//DATE_FIL_CANCELLED DATE           
//PERIOD_FIL_CANCELLED               NUMBER         
//DUTY_PERCENT                       NUMBER(6,2)    
//OVERBOOK_QTY_ALLOWED NOT NULL NUMBER(14,5)   
//ROHS_COMPLIANT NOT NULL VARCHAR2(1)    
//SHOULD_HAVE_BEEN_BLUE_REQ VARCHAR2(1)
//MPV_AUTHORISED_BY NUMBER(6)
//MPV_REASON VARCHAR2(20)
//PPV_AUTHORISED_BY NUMBER(6)
//PPV_REASON VARCHAR2(20)
//MPV_PPV_COMMENTS VARCHAR2(250)
//DELIVERY_CONFIRMED_BY NUMBER(6)
//TOTAL_QTY_DELIVERED NUMBER(14,5)
//INTERNAL_COMMENTS VARCHAR2(300) 