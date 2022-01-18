/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import '@testing-library/jest-dom/extend-expect';
import { cleanup, screen, fireEvent } from '@testing-library/react';
import render from '../../../test-utils';

import suppliersActions from '../../../actions/suppliersActions';
import OrdersBySupplierOptions from '../../reports/OrdersBySupplierOptions';

// import * as itemTypes from '../../../itemTypes';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

const searchSuppliersActionSpy = jest.spyOn(suppliersActions, 'search');

const state = {
    oidc: { user: { profile: { name: 'User Name', employee: '/employees/33087' } } },
    router: { location: { pathname: '', query: { partId: 1, supplierId: 2 } } },
    suppliers: { searchItems: null, loading: false }
};

describe('When component mounts...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(state));

        render(<OrdersBySupplierOptions />);
    });

    test('Displays search fields...', () => {
        expect(screen.getByLabelText('Supplier')).toBeInTheDocument();
        expect(screen.getByLabelText('From Date')).toBeInTheDocument();
        expect(screen.getByLabelText('To Date')).toBeInTheDocument();
        expect(screen.getByLabelText('All or Outstanding')).toBeInTheDocument();

        expect(screen.getByLabelText('Include Returns')).toBeInTheDocument();

        expect(screen.getByLabelText('Stock Controlled')).toBeInTheDocument();

        expect(screen.getByLabelText('Include Credits')).toBeInTheDocument();

        expect(screen.getByLabelText('Include Cancelled')).toBeInTheDocument();
    });
});
