const detailWithReceived = {
    baseDetailTotal: 7200,
    baseNetTotal: 6000,
    baseOrderUnitPrice: 6000,
    baseOurUnitPrice: 1200,
    baseVatTotal: 1200,
    cancelled: 'N',
    cancelledDetails: null,
    deliveryConfirmedBy: null,
    deliveryInstructions: 'PARTIAL SHIPMENT IS NOT ALLOWED',
    detailTotalCurrency: 7200,
    filCancelled: 'N',
    internalComments: null,
    line: 1,
    mrOrders: null,
    netTotalCurrency: 6000,
    orderNumber: 100157,
    drawingReference: 'Rev 123',
    orderPosting: {
        building: null,
        id: 13227,
        lineNumber: 1,
        nominalAccount: {
            accountId: 825,
            nominal: {
                nominalCode: '0000006035',
                description: 'PLANT & MACHINERY'
            },
            department: {
                departmentCode: '0000002508',
                description: 'ASSETS'
            }
        },
        nominalAccountId: 825,
        notes: 'From mini order',
        orderNumber: 100157,
        person: null,
        product: null,
        qty: 5,
        vehicle: null
    },
    orderUnitOfMeasure: 'ORDER',
    orderUnitPriceCurrency: 6000,
    originalOrderLine: null,
    originalOrderNumber: null,
    ourQty: 5,
    orderQty: 1,
    ourUnitOfMeasure: 'ORDER',
    ourUnitPriceCurrency: 1200,
    partDescription: 'THIS IS A GENERAL PART USED WHEN ORDERING SUNDRY DEV.MATERIAL ',
    partNumber: 'DEVELOPMENT',
    purchaseDeliveries: [
        {
            cancelled: 'N',
            dateAdvised: '2022-09-16T00:00:00.0000000',
            dateRequested: '2022-09-01T00:00:00.0000000',
            deliverySeq: 1,
            netTotalCurrency: 6000,
            baseNetTotal: 6000,
            orderDeliveryQty: 1,
            orderLine: 2,
            orderNumber: 100157,
            ourDeliveryQty: 5,
            qtyNetReceived: 1,
            quantityOutstanding: 2,
            callOffDate: '2022-09-15T11:27:36.0000000',
            baseOurUnitPrice: 1200,
            supplierConfirmationComment: 'hello',
            ourUnitPriceCurrency: 1200,
            orderUnitPriceCurrency: 767,
            baseOrderUnitPrice: 6000,
            vatTotalCurrency: 6000,
            baseVatTotal: 1200,
            deliveryTotalCurrency: 36000,
            baseDeliveryTotal: 7200,
            rescheduleReason: 'ADVISED',
            availableAtSupplier: null,
            partNumber: 'DEVELOPMENT',
            callOffRef: null,
            filCancelled: 'N',
            qtyPassedForPayment: 5,
            confirmedBy: null
        }
    ],
    rohsCompliant: 'N',
    stockPoolCode: 'LINN',
    suppliersDesignation:
        "1 X DUST EXTRACTOR WV2000 @ 495.00 1 X SET VACUUM EQUIPMENT FOR ABOVE @ 150.00 1 X REDUCER 4 3/4' I.D. TO 3 7/8' I.D. @ 25.50 1 X 'Y' JUNCTION @ 40.50 1 X 4' X 12' FLEXI HOSE@ 56.00 TOTAL = 767.00 ",
    vatTotalCurrency: 1200
};

export default detailWithReceived;
