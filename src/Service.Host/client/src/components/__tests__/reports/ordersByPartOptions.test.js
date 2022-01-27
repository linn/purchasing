/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import '@testing-library/jest-dom/extend-expect';
import { cleanup, screen } from '@testing-library/react';
import render from '../../../test-utils';
import OrdersByPartOptions from '../../reports/OrdersByPartOptions';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

const state = {
    oidc: { user: { profile: { name: 'User Name', employee: '/employees/33087' } } },
    router: { location: { pathname: '', query: { partId: 1, supplierId: 2 } } },
    parts: { searchItems: null, loading: false },
    ordersByPart: {
        searchItems: null,
        loading: false,
        options: {}
    }
};

const stateWithPrevOptions = {
    oidc: { user: { profile: { name: 'User Name', employee: '/employees/33087' } } },
    router: { location: { pathname: '', query: { partId: 1, supplierId: 2 } } },
    parts: { searchItems: null, loading: false },
    ordersByPart: {
        searchItems: null,
        loading: false,
        options: {
            cancelled: 'Y',
            fromDate: '2021-12-25T09:40:37.465Z',
            partNumber: 'RAW 231',
            toDate: '2022-01-25T09:40:37.465Z'
        }
    }
};

describe('When component mounts...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(state));

        render(<OrdersByPartOptions />);
    });

    test('Displays search fields with correct default values...', () => {
        expect(screen.getByDisplayValue('click to set part')).toBeInTheDocument();
        expect(screen.getByText('From Date')).toBeInTheDocument();
        expect(screen.getByText('To Date')).toBeInTheDocument();

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

        render(<OrdersByPartOptions />);
    });

    test('Displays search fields with correct values...', () => {
        expect(screen.getByDisplayValue('RAW 231')).toBeInTheDocument();
        expect(screen.getByLabelText('Include Cancelled')).toHaveDisplayValue('Yes');
    });
});
