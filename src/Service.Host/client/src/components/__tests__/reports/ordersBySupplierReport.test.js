/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import '@testing-library/jest-dom/extend-expect';
import { cleanup, screen } from '@testing-library/react';
import render from '../../../test-utils';
import OrdersBySupplier from '../../reports/OrdersBySupplier';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

const state = {
    oidc: { user: { profile: { name: 'User Name', employee: '/employees/33087' } } },
    router: { location: { pathname: '', query: { partId: 1, supplierId: 2 } } },
    ordersBySupplier: {
        options: {
            cancelled: 'N',
            credits: 'N',
            fromDate: '2021-12-25T09:40:37.465Z',
            id: '77442',
            outstanding: 'N',
            returns: 'N',
            stockControlled: 'A',
            toDate: '2022-01-25T09:40:37.465Z'
        },
        results: {
            data: {
                title: {
                    displayString: 'Purchase Orders By Supplier - 77442: RICHARD AUSTIN ALLOYS LTD',
                    drillDowns: []
                },
                resultType: null,
                reportValueType: 'Value',
                displaySequence: null,
                headers: {
                    rowHeader: null,
                    columnHeaders: [
                        'Order/Line',
                        'Part Number',
                        'Suppliers Designation',
                        'Qty Ordered',
                        'Qty Rec',
                        'Qty Inv',
                        'Net Total',
                        'Delivery',
                        'Qty',
                        'Req Date',
                        'Advised Date'
                    ],
                    varianceRows: [],
                    varianceColumns: [],
                    totalColumns: [],
                    textColumns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
                },
                filterOptions: [],
                results: [
                    {
                        rowTitle: {
                            displayString: '869190/1',
                            drillDowns: []
                        },
                        rowSortOrder: 0,
                        values: [
                            {
                                displayValue: null,
                                textDisplayValue: '869190/1',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: false,
                                drillDowns: []
                            },
                            {
                                displayValue: null,
                                textDisplayValue: 'RAW 231',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: null,
                                textDisplayValue:
                                    '25.0MM THICK ALUMINIUM 5083 POLYCOATED PLATE (437 X 362MM)',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: null,
                                textDisplayValue: '28',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: null,
                                textDisplayValue: '28',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: null,
                                textDisplayValue: '28',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: null,
                                textDisplayValue: '1786.4',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: null,
                                textDisplayValue: '1',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: null,
                                textDisplayValue: '28',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: null,
                                textDisplayValue: '17-Jan-2022',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: null,
                                textDisplayValue: '17-Jan-2022',
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
            },
            loading: false
        }
    }
};

describe('When component mounts...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(state));

        render(<OrdersBySupplier />);
    });

    test('Displays report title, column title, row data & export button...', () => {
        expect(
            screen.getByText('Purchase Orders By Supplier - 77442: RICHARD AUSTIN ALLOYS LTD')
        ).toBeInTheDocument();
        expect(screen.getByText('Order/Line')).toBeInTheDocument();
        expect(screen.getByText('869190/1')).toBeInTheDocument();
        expect(screen.getByText('RAW 231')).toBeInTheDocument();
        expect(screen.getByText('EXPORT')).toBeInTheDocument();
    });
});
