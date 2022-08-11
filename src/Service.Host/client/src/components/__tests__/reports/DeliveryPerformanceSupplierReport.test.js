/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import { useLocation } from 'react-router';
import '@testing-library/jest-dom/extend-expect';
import { cleanup, screen } from '@testing-library/react';
import render from '../../../test-utils';
import DeliveryPerformanceSupplierReport from '../../reports/DeliveryPerformanceSupplierReport';
import deliveryPerformanceSupplierReportActions from '../../../actions/deliveryPerformanceSupplierReportActions';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

jest.mock('react-router', () => ({
    ...jest.requireActual('react-router'),
    useLocation: jest.fn()
}));

const fetchReportSpy = jest.spyOn(deliveryPerformanceSupplierReportActions, 'fetchReport');

const options = {
    search: '?startPeriod=1463&endPeriod=1463&vendorManager=A'
};

const initialState = {};
const stateWithReport = {
    deliveryPerformanceSupplierReport: {
        results: {
            loading: false,
            data: {
                title: { displayString: 'Delivery Performance By Supplier', drillDowns: [] },
                resultType: null,
                reportValueType: 'Value',
                displaySequence: null,
                headers: {
                    rowHeader: null,
                    columnHeaders: [
                        'Supplier Id',
                        'Supplier Name',
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
                    textColumns: []
                },
                filterOptions: [],
                results: [
                    {
                        rowTitle: { displayString: 'R S COMPONENTS LTD', drillDowns: [] },
                        rowSortOrder: 0,
                        values: [
                            {
                                displayValue: null,
                                textDisplayValue: '8538',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: null,
                                textDisplayValue: 'R S COMPONENTS LTD',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: 47.0,
                                textDisplayValue: null,
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: 39.0,
                                textDisplayValue: null,
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: 83.0,
                                textDisplayValue: null,
                                prefix: null,
                                suffix: '%',
                                decimalPlaces: 1,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: 0.0,
                                textDisplayValue: null,
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: 8.0,
                                textDisplayValue: null,
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: 0.0,
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
                            displayValue: 47.0,
                            textDisplayValue: null,
                            prefix: null,
                            suffix: null,
                            decimalPlaces: null,
                            allowWrap: true,
                            drillDowns: []
                        },
                        {
                            displayValue: 39.0,
                            textDisplayValue: null,
                            prefix: null,
                            suffix: null,
                            decimalPlaces: null,
                            allowWrap: true,
                            drillDowns: []
                        },
                        {
                            displayValue: 83.0,
                            textDisplayValue: null,
                            prefix: null,
                            suffix: '%',
                            decimalPlaces: 1,
                            allowWrap: true,
                            drillDowns: []
                        },
                        {
                            displayValue: 0.0,
                            textDisplayValue: null,
                            prefix: null,
                            suffix: null,
                            decimalPlaces: null,
                            allowWrap: true,
                            drillDowns: []
                        },
                        {
                            displayValue: 8.0,
                            textDisplayValue: null,
                            prefix: null,
                            suffix: null,
                            decimalPlaces: null,
                            allowWrap: true,
                            drillDowns: []
                        },
                        {
                            displayValue: 0.0,
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
            vendorManager: 'A'
        }
    }
};

describe('When component mounts...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(initialState));
        useLocation.mockReturnValue(options);
        render(<DeliveryPerformanceSupplierReport />);
    });

    test('Should fetch report', () => {
        expect(fetchReportSpy).toBeCalledTimes(1);
        expect(fetchReportSpy).toBeCalledWith({
            startPeriod: '1463',
            endPeriod: '1463',
            vendorManager: 'A'
        });
    });
});

describe('When report is returned...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithReport));
        render(<DeliveryPerformanceSupplierReport />);
    });

    test('Should show details', () => {
        expect(screen.getByText('Supplier Id')).toBeInTheDocument();
        expect(screen.getByText('Supplier Name')).toBeInTheDocument();
        expect(screen.getByText('No Of Deliveries')).toBeInTheDocument();
        expect(screen.getByText('No On Time')).toBeInTheDocument();
        expect(screen.getByText('% On Time')).toBeInTheDocument();
        expect(screen.getByText('No Early')).toBeInTheDocument();
        expect(screen.getByText('No Late')).toBeInTheDocument();
        expect(screen.getByText('No Unack')).toBeInTheDocument();
        expect(screen.getByText('8538')).toBeInTheDocument();
        expect(screen.getByText('R S COMPONENTS LTD')).toBeInTheDocument();
        expect(screen.getAllByText('47')).toHaveLength(2);
        expect(screen.getAllByText('39')).toHaveLength(2);
        expect(screen.getAllByText('83.0%')).toHaveLength(2);
        expect(screen.getAllByText('0')).toHaveLength(4);
        expect(screen.getAllByText('8')).toHaveLength(2);
    });
});
