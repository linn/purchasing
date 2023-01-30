/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import '@testing-library/jest-dom/extend-expect';
import { cleanup, screen } from '@testing-library/react';
import render from '../../../test-utils';
import PurchaseOrdersAuthSend from '../../PurchaseOrders/PurchaseOrdersAuthSend';
import purchaseOrdersActions from '../../../actions/purchaseOrdersActions';
import vendorManagersActions from '../../../actions/vendorManagersActions';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

const fetchSpy = jest.spyOn(purchaseOrdersActions, 'searchWithOptions');
const stateSpy = jest.spyOn(purchaseOrdersActions, 'fetchState');
const vmSpy = jest.spyOn(vendorManagersActions, 'fetch');

const initialState = {};
const stateWithOrders = {
    purchaseOrders: {
        searchItems: [
            {
                orderNumber: 827432,
                currency: { code: 'GBP', name: 'UK Sterling' },
                orderDate: '2022-07-20T00:00:00.0000000',
                orderMethod: { name: 'AUTO', description: null },
                cancelled: 'N',
                overbook: null,
                documentType: { description: null, name: null },
                supplier: {
                    id: 92836,
                    name: 'FERRARI PACKAGING LTD',
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
                    vendorManagerId: null,
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
                overbookQty: 0.0,
                details: [
                    {
                        baseDetailTotal: 51.84,
                        baseNetTotal: 43.2,
                        baseOrderUnitPrice: 0.3,
                        baseOurUnitPrice: 0.3,
                        baseVatTotal: 8.64,
                        cancelled: 'N',
                        cancelledDetails: null,
                        deliveryConfirmedBy: null,
                        deliveryInstructions: null,
                        detailTotalCurrency: 51.84,
                        internalComments: null,
                        line: 1,
                        mrOrders: null,
                        netTotalCurrency: 43.2,
                        orderNumber: 827432,
                        orderPosting: null,
                        orderUnitOfMeasure: 'ONES',
                        orderUnitPriceCurrency: 0.3,
                        originalOrderLine: null,
                        originalOrderNumber: null,
                        ourQty: 144.0,
                        orderQty: 144.0,
                        ourUnitOfMeasure: 'ONES',
                        ourUnitPriceCurrency: 0.3,
                        partDescription: '1" SELLOTAPE  ',
                        partNumber: '1P-002',
                        purchaseDeliveries: [
                            {
                                cancelled: 'N',
                                dateAdvised: '2022-07-28T13:23:01.0000000',
                                dateRequested: '2022-07-25T00:00:00.0000000',
                                deliverySeq: 1,
                                netTotalCurrency: 21.6,
                                baseNetTotal: 21.6,
                                orderDeliveryQty: 72.0,
                                orderLine: 1,
                                orderNumber: 827432,
                                ourDeliveryQty: 72.0,
                                qtyNetReceived: 0.0,
                                quantityOutstanding: 144.0,
                                callOffDate: '2022-07-27T14:25:34.0000000',
                                baseOurUnitPrice: 0.3,
                                supplierConfirmationComment: null,
                                ourUnitPriceCurrency: 0.3,
                                orderUnitPriceCurrency: 0.3,
                                baseOrderUnitPrice: 0.3,
                                vatTotalCurrency: 4.32,
                                baseVatTotal: 4.32,
                                deliveryTotalCurrency: 25.92,
                                baseDeliveryTotal: 25.92,
                                rescheduleReason: 'ADVISED',
                                availableAtSupplier: null,
                                partNumber: '1P-002',
                                callOffRef: null,
                                filCancelled: 'N',
                                qtyPassedForPayment: 0.0,
                                confirmedBy: null
                            },
                            {
                                cancelled: 'N',
                                dateAdvised: null,
                                dateRequested: '2022-07-25T00:00:00.0000000',
                                deliverySeq: 2,
                                netTotalCurrency: 21.6,
                                baseNetTotal: 21.6,
                                orderDeliveryQty: 72.0,
                                orderLine: 1,
                                orderNumber: 827432,
                                ourDeliveryQty: 72.0,
                                qtyNetReceived: 0.0,
                                quantityOutstanding: 72.0,
                                callOffDate: '2022-07-27T14:25:34.0000000',
                                baseOurUnitPrice: 0.3,
                                supplierConfirmationComment: null,
                                ourUnitPriceCurrency: 0.3,
                                orderUnitPriceCurrency: 0.3,
                                baseOrderUnitPrice: 0.3,
                                vatTotalCurrency: 4.32,
                                baseVatTotal: 4.32,
                                deliveryTotalCurrency: 25.92,
                                baseDeliveryTotal: 25.92,
                                rescheduleReason: 'REQUESTED',
                                availableAtSupplier: null,
                                partNumber: '1P-002',
                                callOffRef: null,
                                filCancelled: 'N',
                                qtyPassedForPayment: 0.0,
                                confirmedBy: null
                            }
                        ],
                        rohsCompliant: 'N',
                        stockPoolCode: 'LINN',
                        suppliersDesignation: '1" SELLOTAPE 25mm x 66m  ',
                        vatTotalCurrency: 8.64
                    }
                ],
                orderContactName: 'LINDSAY',
                exchangeRate: 1.0,
                issuePartsToSupplier: 'N',
                deliveryAddress: null,
                requestedBy: { id: 100, fullName: null },
                enteredBy: { id: 100, fullName: 'Automated Transaction' },
                quotationRef: null,
                authorisedBy: { id: 5, fullName: 'Fred' },
                sentByMethod: 'EMAIL',
                filCancelled: 'N',
                remarks: null,
                dateFilCancelled: null,
                periodFilCancelled: null,
                currentlyUsingOverbookForm: false,
                orderAddress: null,
                supplierContactEmail: null,
                supplierContactPhone: null,
                orderNetTotal: 43.2,
                baseOrderNetTotal: 43.2,
                links: [
                    {
                        href: '/purchasing/purchase-orders/allow-over-book',
                        rel: 'allow-over-book-search'
                    },
                    { href: '/purchasing/purchase-orders/827432', rel: 'self' },
                    { href: '/purchasing/purchase-orders/827432', rel: 'edit' },
                    {
                        href: '/purchasing/purchase-orders/827432/allow-over-book',
                        rel: 'allow-over-book'
                    }
                ]
            }
        ]
    }
};

describe('When component mounts...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(initialState));
        render(<PurchaseOrdersAuthSend />);
    });

    test('Should fetch orders', () => {
        expect(fetchSpy).toBeCalledTimes(1);
        expect(stateSpy).toBeCalledTimes(1);
        expect(vmSpy).toBeCalledTimes(1);
    });
});

describe('When orders are returned...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithOrders));
        render(<PurchaseOrdersAuthSend />);
    });

    test('Should show orders', () => {
        expect(screen.getByText('827432')).toBeInTheDocument();
        expect(screen.getByText('20 Jul 2022')).toBeInTheDocument();
        expect(screen.getByText('FERRARI PACKAGING LTD')).toBeInTheDocument();
        expect(screen.getByText('Fred')).toBeInTheDocument();
        expect(screen.getByText('EMAIL')).toBeInTheDocument();
        expect(screen.getByText('43.20')).toBeInTheDocument();
        expect(screen.getByText('144 x 1P-002')).toBeInTheDocument();
    });
});
