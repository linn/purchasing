/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import '@testing-library/jest-dom/extend-expect';
import { cleanup, screen } from '@testing-library/react';
import routeData from 'react-router';
import render from '../../test-utils';
import AutomaticPurchaseOrders from '../AutomaticPurchaseOrders';
import automaticPurchaseOrderActions from '../../actions/automaticPurchaseOrderActions';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

const fetchSpy = jest.spyOn(automaticPurchaseOrderActions, 'fetch');

const initialState = {};
const stateWithOrder = {
    automaticPurchaseOrder: {
        item: {
            id: 1315946,
            startedBy: 7005,
            jobRef: 'AAJBBE',
            dateRaised: '2022-07-15T15:30:00.0000000',
            supplierId: null,
            planner: null,
            details: [
                {
                    id: 1315946,
                    sequence: 1,
                    partNumber: 'MOLD 198/1',
                    supplierId: 75901,
                    supplierName: 'PASCOE ENGINEERING LTD',
                    orderNumber: 827429,
                    quantity: 200.0,
                    quantityRecommended: 200.0,
                    recommendationCode: 'CHECK1',
                    orderLog: 'Created by auto order',
                    currencyCode: 'GBP',
                    currencyPrice: 590.0,
                    basePrice: 590.0,
                    requestedDate: '2022-07-11T00:00:00.0000000',
                    orderMethod: 'MANUAL',
                    issuePartsToSupplier: 'N',
                    issueSerialNumbers: 'N'
                }
            ],
            links: [{ href: '/purchasing/automatic-purchase-orders/1315946', rel: 'self' }]
        }
    }
};

const idParam = {
    id: 1315946
};
beforeEach(() => {
    jest.spyOn(routeData, 'useParams').mockReturnValue(idParam);
});

describe('When component mounts...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(initialState));
        render(<AutomaticPurchaseOrders />);
    });

    test('Should fetch order', () => {
        expect(fetchSpy).toBeCalledTimes(1);
        expect(fetchSpy).toBeCalledWith(1315946);
    });
});

describe('When automatic order is returned...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithOrder));
        render(<AutomaticPurchaseOrders />);
    });

    test('Should show id', () => {
        expect(screen.getByText('Transaction Id:')).toBeInTheDocument();
        expect(screen.getByText('1315946')).toBeInTheDocument();
    });

    test('Should show jobref', () => {
        expect(screen.getByText('JobRef:')).toBeInTheDocument();
        expect(screen.getByText('AAJBBE')).toBeInTheDocument();
    });

    test('Should show detail', () => {
        expect(screen.getByText('MOLD 198/1')).toBeInTheDocument();
        expect(screen.getByText('75901')).toBeInTheDocument();
        expect(screen.getByText('PASCOE ENGINEERING LTD')).toBeInTheDocument();
        expect(screen.getByText('200')).toBeInTheDocument();
        expect(screen.getByText('11 Jul 2022')).toBeInTheDocument();
        expect(screen.getByText('827429')).toBeInTheDocument();
        expect(screen.getByText('N')).toBeInTheDocument();
    });
});
