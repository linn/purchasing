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

    // test('Displays search fields with correct default values...', () => {
    //     const supplierSearchContainer = screen.getByTestId('supplierSearch');
    //     const searchInput = within(supplierSearchContainer).getByRole('input');

    //     // var button = screen.getByLabelText('Run Report');
    //     fireEvent.click(searchInput);
    //     expect(screen.getByLabelText('Search for a supplier')).toBeInTheDocument();

        

    //     // expect(addItemActionSpy).toHaveBeenCalledTimes(1);
    //     // expect(addItemActionSpy).toHaveBeenCalledWith(
    //     //     expect.objectContaining({ createdBy: 33087 })
    //     // );
    //     // searchSuppliersActionSpy

    // });
});
