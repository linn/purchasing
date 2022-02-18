/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import '@testing-library/jest-dom/extend-expect';
import { cleanup, screen } from '@testing-library/react';
import render from '../../../test-utils';
import SpendByPart from '../../reports/SpendByPart';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

const state = {
    oidc: { user: { profile: { name: 'User Name', employee: '/employees/33087' } } },
    router: { location: { pathname: '', query: { id: 89351 } } },
    spendByPartReport: {
        options: {
            id: 89351
        },
        results: {
            loading: false,
            data: {
                title: {
                    displayString:
                        'Spend by part report for Supplier: EXMEL SOLUTIONS  LTD (89351)',
                    drillDowns: []
                },
                resultType: null,
                reportValueType: 'Value',
                displaySequence: null,
                headers: {
                    rowHeader: null,
                    columnHeaders: [
                        'PartNumber',
                        'Description',
                        'Last Year',
                        'This Year',
                        'This Month'
                    ],
                    varianceRows: [],
                    varianceColumns: [],
                    totalColumns: [],
                    textColumns: [0, 1]
                },
                filterOptions: [],
                results: [
                    {
                        rowTitle: {
                            displayString: 'SUNDRY',
                            drillDowns: []
                        },
                        rowSortOrder: 0,
                        values: [
                            {
                                displayValue: null,
                                textDisplayValue: 'Part 007',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: false,
                                drillDowns: []
                            },
                            {
                                displayValue: null,
                                textDisplayValue: 'Jimmy Bond action figure',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: 3439.67,
                                textDisplayValue: null,
                                prefix: null,
                                suffix: null,
                                decimalPlaces: 2,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: 547.56,
                                textDisplayValue: null,
                                prefix: null,
                                suffix: null,
                                decimalPlaces: 2,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: 0,
                                textDisplayValue: null,
                                prefix: null,
                                suffix: null,
                                decimalPlaces: 2,
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
                            displayValue: 3439.67,
                            textDisplayValue: null,
                            prefix: null,
                            suffix: null,
                            decimalPlaces: 2,
                            allowWrap: true,
                            drillDowns: []
                        },
                        {
                            displayValue: 547.56,
                            textDisplayValue: null,
                            prefix: null,
                            suffix: null,
                            decimalPlaces: 2,
                            allowWrap: true,
                            drillDowns: []
                        },
                        {
                            displayValue: 0,
                            textDisplayValue: null,
                            prefix: null,
                            suffix: null,
                            decimalPlaces: 2,
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

        render(<SpendByPart />);
    });

    test('Displays report title, column title, row data & export button...', () => {
        expect(screen.getByText('Part 007')).toBeInTheDocument();
        expect(screen.getByText('Jimmy Bond action figure')).toBeInTheDocument();
        expect(screen.getByText('Export')).toBeInTheDocument();
    });
});
