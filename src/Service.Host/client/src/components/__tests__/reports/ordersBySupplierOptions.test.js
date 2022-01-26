/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import '@testing-library/jest-dom/extend-expect';
import { cleanup, screen } from '@testing-library/react';
import render from '../../../test-utils';
import OrdersBySupplierOptions from '../../reports/OrdersBySupplierOptions';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

const state = {
    oidc: { user: { profile: { name: 'User Name', employee: '/employees/33087' } } },
    router: { location: { pathname: '', query: { partId: 1, supplierId: 2 } } },
    suppliers: { searchItems: null, loading: false },
    ordersBySupplier: {
        searchItems: null,
        loading: false,
        options: {}
    }
};

const stateWithPrevOptions = {
    oidc: { user: { profile: { name: 'User Name', employee: '/employees/33087' } } },
    router: { location: { pathname: '', query: { partId: 1, supplierId: 2 } } },
    suppliers: { searchItems: null, loading: false },
    ordersBySupplier: {
        searchItems: null,
        loading: false,
        options: {
            cancelled: 'Y',
            credits: 'Y',
            fromDate: '2021-12-25T09:40:37.465Z',
            id: '77442',
            outstanding: 'Y',
            returns: 'Y',
            stockControlled: 'O',
            toDate: '2022-01-25T09:40:37.465Z'
        }
    }
};

describe('When component mounts...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(state));

        render(<OrdersBySupplierOptions />);
    });

    test('Displays search fields with correct default values...', () => {
        expect(screen.getByText('Supplier')).toBeInTheDocument();
        expect(screen.getByText('From Date')).toBeInTheDocument();
        expect(screen.getByText('To Date')).toBeInTheDocument();

        expect(screen.getByLabelText('All or Outstanding')).toBeInTheDocument();
        expect(screen.getByLabelText('All or Outstanding')).toHaveDisplayValue('All');

        expect(screen.getByLabelText('Include Returns')).toBeInTheDocument();
        expect(screen.getByLabelText('Include Returns')).toHaveDisplayValue('No');

        expect(screen.getByLabelText('Stock Controlled')).toBeInTheDocument();
        expect(screen.getByLabelText('Stock Controlled')).toHaveDisplayValue('All');

        expect(screen.getByLabelText('Include Credits')).toBeInTheDocument();
        expect(screen.getByLabelText('Include Credits')).toHaveDisplayValue('No');

        expect(screen.getByLabelText('Include Cancelled')).toBeInTheDocument();
        expect(screen.getByLabelText('Include Cancelled')).toHaveDisplayValue('No');

        expect(screen.getByText('Run Report')).toBeInTheDocument();
    });
});

describe('When component has previous options...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithPrevOptions));

        render(<OrdersBySupplierOptions />);
    });

    test('Displays search fields with correct default values...', () => {
        expect(screen.getByLabelText('All or Outstanding')).toHaveDisplayValue('Outstanding');

        expect(screen.getByLabelText('Stock Controlled')).toHaveDisplayValue(
            'Stock Controlled Only'
        );

        expect(screen.getByLabelText('Include Returns')).toHaveDisplayValue('Yes');

        expect(screen.getByLabelText('Include Credits')).toHaveDisplayValue('Yes');

        expect(screen.getByLabelText('Include Cancelled')).toHaveDisplayValue('Yes');
    });
});
