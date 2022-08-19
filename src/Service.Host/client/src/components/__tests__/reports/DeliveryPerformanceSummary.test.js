/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import '@testing-library/jest-dom/extend-expect';
import { cleanup, screen } from '@testing-library/react';
import render from '../../../test-utils';
import DeliveryPerformanceSummary from '../../reports/DeliveryPerformanceSummary';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

const state = {
    oidc: { user: { profile: { name: 'User Name', employee: '/employees/33087' } } },
    vendorManagers: {
        items: [
            {
                vmId: 'A',
                userNumber: 12345,
                name: 'Name'
            }
        ]
    },
    ledgerPeriods: {
        items: [
            {
                periodNumber: 1,
                monthName: 'JUN2024'
            }
        ]
    },
    deliveryPerformanceSummaryReport: {
        options: {
            startPeriod: 1,
            endPeriod: 1
        }
    }
};

const stateWithPrevOptions = {
    oidc: { user: { profile: { name: 'User Name', employee: '/employees/33087' } } },
    vendorManagers: {
        items: [
            {
                vmId: 'A',
                userNumber: 12345,
                name: 'Name'
            }
        ]
    },
    ledgerPeriods: {
        items: [
            {
                periodNumber: 1,
                monthName: 'JUN2024'
            }
        ]
    },
    deliveryPerformanceSummaryReport: {
        options: {
            vendorManager: 'A',
            supplierId: 123,
            startPeriod: 1,
            endPeriod: 1
        }
    }
};

describe('When component mounts...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(state));

        render(<DeliveryPerformanceSummary />);
    });

    test('It should render options', () => {
        expect(screen.getByText('Run Report')).toBeInTheDocument();
    });
});

describe('When component has previous options...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithPrevOptions));

        render(<DeliveryPerformanceSummary />);
    });

    test('It should render previous values', () => {
        expect(screen.getByText('A Name (12345)')).toBeInTheDocument();
    });
});
