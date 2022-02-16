/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import '@testing-library/jest-dom/extend-expect';
import { cleanup, screen } from '@testing-library/react';
import render from '../../../test-utils';
import SpendByPartOptions from '../../reports/SpendByPartOptions';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

const state = {
    oidc: { user: { profile: { name: 'User Name', employee: '/employees/33087' } } },
    router: { location: { pathname: '', query: { partId: 1, supplierId: 2 } } },
    suppliers: { searchItems: null, loading: false },
    spendByPartReport: {
        searchItems: null,
        loading: false,
        options: {}
    }
};

describe('When component mounts...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(state));

        render(<SpendByPartOptions />);
    });

    test('Displays search fields with correct default values...', () => {
        expect(screen.getByText('Supplier')).toBeInTheDocument();

        expect(screen.getByText('Run Report')).toBeInTheDocument();
    });
});
