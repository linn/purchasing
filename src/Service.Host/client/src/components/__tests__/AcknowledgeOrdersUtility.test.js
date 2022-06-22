/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import { useLocation } from 'react-router';
import queryString from 'query-string';
import '@testing-library/jest-dom/extend-expect';
import { screen, fireEvent, cleanup, waitFor } from '@testing-library/react';
import render from '../../test-utils';
import purchaseOrderDeliveryActions from '../../actions/purchaseOrderDeliveryActions';
import purchaseOrderDeliveriesActions from '../../actions/purchaseOrderDeliveriesActions';
import batchPurchaseOrderDeliveriesUploadActions from '../../actions/batchPurchaseOrderDeliveriesUploadActions';
import batchPurchaseOrderDeliveriesUpdateActions from '../../actions/batchPurchaseOrderDeliveriesUpdateActions';
import AcknowledgeOrdersUtility from '../AcknowledgeOrdersUtility';
import { purchaseOrderDeliveries, batchPurchaseOrderDeliveriesUpload } from '../../itemTypes';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

jest.mock('react-router', () => ({
    ...jest.requireActual('react-router'),
    useLocation: jest.fn()
}));

const fetchByHrefSpy = jest.spyOn(purchaseOrderDeliveriesActions, 'fetchByHref');

const searchResults = [
    {
        cancelled: 'Y',
        dateAdvised: null,
        dateRequested: '03/01/1994',
        deliverySeq: 1,
        netTotalCurrency: 375,
        baseNetTotal: 375,
        orderDeliveryQty: 15000,
        orderLine: 1,
        orderNumber: 123463,
        ourDeliveryQty: 7500,
        qtyNetReceived: 0,
        quantityOutstanding: 15000,
        callOffDate: '1993-09-03T00:00:00.0000000',
        baseOurUnitPrice: 0.025,
        supplierConfirmationComment: 'a new comment',
        ourUnitPriceCurrency: 0.025,
        orderUnitPriceCurrency: 0.025,
        baseOrderUnitPrice: 0.025,
        vatTotalCurrency: 65.625,
        baseVatTotal: 65.625,
        deliveryTotalCurrency: 440.625,
        baseDeliveryTotal: 440.625,
        rescheduleReason: 'REQUESTED',
        availableAtSupplier: 'Y',
        partNumber: 'TRAN 036',
        callOffRef: null,
        filCancelled: 'N',
        qtyPassedForPayment: 0,
        href: null
    },
    {
        cancelled: 'Y',
        dateAdvised: null,
        dateRequested: '03/01/1994',
        deliverySeq: 2,
        netTotalCurrency: 375,
        baseNetTotal: 375,
        orderDeliveryQty: 15000,
        orderLine: 1,
        orderNumber: 123463,
        ourDeliveryQty: 7500,
        qtyNetReceived: 0,
        quantityOutstanding: 15000,
        callOffDate: '1993-09-03T00:00:00.0000000',
        baseOurUnitPrice: 0.025,
        supplierConfirmationComment: 'a new comment',
        ourUnitPriceCurrency: 0.025,
        orderUnitPriceCurrency: 0.025,
        baseOrderUnitPrice: 0.025,
        vatTotalCurrency: 65.625,
        baseVatTotal: 65.625,
        deliveryTotalCurrency: 440.625,
        baseDeliveryTotal: 440.625,
        rescheduleReason: 'ADVISED',
        availableAtSupplier: 'Y',
        partNumber: 'TRAN 036',
        callOffRef: null,
        filCancelled: 'N',
        qtyPassedForPayment: 0,
        href: null
    }
];

describe('When url query params specifies order...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback =>
            callback({
                router: {
                    location: {
                        pathname: '',
                        query: { orderNumber: 999 }
                    }
                }
            })
        );
        useLocation.mockImplementation(() => ({
            search: 'orderNumber=999'
        }));
        render(<AcknowledgeOrdersUtility />);
    });

    test('Should search for that exact order', () => {
        expect(fetchByHrefSpy).toBeCalledWith(
            `${purchaseOrderDeliveries.uri}?${queryString.stringify({
                orderNumberSearchTerm: 999,
                includeAcknowledged: true
            })}`
        );
    });

    test('Should open look up orders panel', () => {
        expect(screen.getByLabelText('Order Number')).toBeInTheDocument();
    });
});

describe('When looking up orders', () => {
    beforeEach(() => {
        cleanup();
        useLocation.mockImplementation(() => ({
            search: null
        }));
        jest.clearAllMocks();
        useSelector.mockImplementation(callback =>
            callback({
                purchaseOrderDeliveries: {
                    loading: false
                }
            })
        );
        render(<AcknowledgeOrdersUtility />);
        const expansionPanelOpen = screen.getByText('Look Up Some Orders');
        fireEvent.click(expansionPanelOpen);
        const orderNumberInput = screen.getByLabelText('Order Number');
        fireEvent.change(orderNumberInput, { target: { value: 123456 } });
        const supplierInput = screen.getByLabelText('Supplier');

        fireEvent.change(supplierInput, { target: { value: '654321' } });

        // press enter to fire the search
        fireEvent.keyDown(supplierInput, { key: 'Enter', code: 'Enter', keyCode: 13 });
    });

    test('Should look up orders', () => {
        expect(fetchByHrefSpy).toBeCalledWith(
            `${purchaseOrderDeliveries.uri}?${queryString.stringify({
                includeAcknowledged: true,
                orderNumberSearchTerm: 123456,
                supplierSearchTerm: 654321
            })}`
        );
    });
});

describe('When Search Results', () => {
    beforeAll(() => {
        cleanup();
        jest.clearAllMocks();
        render(<AcknowledgeOrdersUtility />);

        const expansionPanelOpen = screen.getByText('Look Up Some Orders');
        fireEvent.click(expansionPanelOpen);
        useSelector.mockImplementation(callback =>
            callback({
                purchaseOrderDeliveries: {
                    loading: false,
                    items: searchResults
                }
            })
        );
    });

    test('Should render results', async () => {
        waitFor(() => expect(screen.getByText('REQUESTED')).toBeInTheDocument());
        waitFor(() => expect(screen.getByText('ADVISED')).toBeInTheDocument());
    });
});

describe('When Updating', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useLocation.mockImplementation(() => ({
            search: null
        }));
        render(<AcknowledgeOrdersUtility />);

        const expansionPanelOpen = screen.getByText('Look Up Some Orders');
        fireEvent.click(expansionPanelOpen);
        useSelector.mockImplementation(callback =>
            callback({
                purchaseOrderDeliveries: {
                    loading: false,
                    items: searchResults
                }
            })
        );

        // select the first row
        const firstCheckbox = screen.getAllByRole('checkbox')[1];
        fireEvent.click(firstCheckbox);
        const applyChangesButton = screen.getByText('Apply Changes To Selected');
        fireEvent.click(applyChangesButton);
    });

    test('Should open change dialog', async () => {
        waitFor(() => expect(screen.getByLabelText('Comment')).toBeInTheDocument());
    });

    test('Should update selected rows', async () => {
        const commentInput = await screen.findByLabelText('Comment');
        expect(commentInput).toBeInTheDocument();
        fireEvent.change(commentInput, { target: { value: 'NEW COMMENT' } });
        expect(screen.getByDisplayValue('NEW COMMENT')).toBeInTheDocument();
    });
});
