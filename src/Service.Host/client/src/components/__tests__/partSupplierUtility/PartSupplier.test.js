/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import '@testing-library/jest-dom/extend-expect';
import { cleanup, screen, fireEvent } from '@testing-library/react';
import render from '../../../test-utils';
import PartSupplier from '../../partSupplierUtility/PartSupplier';
import partSuppliersActions from '../../../actions/partSuppliersActions';
import unitsOfMeasureActions from '../../../actions/unitsOfMeasureActions';
import deliveryAddressesActions from '../../../actions/deliveryAddressesActions';
import orderMethodsactions from '../../../actions/orderMethodActions';
import currenciesActions from '../../../actions/currenciesActions';
import packagingGroupActions from '../../../actions/packagingGroupActions';
import employeesActions from '../../../actions/employeesActions';
import partSupplierActions from '../../../actions/partSupplierActions';
import * as itemTypes from '../../../itemTypes';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

const fetchAppStateActionsSpy = jest.spyOn(partSuppliersActions, 'fetchState');
const fetchItemActionsSpy = jest.spyOn(partSupplierActions, 'fetchByHref');
const fetchUnitsOfMeasureActionsSpy = jest.spyOn(unitsOfMeasureActions, 'fetch');
const fetchDeliveryAddressesActionsSpy = jest.spyOn(deliveryAddressesActions, 'fetch');
const fetchOrderMethodsactionsSpy = jest.spyOn(orderMethodsactions, 'fetch');
const fetchCurrenciesActionsSpy = jest.spyOn(currenciesActions, 'fetch');
const fetchPackagingGroupActionsSpy = jest.spyOn(packagingGroupActions, 'fetch');
const fetchEmployeesActionsSpy = jest.spyOn(employeesActions, 'fetch');

const updateItemActionsSpy = jest.spyOn(partSupplierActions, 'update');
const addItemActionSpy = jest.spyOn(partSupplierActions, 'add');

const state = {
    oidc: { user: { profile: { name: 'User Name', employee: '/employees/33087' } } },
    router: { location: { pathname: '', query: { partId: 1, supplierId: 2 } } },
    partSupplier: {
        loading: false,
        item: { supplierName: '', partNumber: '', links: [] }
    }
};

const stateWithItemLoaded = {
    ...state,
    partSupplier: {
        loading: false,
        item: { supplierName: 'SUPPLIER', partNumber: 'PART', links: [] }
    }
};

const stateWithPart = {
    ...state,
    part: {
        item: {
            partNumber: 'A PART',
            manufacturers: [
                { partNumber: 'P1', manufacturerDescription: 'M1' },
                { partNumber: 'P2', manufacturerDescription: 'M2' }
            ],
            links: [{ rel: 'mechanical-sourcing-sheet', href: '/parts/sources/382' }]
        }
    },
    partSupplier: {
        loading: false,
        item: { supplierName: 'SUPPLIER', partNumber: 'PART', links: [] }
    }
};

const stateWithItemLoadedWhereUserCanEdit = {
    ...state,
    partSupplier: {
        loading: false,
        item: { supplierName: 'SUPPLIER', partNumber: 'PART', links: [{ rel: 'edit' }] }
    }
};

const stateWithItemLoadedWhereEditing = {
    ...state,
    partSupplier: {
        loading: false,
        editStatus: 'edit',
        item: { supplierName: 'SUPPLIER', partNumber: 'PART', links: [{ rel: 'edit' }] }
    }
};

const stateWhereCreating = {
    ...state,
    router: { location: { pathname: '/create', query: {} } },

    partSupplier: {
        loading: false,
        editStatus: 'create',
        item: { supplierName: 'SUPPLIER', partNumber: 'PART', links: [{ rel: 'create' }] }
    },
    partSuppliers: {
        applicationState: { links: [{ href: '/create', rel: 'create' }] }
    }
};

describe('When component mounts...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(state));
        render(<PartSupplier />);
    });

    test('Initialisation actions are dispatched...', () => {
        expect(fetchAppStateActionsSpy).toBeCalledTimes(1);
        expect(fetchUnitsOfMeasureActionsSpy).toBeCalledTimes(1);
        expect(fetchDeliveryAddressesActionsSpy).toBeCalledTimes(1);
        expect(fetchOrderMethodsactionsSpy).toBeCalledTimes(1);
        expect(fetchCurrenciesActionsSpy).toBeCalledTimes(1);
        expect(fetchPackagingGroupActionsSpy).toBeCalledTimes(1);
        expect(fetchEmployeesActionsSpy).toBeCalledTimes(1);
        expect(fetchItemActionsSpy).toHaveBeenCalledWith(
            `${itemTypes.partSupplier.uri}?partId=${1}&supplierId=${2}`
        );
    });
});

describe('When item loaded...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithItemLoaded));
        render(<PartSupplier />);
    });

    test('Page renders data...', () => {
        expect(screen.getByText('SUPPLIER')).toBeInTheDocument();
        expect(screen.getByText('PART')).toBeInTheDocument();
    });

    test('Page shows correct icon and tooltip...', async () => {
        fireEvent.mouseOver(screen.getByTestId('EditOffIcon'));
        expect(
            await screen.findByText('You do not have write access to Part Suppliers')
        ).toBeInTheDocument();
    });

    test('Save button should be disabled...', () => {
        const saveButton = screen.getByRole('button', { name: 'Save' });
        expect(saveButton).toBeDisabled();
    });
});

describe('When user can edit...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithItemLoadedWhereUserCanEdit));
        render(<PartSupplier />);
    });

    test('Page shows correct icon tooltip...', async () => {
        fireEvent.mouseOver(screen.getByTestId('ModeEditIcon'));
        expect(
            await screen.findByText('You have write access to Part Suppliers')
        ).toBeInTheDocument();
    });
});

describe('When item loading...', () => {
    beforeEach(() => {
        cleanup();
        useSelector.mockClear();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback =>
            callback({ ...state, partSupplier: { loading: true } })
        );
    });

    test('Should render loading spinner...', () => {
        render(<PartSupplier />);
        expect(screen.getByRole('progressbar')).toBeInTheDocument();
    });
});

describe('When editing...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithItemLoadedWhereEditing));
        render(<PartSupplier />);
    });

    test('Save button should be enabled...', () => {
        const saveButton = screen.getByRole('button', { name: 'Save' });
        expect(saveButton).not.toBeDisabled();
    });

    test('Should dispatch update action when save clicked...', () => {
        const saveButton = screen.getByRole('button', { name: 'Save' });
        fireEvent.click(saveButton);
        expect(updateItemActionsSpy).toHaveBeenCalledTimes(1);
        expect(updateItemActionsSpy).toHaveBeenCalledWith(
            null,
            expect.objectContaining({ partNumber: 'PART', supplierName: 'SUPPLIER' })
        );
    });
});

describe('When creating...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWhereCreating));
        render(<PartSupplier />);
    });

    test('Save button should be enabled...', () => {
        const saveButton = screen.getByRole('button', { name: 'Save' });
        expect(saveButton).not.toBeDisabled();
    });

    test('Should dispatch add action when save clicked...', () => {
        const saveButton = screen.getByRole('button', { name: 'Save' });
        fireEvent.click(saveButton);
        expect(addItemActionSpy).toHaveBeenCalledTimes(1);
        expect(addItemActionSpy).toHaveBeenCalledWith(
            expect.objectContaining({ createdBy: 33087 })
        );
    });
});

describe('When new part selected...', () => {
    beforeEach(() => {
        const stateWithPartSearchResults = {
            ...state,
            router: { location: { pathname: '/create', query: {} } },
            partSupplier: {
                loading: false
            },
            parts: {
                searchItems: [{ id: 1, partNumber: 'SOME PART', description: 'SOME DESC' }]
            }
        };
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithPartSearchResults));
        render(<PartSupplier />);
    });

    test('Should set designation and part description to be selected results description', () => {
        const input = screen.getByLabelText('Part');
        fireEvent.click(input);
        const result = screen.getByText('SOME PART');
        fireEvent.click(result);
        expect(screen.getAllByText('SOME DESC')).toHaveLength(2);
    });
});

describe('When clicking tabs...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithItemLoaded));
        render(<PartSupplier />);
    });

    test('Should render Part and Supplier tab', () => {
        const tab = screen.getByText('Part and Supplier');
        fireEvent.click(tab);
        expect(screen.getByLabelText('Designation')).toBeInTheDocument();
    });

    test('Should render Order Details tab', () => {
        const tab = screen.getByText('Order Details');
        fireEvent.click(tab);
        expect(screen.getByLabelText('Order Method')).toBeInTheDocument();
    });

    test('Should render Other Details tab', () => {
        const tab = screen.getByText('Other Details');
        fireEvent.click(tab);
        expect(screen.getByLabelText('Contract Lead Time Weeks')).toBeInTheDocument();
    });

    test('Should render Lifecycle tab', () => {
        const tab = screen.getByText('Lifecycle');
        fireEvent.click(tab);
        expect(screen.getByLabelText('Created By')).toBeInTheDocument();
    });

    test('Should render Manufacturer tab', () => {
        const tab = screen.getByText('Manufacturer');
        fireEvent.click(tab);
        expect(screen.getByLabelText('Manufacturer')).toBeInTheDocument();
    });
});

describe('When url query params specify tab...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
    });

    test('Should render Part and Supplier tab', () => {
        useSelector.mockImplementation(callback =>
            callback({
                ...stateWithItemLoaded,
                router: {
                    location: {
                        pathname: '',
                        query: { partId: 1, supplierId: 2, tab: 'partAndSupplier' }
                    }
                }
            })
        );
        render(<PartSupplier />);
        expect(screen.getByLabelText('Designation')).toBeInTheDocument();
    });

    test('Should render Order Details tab', () => {
        useSelector.mockImplementation(callback =>
            callback({
                ...stateWithItemLoaded,
                router: {
                    location: {
                        pathname: '',
                        query: { partId: 1, supplierId: 2, tab: 'orderDetails' }
                    }
                }
            })
        );
        render(<PartSupplier />);

        expect(screen.getByLabelText('Order Method')).toBeInTheDocument();
    });

    test('Should render Other Details tab', () => {
        useSelector.mockImplementation(callback =>
            callback({
                ...stateWithItemLoaded,
                router: {
                    location: {
                        pathname: '',
                        query: { partId: 1, supplierId: 2, tab: 'otherDetails' }
                    }
                }
            })
        );
        render(<PartSupplier />);
        expect(screen.getByLabelText('Contract Lead Time Weeks')).toBeInTheDocument();
    });

    test('Should render Lifecycle tab', () => {
        useSelector.mockImplementation(callback =>
            callback({
                ...stateWithItemLoaded,
                router: {
                    location: {
                        pathname: '',
                        query: { partId: 1, supplierId: 2, tab: 'lifecycle' }
                    }
                }
            })
        );
        render(<PartSupplier />);
        expect(screen.getByLabelText('Created By')).toBeInTheDocument();
    });

    test('Should render Manufacturer tab', () => {
        useSelector.mockImplementation(callback =>
            callback({
                ...stateWithItemLoaded,
                router: {
                    location: {
                        pathname: '',
                        query: { partId: 1, supplierId: 2, tab: 'manufacturer' }
                    }
                }
            })
        );
        render(<PartSupplier />);
        expect(screen.getByLabelText('Manufacturer')).toBeInTheDocument();
    });
});

describe('When part has manufacturers...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithPart));
        render(<PartSupplier />);
    });

    test('Should link to manufacturers', () => {
        expect(screen.getByRole('link', { name: 'Edit Manufacturers' })).toBeInTheDocument();
    });

    test('Should list manufacturers and their part numbers in the Manufacturers box', () => {
        expect(screen.getByText('M1 - P1 M2 - P2')).toBeInTheDocument();
    });
});
