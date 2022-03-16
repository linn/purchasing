/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import { useLocation } from 'react-router-dom';
import '@testing-library/jest-dom/extend-expect';
import { cleanup, screen } from '@testing-library/react';
import render from '../../../test-utils';
import UnacknowledgedOrdersReport from '../../reports/UnacknowledgedOrdersReport';
import unacknowledgedOrdersReportActions from '../../../actions/unacknowledgedOrdersReportActions';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

jest.mock('react-router-dom', () => ({
    ...jest.requireActual('react-router-dom'),
    useLocation: jest.fn()
}));

const fetchUnacknowledgedOrdersReportActionsSpy = jest.spyOn(
    unacknowledgedOrdersReportActions,
    'fetchReport'
);

const initialState = {};

const stateWithReport = {
    ...initialState,
    unacknowledgedOrdersReport: {
        options: null,
        results: {
            loading: false,
            data: {
                title: {
                    displayString: 'Unacknowledged orders for R S COMPONENTS LTD',
                    drillDowns: []
                },
                resultType: null,
                reportValueType: 'Value',
                displaySequence: null,
                headers: {
                    rowHeader: null,
                    columnHeaders: [
                        'Order Number/Line',
                        'Part Number',
                        'Description',
                        'Delivery No',
                        'Qty',
                        'Unit Price',
                        'Requested Delivery'
                    ],
                    varianceRows: [],
                    varianceColumns: [],
                    totalColumns: [],
                    textColumns: [0, 1, 2, 3, 5, 6]
                },
                filterOptions: [],
                results: [
                    {
                        rowTitle: {
                            displayString: '827410/1/1',
                            drillDowns: []
                        },
                        rowSortOrder: 0,
                        values: [
                            {
                                displayValue: null,
                                textDisplayValue: '827410/1',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: null,
                                textDisplayValue: '1C-012',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: null,
                                textDisplayValue:
                                    'P/CLIPS 472-6403  FOR LP12 TURNTABLE  ***** LEAD FREE ******',
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
                                displayValue: 20.0,
                                textDisplayValue: null,
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: null,
                                textDisplayValue: '1.62',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: null,
                                textDisplayValue: '22-Nov-2021',
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
            }
        }
    }
};

const options = { search: '?supplierId=12345' };

describe('When component mounts...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(initialState));
        useLocation.mockImplementation(() => options);
        render(<UnacknowledgedOrdersReport />);
    });

    test('Initialisation actions are dispatched', () => {
        expect(fetchUnacknowledgedOrdersReportActionsSpy).toBeCalledTimes(1);
    });

    test('Should show back button', () => {
        expect(screen.getByText('Back')).toBeInTheDocument();
    });
});

describe('When report loaded...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithReport));
        render(<UnacknowledgedOrdersReport />);
    });

    test('Displays title', () => {
        expect(
            screen.getByText('Unacknowledged orders for R S COMPONENTS LTD')
        ).toBeInTheDocument();
    });

    test('Displays report data', () => {
        expect(screen.getByText('Order Number/Line')).toBeInTheDocument();
        expect(screen.getByText('827410/1')).toBeInTheDocument();
        expect(screen.getByText('Part Number')).toBeInTheDocument();
        expect(screen.getByText('1C-012')).toBeInTheDocument();
        expect(screen.getByText('Description')).toBeInTheDocument();
        expect(
            screen.getByText('P/CLIPS 472-6403 FOR LP12 TURNTABLE ***** LEAD FREE ******')
        ).toBeInTheDocument();
        expect(screen.getByText('Delivery No')).toBeInTheDocument();
        expect(screen.getByText('1')).toBeInTheDocument();
        expect(screen.getByText('Qty')).toBeInTheDocument();
        expect(screen.getByText('20')).toBeInTheDocument();
        expect(screen.getByText('Unit Price')).toBeInTheDocument();
        expect(screen.getByText('1.62')).toBeInTheDocument();
        expect(screen.getByText('Requested Delivery')).toBeInTheDocument();
        expect(screen.getByText('22-Nov-2021')).toBeInTheDocument();
    });
});
