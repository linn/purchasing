/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import '@testing-library/jest-dom/extend-expect';
import { cleanup, screen } from '@testing-library/react';
import render from '../../../test-utils';
import SpendBySupplierOptions from '../../reports/SpendBySupplierOptions';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

const state = {
    oidc: { user: { profile: { name: 'User Name', employee: '/employees/33087' } } },
    vendorManagers: {
        items: [
            {
                vmId: 'P',
                userNumber: 12345,
                name: 'MR POTATO HEAD'
            },
            {
                vmId: 'A',
                userNumber: 11111,
                name: 'Goodbye Mr A'
            }
        ],
        loading: false
    },
    spendBySupplierReport: {
        options: null
    }
};

const stateWithPrevOptions = {
    oidc: { user: { profile: { name: 'User Name', employee: '/employees/33087' } } },
    vendorManagers: {
        items: [
            {
                vmId: 'P',
                userNumber: 12345,
                name: 'MR POTATO HEAD'
            },
            {
                vmId: 'A',
                userNumber: 11111,
                name: 'Goodbye Mr A'
            }
        ],
        loading: false
    },
    spendBySupplierReport: {
        options: {
            vm: 'A'
        }
    }
};

describe('When component mounts...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(state));

        render(<SpendBySupplierOptions />);
    });

    test('Displays search fields with correct default values...', () => {
        expect(screen.getByText('All')).toBeInTheDocument();
    });
});

describe('When component has previous options...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithPrevOptions));

        render(<SpendBySupplierOptions />);
    });

    test('Displays search fields with correct values...', () => {
        expect(screen.getByText('A Goodbye Mr A (11111)')).toBeInTheDocument();
    });
});
