const order = {
    orderNumber: 100157,
    currency: {
        code: 'GBP',
        name: 'UK Sterling',
        exchangeRate: null
    },
    orderDate: '1991-11-25T00:00:00.0000000',
    orderMethod: {
        name: 'MANUAL',
        description: 'MANUAL ORDERING'
    },
    cancelled: 'N',
    overbook: null,
    documentType: {
        description: 'PURCHASE ORDER',
        name: 'PO'
    },
    links: [
        { href: '/fil-cancel', rel: 'fil-cancel' },
        { href: '/edit', rel: 'edit' }
    ],
    supplier: {
        id: 6231,
        name: 'THE SAW CENTRE',
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
        vendorManagerId: 'A',
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
                    orderLine: 1,
                    orderNumber: 100157,
                    ourDeliveryQty: 5,
                    qtyNetReceived: 0,
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
        }
    ],
    orderContactName: 'Ken Nish',
    exchangeRate: 1,
    issuePartsToSupplier: 'N',
    deliveryAddress: {
        address: 'LINN PRODUCTS LIMITED\nFloors Road\nWaterfoot\nGlasgow\nG76 0EP\nUnited Kingdom',
        addressId: 6588,
        description: 'OLD LINN HQ'
    },
    requestedBy: {
        id: 7005,
        fullName: 'RICHARD PHILLIPS'
    },
    enteredBy: {
        id: 20000,
        fullName: 'JANE MCFARLANE'
    },
    quotationRef: 'IAIN',
    authorisedBy: {
        id: 5001,
        fullName: 'KRISTINE CROZIER'
    },
    sentByMethod: 'NONE',
    filCancelled: 'Y',
    remarks: 'Requested by MALCOLM',
    dateFilCancelled: '1993-08-31T00:00:00.0000000',
    periodFilCancelled: null,
    currentlyUsingOverbookForm: false,
    orderAddress: {
        addressId: 28822,
        addressee: 'THE SAW CENTRE',
        addressee2: null,
        line1: 'EGLINGTON STREET',
        line2: 'GLASGOW',
        line3: null,
        line4: null,
        postCode: null,
        countryCode: 'GB',
        countryName: 'UNITED KINGDOM / ISLE OF MAN',
        fullAddress: 'THE SAW CENTRE\nEGLINGTON STREET\nGLASGOW\n\nUnited Kingdom'
    },
    invoiceAddressId: 28823,
    supplierContactEmail: null,
    supplierContactPhone: '0141 123 4567',
    orderNetTotal: 6000,
    baseOrderNetTotal: 6000
};

export default order;
