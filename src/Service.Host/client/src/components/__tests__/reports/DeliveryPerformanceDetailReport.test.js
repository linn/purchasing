/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import { useLocation } from 'react-router';
import '@testing-library/jest-dom/extend-expect';
import { cleanup, screen } from '@testing-library/react';
import render from '../../../test-utils';
import DeliveryPerformanceDetailReport from '../../reports/DeliveryPerformanceDetailReport';
import deliveryPerformanceDetailReportActions from '../../../actions/deliveryPerformanceDetailReportActions';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

jest.mock('react-router', () => ({
    ...jest.requireActual('react-router'),
    useLocation: jest.fn()
}));

const fetchReportSpy = jest.spyOn(deliveryPerformanceDetailReportActions, 'fetchReport');

const options = {
    search: '?startPeriod=1463&endPeriod=1463&supplierId=123'
};

const initialState = {};
const stateWithReport = {
    deliveryPerformanceDetailReport: {
        results: {
            loading: false,
            data: {
                title: { displayString: 'Delivery Performance Details', drillDowns: [] },
                resultType: null,
                reportValueType: 'Value',
                displaySequence: null,
                headers: {
                    rowHeader: null,
                    columnHeaders: [
                        'Order',
                        'Line',
                        'Del',
                        'Part Number',
                        'Requested',
                        'Advised',
                        'Arrived',
                        'Reason',
                        'On Time'
                    ],
                    varianceRows: [],
                    varianceColumns: [],
                    totalColumns: [],
                    textColumns: [0, 1, 2, 3, 4, 5, 6, 7, 8]
                },
                filterOptions: [],
                results: [
                    {
                        rowTitle: { displayString: '0', drillDowns: [] },
                        rowSortOrder: 0,
                        values: [
                            {
                                displayValue: null,
                                textDisplayValue: '745250',
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
                                textDisplayValue: '1',
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
                                textDisplayValue: '17-Nov-2008',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: null,
                                textDisplayValue: '17-Nov-2008',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: null,
                                textDisplayValue: '20-Dec-2021',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: null,
                                textDisplayValue: 'REQUESTED',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: null,
                                textDisplayValue: 'No',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            }
                        ],
                        rowType: 'Value'
                    },
                    {
                        rowTitle: { displayString: '21', drillDowns: [] },
                        rowSortOrder: 1,
                        values: [
                            {
                                displayValue: null,
                                textDisplayValue: '827409',
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
                                textDisplayValue: '1',
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
                                textDisplayValue: '22-Nov-2021',
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
                            },
                            {
                                displayValue: null,
                                textDisplayValue: '20-Dec-2021',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: null,
                                textDisplayValue: 'REQUESTED',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: null,
                                textDisplayValue: 'Yes',
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
                    rowTitle: { displayString: 'Totals', drillDowns: [] },
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
        },
        options: {
            startPeriod: 1463,
            endPeriod: 1463,
            supplierId: 123
        }
    }
};

describe('When component mounts...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(initialState));
        useLocation.mockReturnValue(options);
        render(<DeliveryPerformanceDetailReport />);
    });

    test('Should fetch report', () => {
        expect(fetchReportSpy).toBeCalledTimes(1);
        expect(fetchReportSpy).toBeCalledWith({
            startPeriod: '1463',
            endPeriod: '1463',
            supplierId: '123'
        });
    });
});

describe('When report is returned...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithReport));
        render(<DeliveryPerformanceDetailReport />);
    });

    test('Should show details', () => {
        expect(screen.getByText('Order')).toBeInTheDocument();
        expect(screen.getByText('Line')).toBeInTheDocument();
        expect(screen.getByText('Del')).toBeInTheDocument();
        expect(screen.getByText('Part Number')).toBeInTheDocument();
        expect(screen.getByText('Requested')).toBeInTheDocument();
        expect(screen.getByText('Advised')).toBeInTheDocument();
        expect(screen.getByText('Arrived')).toBeInTheDocument();
        expect(screen.getByText('Reason')).toBeInTheDocument();
        expect(screen.getByText('On Time')).toBeInTheDocument();
        expect(screen.getByText('745250')).toBeInTheDocument();
        expect(screen.getByText('Yes')).toBeInTheDocument();
        expect(screen.getByText('No')).toBeInTheDocument();
        expect(screen.getAllByText('17-Nov-2008')).toHaveLength(2);
        expect(screen.getAllByText('REQUESTED')).toHaveLength(2);
        expect(screen.getAllByText('1C-012')).toHaveLength(2);
    });
});
