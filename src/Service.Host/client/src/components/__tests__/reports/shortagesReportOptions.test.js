/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import '@testing-library/jest-dom/extend-expect';
import { cleanup, screen } from '@testing-library/react';
import render from '../../../test-utils';
import ShortagesReportOptions from '../../reports/ShortagesReportOptions';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

const state = {
    oidc: { user: { profile: { name: 'User Name', employee: '/employees/33156' } } },
    vendorManagers: {
        items: [
            {
                vmId: 'J',
                userNumber: 12345,
                name: 'J. Fire'
            },
            {
                vmId: 'A',
                userNumber: 11111,
                name: 'A. Gob'
            }
        ],
        loading: false
    },
    spendBySupplierReport: {
        options: null
    }
};

const stateWithPrevOptions = {
    oidc: { user: { profile: { name: 'User Name', employee: '/employees/33156' } } },
    vendorManagers: {
        items: [
            {
                vmId: 'J',
                userNumber: 12345,
                name: 'J. Fire'
            },
            {
                vmId: 'A',
                userNumber: 11111,
                name: 'A. Gob'
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

        render(<ShortagesReportOptions />);
    });

    test('Displays search fields with correct default values...', () => {
        expect(screen.getByText('Shortages Report')).toBeInTheDocument();

        expect(screen.getByText('Run Report')).toBeInTheDocument();
    });
});

describe('When component has previous options...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithPrevOptions));

        render(<ShortagesReportOptions />);
    });

    test('Displays search fields with correct values...', () => {
        expect(screen.getByText('A A. Gob (11111)')).toBeInTheDocument();
    });
});
