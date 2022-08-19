/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import '@testing-library/jest-dom/extend-expect';
import { cleanup, screen } from '@testing-library/react';
import render from '../../../test-utils';
import SpendBySupplier from '../../reports/SpendBySupplier';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

const state = {
    oidc: { user: { profile: { name: 'User Name', employee: '/employees/33087' } } },
    router: { location: { pathname: '', query: { vm: '' } } },
    spendBySupplierReport: {
        options: {
            vm: ''
        },
        results: {
            loading: false,
            data: {
                title: {
                    displayString:
                        'Spend by supplier report for Vendor Manager: A - No person assigned (101). For this financial year and last, excludes factors & VAT.',
                    drillDowns: []
                },
                resultType: null,
                reportValueType: 'Value',
                displaySequence: null,
                headers: {
                    rowHeader: null,
                    columnHeaders: ['Supplier Id', 'Name', 'Last Year', 'This Year', 'This Month'],
                    varianceRows: [],
                    varianceColumns: [],
                    totalColumns: [],
                    textColumns: [0, 1, 2, 3, 4]
                },
                filterOptions: [],
                results: [
                    {
                        rowTitle: {
                            displayString: '47881',
                            drillDowns: []
                        },
                        rowSortOrder: 0,
                        values: [
                            {
                                displayValue: null,
                                textDisplayValue: '47881',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: false,
                                drillDowns: []
                            },
                            {
                                displayValue: null,
                                textDisplayValue: 'TRUMPF LTD',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: null,
                                textDisplayValue: '£324,249.50',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: null,
                                textDisplayValue: '£61,484.98',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: null,
                                textDisplayValue: '£62.10',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            }
                        ],
                        rowType: 'Value'
                    }
                ],
                totals: {
                    rowTitle: {
                        displayString: 'Totals',
                        drillDowns: []
                    },
                    rowSortOrder: null,
                    values: [
                        {
                            displayValue: null,
                            textDisplayValue: null,
                            prefix: null,
                            suffix: null,
                            decimalPlaces: null,
                            allowWrap: false,
                            drillDowns: []
                        },
                        {
                            displayValue: null,
                            textDisplayValue: null,
                            prefix: null,
                            suffix: null,
                            decimalPlaces: null,
                            allowWrap: true,
                            drillDowns: []
                        },
                        {
                            displayValue: null,
                            textDisplayValue: null,
                            prefix: null,
                            suffix: null,
                            decimalPlaces: null,
                            allowWrap: true,
                            drillDowns: []
                        },
                        {
                            displayValue: null,
                            textDisplayValue: null,
                            prefix: null,
                            suffix: null,
                            decimalPlaces: null,
                            allowWrap: true,
                            drillDowns: []
                        },
                        {
                            displayValue: null,
                            textDisplayValue: null,
                            prefix: null,
                            suffix: null,
                            decimalPlaces: null,
                            allowWrap: true,
                            drillDowns: []
                        }
                    ],
                    rowType: 'Total'
                },
                reportHelpText: null
            }
        }
    }
};

describe('When component mounts...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(state));

        render(<SpendBySupplier />);
    });

    test('Displays report title, column title, row data & export button...', () => {
        expect(
            screen.getByText(
                'Spend by supplier report for Vendor Manager: A - No person assigned (101). For this financial year and last, excludes factors & VAT.'
            )
        ).toBeInTheDocument();
        expect(screen.getByText('Supplier Id')).toBeInTheDocument();
        expect(screen.getByText('TRUMPF LTD')).toBeInTheDocument();
        expect(screen.getByText('£324,249.50')).toBeInTheDocument();
        expect(screen.getByText('£61,484.98')).toBeInTheDocument();
        expect(screen.getByText('£62.10')).toBeInTheDocument();
        expect(screen.getByText('EXPORT')).toBeInTheDocument();
    });
});
