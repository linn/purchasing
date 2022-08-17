/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import { useLocation } from 'react-router';
import '@testing-library/jest-dom/extend-expect';
import { cleanup, screen } from '@testing-library/react';
import render from '../../../test-utils';
import DeliveryPerformanceSummaryReport from '../../reports/DeliveryPerformanceSummaryReport';
import deliveryPerformanceSummaryReportActions from '../../../actions/deliveryPerformanceSummaryReportActions';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

jest.mock('react-router', () => ({
    ...jest.requireActual('react-router'),
    useLocation: jest.fn()
}));

const fetchReportSpy = jest.spyOn(deliveryPerformanceSummaryReportActions, 'fetchReport');

const options = {
    state: {
        startPeriod: 1466,
        endPeriod: 1472
    }
};

const initialState = {};
const stateWithReport = {
    deliveryPerformanceSummaryReport: {
        results: {
            loading: false,
            data: {
                title: {
                    displayString: 'Supplier Delivery Performance Summary',
                    drillDowns: []
                },
                resultType: null,
                reportValueType: 'Value',
                displaySequence: null,
                headers: {
                    rowHeader: null,
                    columnHeaders: [
                        'Month Name',
                        'No Of Deliveries',
                        'No On Time',
                        '% On Time',
                        'No Early',
                        'No Late',
                        'No Unack'
                    ],
                    varianceRows: [],
                    varianceColumns: [],
                    totalColumns: [],
                    textColumns: [0]
                },
                filterOptions: [],
                results: [
                    {
                        rowTitle: {
                            displayString: 'FEB2022',
                            drillDowns: []
                        },
                        rowSortOrder: 1466,
                        values: [
                            {
                                displayValue: null,
                                textDisplayValue: 'FEB2022',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: 10,
                                textDisplayValue: null,
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: 10,
                                textDisplayValue: null,
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: 100,
                                textDisplayValue: null,
                                prefix: null,
                                suffix: '%',
                                decimalPlaces: 1,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: 0,
                                textDisplayValue: null,
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: 0,
                                textDisplayValue: null,
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: 0,
                                textDisplayValue: null,
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
                        rowTitle: {
                            displayString: 'MAR2022',
                            drillDowns: []
                        },
                        rowSortOrder: 1467,
                        values: [
                            {
                                displayValue: null,
                                textDisplayValue: 'MAR2022',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: 6,
                                textDisplayValue: null,
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: 3,
                                textDisplayValue: null,
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: 50,
                                textDisplayValue: null,
                                prefix: null,
                                suffix: '%',
                                decimalPlaces: 1,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: 1,
                                textDisplayValue: null,
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: 1,
                                textDisplayValue: null,
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: 1,
                                textDisplayValue: null,
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
                            displayValue: 16,
                            textDisplayValue: null,
                            prefix: null,
                            suffix: null,
                            decimalPlaces: null,
                            allowWrap: true,
                            drillDowns: []
                        },
                        {
                            displayValue: 13,
                            textDisplayValue: null,
                            prefix: null,
                            suffix: null,
                            decimalPlaces: null,
                            allowWrap: true,
                            drillDowns: []
                        },
                        {
                            displayValue: 81.3,
                            textDisplayValue: null,
                            prefix: null,
                            suffix: '%',
                            decimalPlaces: 1,
                            allowWrap: true,
                            drillDowns: []
                        },
                        {
                            displayValue: 1,
                            textDisplayValue: null,
                            prefix: null,
                            suffix: null,
                            decimalPlaces: null,
                            allowWrap: true,
                            drillDowns: []
                        },
                        {
                            displayValue: 1,
                            textDisplayValue: null,
                            prefix: null,
                            suffix: null,
                            decimalPlaces: null,
                            allowWrap: true,
                            drillDowns: []
                        },
                        {
                            displayValue: 1,
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
            startPeriod: 1466,
            endPeriod: 1472
        }
    }
};

describe('When component mounts...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(initialState));
        useLocation.mockReturnValue(options);
        render(<DeliveryPerformanceSummaryReport />);
    });

    test('Should fetch report', () => {
        expect(fetchReportSpy).toBeCalledTimes(1);
        expect(fetchReportSpy).toBeCalledWith({ startPeriod: 1466, endPeriod: 1472 });
    });
});

describe('When report is returned...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithReport));
        render(<DeliveryPerformanceSummaryReport />);
    });

    test('Should show details', () => {
        expect(screen.getByText('Month Name')).toBeInTheDocument();
        expect(screen.getByText('No Of Deliveries')).toBeInTheDocument();
        expect(screen.getByText('No On Time')).toBeInTheDocument();
        expect(screen.getByText('% On Time')).toBeInTheDocument();
        expect(screen.getByText('No Early')).toBeInTheDocument();
        expect(screen.getByText('No Late')).toBeInTheDocument();
        expect(screen.getByText('No Unack')).toBeInTheDocument();
        expect(screen.getByText('FEB2022')).toBeInTheDocument();
        expect(screen.getByText('MAR2022')).toBeInTheDocument();
        expect(screen.getAllByText('10')).toHaveLength(2);
        expect(screen.getByText('100.0%')).toBeInTheDocument();
        expect(screen.getByText('6')).toBeInTheDocument();
        expect(screen.getByText('3')).toBeInTheDocument();
        expect(screen.getByText('50.0%')).toBeInTheDocument();
        expect(screen.getByText('81.3%')).toBeInTheDocument();
        expect(screen.getAllByText('0')).toHaveLength(3);
        expect(screen.getAllByText('1')).toHaveLength(6);
    });
});
