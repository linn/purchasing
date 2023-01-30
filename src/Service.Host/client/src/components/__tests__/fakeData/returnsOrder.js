const returnsOrder = {
    orderNumber: 743638,
    currency: {
        code: 'GBP',
        name: 'UK Sterling',
        exchangeRate: null
    },
    orderDate: '2007-07-31T00:00:00.0000000',
    orderMethod: {
        name: 'MANUAL',
        description: null
    },
    cancelled: 'N',
    overbook: 'N',
    documentType: {
        description: 'RETURNS ORDER',
        name: 'RO'
    },
    supplier: {
        id: 32889,
        name: 'TECHNISPRAY',
        currencyCode: null,
        webAddress: null,
        phoneNumber: null,
        orderContactMethod: null,
        invoiceContactMethod: null,
        suppliersReference: null,
        invoiceGoesToId: null,
        invoiceGoesToName: null,
        expenseAccount: null,
        paymentDays: 0,
        paymentMethod: null,
        paysInFc: null,
        currencyName: null,
        approvedCarrier: null,
        accountingCompany: null,
        vatNumber: null,
        partCategory: null,
        partCategoryDescription: null,
        orderHold: null,
        notesForBuyer: null,
        deliveryDay: null,
        refersToFcId: null,
        refersToFcName: null,
        pmDeliveryDaysGrace: null,
        orderAddressId: null,
        orderFullAddress: null,
        invoiceAddressId: null,
        invoiceFullAddress: null,
        vendorManagerId: 'L',
        plannerId: null,
        accountControllerId: null,
        accountControllerName: null,
        openedById: null,
        openedByName: null,
        closedById: null,
        closedByName: null,
        dateOpened: null,
        dateClosed: null,
        reasonClosed: null,
        notes: null,
        organisationId: 0,
        supplierContacts: null,
        groupId: null,
        country: null,
        orderAddress: null,
        links: null
    },
    overbookQty: 0,
    details: [
        {
            baseDetailTotal: 176.25,
            baseNetTotal: 150,
            baseOrderUnitPrice: 15,
            baseOurUnitPrice: 15,
            baseVatTotal: 26.25,
            cancelled: 'N',
            cancelledDetails: null,
            deliveryConfirmedBy: null,
            deliveryInstructions: null,
            detailTotalCurrency: 176.25,
            internalComments: null,
            line: 1,
            mrOrders: null,
            netTotalCurrency: 150,
            orderNumber: 743638,
            orderPosting: null,
            orderUnitOfMeasure: 'ONES',
            orderUnitPriceCurrency: 15,
            originalOrderLine: 1,
            originalOrderNumber: 742872,
            ourQty: 10,
            orderQty: 10,
            ourUnitOfMeasure: 'ONES',
            ourUnitPriceCurrency: 15,
            partDescription: 'PAINTED ASSEMBLY OF ARM TUBE, ARM TUBE HOLDER & MAIN BODY.',
            partNumber: 'EK 092P/1',
            purchaseDeliveries: [
                {
                    cancelled: 'N',
                    dateAdvised: null,
                    dateRequested: '2007-08-28T00:00:00.0000000',
                    deliverySeq: 1,
                    netTotalCurrency: 150,
                    baseNetTotal: 150,
                    orderDeliveryQty: 10,
                    orderLine: 1,
                    orderNumber: 743638,
                    ourDeliveryQty: 10,
                    qtyNetReceived: 0,
                    quantityOutstanding: 10,
                    callOffDate: '2007-07-31T00:00:00.0000000',
                    baseOurUnitPrice: 15,
                    supplierConfirmationComment: null,
                    ourUnitPriceCurrency: 15,
                    orderUnitPriceCurrency: 15,
                    baseOrderUnitPrice: 15,
                    vatTotalCurrency: 26.25,
                    baseVatTotal: 26.25,
                    deliveryTotalCurrency: 176.25,
                    baseDeliveryTotal: 176.25,
                    rescheduleReason: 'REQUESTED',
                    availableAtSupplier: null,
                    partNumber: 'EK 092P/1',
                    callOffRef: null,
                    filCancelled: 'N',
                    qtyPassedForPayment: 0,
                    confirmedBy: null,
                    links: [
                        {
                            href: '/purchasing/purchase-orders/743638',
                            rel: 'order'
                        }
                    ]
                }
            ],
            rohsCompliant: 'N',
            stockPoolCode: 'LINN',
            suppliersDesignation: 'PAINTED ASSEMBLY OF ARM TUBE, ARM TUBE HOLDER & MAIN BODY.',
            vatTotalCurrency: 26.25,
            filCancelled: 'N',
            dateFilCancelled: null,
            reasonFilCancelled: null,
            filCancelledBy: null,
            filCancelledByName: null,
            drawingReference: null
        }
    ],
    exchangeRate: 1,
    issuePartsToSupplier: 'Y',
    deliveryAddress: null,
    requestedBy: {
        id: 32396,
        fullName: null
    },
    sentByMethod: 'EMAIL',
    filCancelled: 'N',
    remarks: null,
    dateFilCancelled: null,
    periodFilCancelled: null,
    orderAddress: null,
    invoiceAddressId: 33759,
    supplierContactEmail: null,
    supplierContactPhone: null,
    orderNetTotal: 150,
    baseOrderNetTotal: 150,
    reasonCancelled: null,
    cancelledByName: null,
    dateCancelled: '',
    ledgerEntries: null,
    links: [
        {
            href: '/purchasing/purchase-orders/create',
            rel: 'create'
        },
        {
            href: '/purchasing/purchase-orders/quick-create',
            rel: 'quick-create'
        },
        {
            href: '/purchasing/purchase-orders/generate-order-from-supplier-id',
            rel: 'generate-order-fields'
        },
        {
            href: '/purchasing/purchase-orders/create',
            rel: 'create-for-other-user'
        },
        {
            href: '/purchasing/purchase-orders/743638',
            rel: 'self'
        },
        {
            href: '/purchasing/purchase-orders/743638/html',
            rel: 'html'
        },
        {
            href: '/purchasing/purchase-orders/743638',
            rel: 'edit'
        },
        {
            href: '/purchasing/purchase-orders/743638',
            rel: 'overbook'
        },
        {
            href: '/purchasing/purchase-orders/743638/email-dept',
            rel: 'email-dept'
        }
    ],
    href: '/purchasing/purchase-orders/743638'
};

export default returnsOrder;
