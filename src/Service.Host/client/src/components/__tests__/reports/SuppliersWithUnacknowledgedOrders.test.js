/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import '@testing-library/jest-dom/extend-expect';
import { cleanup, screen, fireEvent } from '@testing-library/react';
import render from '../../../test-utils';
import SuppliersWithUnacknowledgedOrders from '../../reports/SuppliersWithUnacknowledgedOrders';
import suppliersWithUnacknowledgedOrdersActions from '../../../actions/suppliersWithUnacknowledgedOrdersActions';
import vendorManagersActions from '../../../actions/vendorManagersActions';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

const fetchsuppliersWithUnacknowledgedOrdersActionsSpy = jest.spyOn(
    suppliersWithUnacknowledgedOrdersActions,
    'fetchReport'
);
const fetchVendorManagersSpy = jest.spyOn(vendorManagersActions, 'fetch');

const initialState = {
    suppliersWithUnacknowledgedOrders: {
        options: null
    }
};

const stateWithVendorManagers = {
    suppliersWithUnacknowledgedOrders: {
        options: null
    },
    vendorManagers: {
        items: [
            { vmId: 'A', userNumber: 123, name: '123' },
            { vmId: 'B', userNumber: 456, name: '456' }
        ],
        loading: false
    }
};

const stateWithReport = {
    ...initialState,
    vendorManagers: {
        items: [
            { vmId: 'A', userNumber: 123, name: '123' },
            { vmId: 'B', userNumber: 456, name: '456' }
        ],
        loading: false
    },
    suppliersWithUnacknowledgedOrders: {
        options: null,
        results: {
            loading: false,
            data: {
                title: {
                    displayString: 'Suppliers with unacknowledged orders',
                    drillDowns: []
                },
                resultType: null,
                reportValueType: 'Value',
                displaySequence: null,
                headers: {
                    rowHeader: null,
                    columnHeaders: ['Supplier Id', 'Supplier Name', '', ''],
                    varianceRows: [],
                    varianceColumns: [],
                    totalColumns: [],
                    textColumns: [0, 1, 2, 3]
                },
                filterOptions: [],
                results: [
                    {
                        rowTitle: {
                            displayString: '41193',
                            drillDowns: []
                        },
                        rowSortOrder: 0,
                        values: [
                            {
                                displayValue: null,
                                textDisplayValue: '41193',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: null,
                                textDisplayValue: 'ABACUS POLAR SCOTLAND LTD',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: null,
                                textDisplayValue: 'view',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: [
                                    {
                                        name: 'view',
                                        href: '/purchasing/reports/unacknowledged-orders?supplierId=41193'
                                    }
                                ]
                            },
                            {
                                displayValue: null,
                                textDisplayValue: 'csv',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: [
                                    {
                                        name: 'csv',
                                        href: '/purchasing/reports/unacknowledged-orders/export?supplierId=41193'
                                    }
                                ]
                            }
                        ],
                        rowType: 'Value'
                    },
                    {
                        rowTitle: {
                            displayString: '110945',
                            drillDowns: []
                        },
                        rowSortOrder: 1,
                        values: [
                            {
                                displayValue: null,
                                textDisplayValue: '110945',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: null,
                                textDisplayValue: 'ABERCORN ELECTRONICS LTD (USD ACCOUNT)',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: []
                            },
                            {
                                displayValue: null,
                                textDisplayValue: 'view',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: [
                                    {
                                        name: 'view',
                                        href: '/purchasing/reports/unacknowledged-orders?supplierId=110945'
                                    }
                                ]
                            },
                            {
                                displayValue: null,
                                textDisplayValue: 'csv',
                                prefix: null,
                                suffix: null,
                                decimalPlaces: null,
                                allowWrap: true,
                                drillDowns: [
                                    {
                                        name: 'csv',
                                        href: '/purchasing/reports/unacknowledged-orders/export?supplierId=110945'
                                    }
                                ]
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
        useSelector.mockImplementation(callback => callback(initialState));
        render(<SuppliersWithUnacknowledgedOrders />);
    });

    test('Initialisation actions are dispatched', () => {
        expect(fetchVendorManagersSpy).toBeCalledTimes(1);
    });

    test('Should not run report immediately', () => {
        expect(fetchsuppliersWithUnacknowledgedOrdersActionsSpy).not.toBeCalled();
    });

    test('Should show run button', () => {
        expect(screen.getByText('Run Report')).toBeInTheDocument();
    });
});

describe('When report loaded...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithReport));
        render(<SuppliersWithUnacknowledgedOrders />);
    });

    test('Displays title', () => {
        expect(screen.getByText('Suppliers with unacknowledged orders')).toBeInTheDocument();
    });

    test('Displays report data', () => {
        expect(screen.getByText('Supplier Id')).toBeInTheDocument();
        expect(screen.getByText('Supplier Name')).toBeInTheDocument();
        expect(screen.getByText('110945')).toBeInTheDocument();
        expect(screen.getByText('41193')).toBeInTheDocument();
        expect(screen.getByText('ABACUS POLAR SCOTLAND LTD')).toBeInTheDocument();
        expect(screen.getByText('ABERCORN ELECTRONICS LTD (USD ACCOUNT)')).toBeInTheDocument();
    });
});

describe('When vendor manager selected...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithVendorManagers));
        render(<SuppliersWithUnacknowledgedOrders />);
        const vmDropdown = screen.getByLabelText('Vendor Manager');
        fireEvent.change(vmDropdown, { target: { value: 'A' } });
    });

    test('Should update report', () => {
        expect(fetchsuppliersWithUnacknowledgedOrdersActionsSpy).toHaveBeenCalledWith({
            vendorManager: 'A',
            useSupplierGroup: true
        });
    });
});

describe('When run report clicked ...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithVendorManagers));
        render(<SuppliersWithUnacknowledgedOrders />);
        const runButton = screen.getByText('Run Report');
        fireEvent.click(runButton);
    });

    test('Should update report', () => {
        expect(fetchsuppliersWithUnacknowledgedOrdersActionsSpy).toHaveBeenCalledTimes(1);
    });
});
