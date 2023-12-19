/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import '@testing-library/jest-dom/extend-expect';
import { cleanup, screen, fireEvent } from '@testing-library/react';
import { useLocation } from 'react-router';
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
import partPriceConversionsActions from '../../../actions/partPriceConversionsActions';
import * as itemTypes from '../../../itemTypes';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

jest.mock('react-router', () => ({
    ...jest.requireActual('react-router'),
    useLocation: jest.fn()
}));

useLocation.mockImplementation(() => ({
    search: 'partId=1&supplierId=2'
}));

const fetchAppStateActionsSpy = jest.spyOn(partSuppliersActions, 'fetchState');
const fetchItemActionsSpy = jest.spyOn(partSupplierActions, 'fetchByHref');
const fetchUnitsOfMeasureActionsSpy = jest.spyOn(unitsOfMeasureActions, 'fetch');
const fetchDeliveryAddressesActionsSpy = jest.spyOn(deliveryAddressesActions, 'fetch');
const fetchOrderMethodsactionsSpy = jest.spyOn(orderMethodsactions, 'fetch');
const fetchCurrenciesActionsSpy = jest.spyOn(currenciesActions, 'fetch');
const fetchPackagingGroupActionsSpy = jest.spyOn(packagingGroupActions, 'fetch');
const fetchEmployeesActionsSpy = jest.spyOn(employeesActions, 'fetch');
const fetchPartPriceConversionsSpy = jest.spyOn(partPriceConversionsActions, 'fetchByHref');

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

const validitem = {
    supplierName: 'SUPPLIER',
    supplierId: 123,
    partNumber: 'PART',
    orderMethodName: 'METHOD',
    currencyCode: 'GBP',
    minimumOrderQty: 1,
    orderIncrement: 1,
    leadTimeWeeks: 1,
    damagesPercent: 1,
    links: [{ rel: 'edit' }]
};
const stateWithItemLoadedWhereUserCanEdit = {
    ...state,
    partSupplier: {
        loading: false,
        item: validitem
    }
};

const stateWithItemLoadedWhereEditing = {
    ...state,
    partSupplier: {
        loading: false,
        editStatus: 'edit',
        item: validitem
    }
};

const stateWhereCreating = {
    ...state,
    partSupplier: {
        loading: false,
        editStatus: 'create',
        item: null
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
        fireEvent.mouseEnter(screen.getByTestId('EditOffIcon'));
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
        fireEvent.mouseEnter(screen.getByTestId('ModeEditIcon'));
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
        render(<PartSupplier creating />);
    });

    test('Save button should be disabled since no data...', () => {
        const saveButton = screen.getByRole('button', { name: 'Save' });
        expect(saveButton).toBeDisabled();
    });

    test('Preferred Supplier button should be disabled', () => {
        expect(screen.getByRole('button', { name: 'Preferred Supplier' })).toBeDisabled();
    });

    test('Change prices button should be disabled', () => {
        const tab = screen.getByText('Order Details');
        fireEvent.click(tab);
        expect(screen.getByRole('button', { name: 'Change Prices' })).toBeDisabled();
    });
});

describe('When save clicked when creating...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback =>
            callback({
                ...stateWhereCreating,
                parts: {
                    searchItems: [{ id: 1, partNumber: 'SOME PART', description: 'SOME DESC' }]
                },
                suppliers: {
                    searchItems: [{ id: 123, name: 'SOME SUPPLIER' }]
                },
                orderMethods: { items: [{ name: 'METHOD', description: 'METHOD' }] },
                currencies: {
                    items: [
                        { code: 'GBP', name: 'Sterling' },
                        { code: 'USD', name: 'Dollar Dollar Bills' }
                    ]
                }
            })
        );
        render(<PartSupplier creating />);
    });
    test('Should dispatch add action when save clicked...', async () => {
        // enter required fields

        // Part Supplier Tab
        let input = screen.getByLabelText('Part');
        fireEvent.click(input);
        fireEvent.keyDown(input, { key: 'Enter', keyCode: 13, charCode: 13 });
        let result = screen.getByRole('button', { name: 'SOME PART SOME DESC' });
        fireEvent.click(result);

        input = screen.getByLabelText('Supplier');
        fireEvent.click(input);
        fireEvent.keyDown(input, { key: 'Enter', keyCode: 13, charCode: 13 });
        result = screen.getByRole('button', { name: '123 SOME SUPPLIER open' });
        fireEvent.click(result);

        // Order Details tab
        let tab = screen.getByText('Order Details');
        fireEvent.click(tab);

        let dropdown = screen.getByLabelText('Order Method');
        fireEvent.change(dropdown, { target: { value: 'METHOD' } });

        dropdown = screen.getByLabelText('Currency');
        fireEvent.change(dropdown, { target: { value: 'USD' } });
        input = screen.getByLabelText('Minimum Order Qty');
        fireEvent.change(input, { target: { value: 111 } });

        input = screen.getByLabelText('Order Increment');
        fireEvent.change(input, { target: { value: 222 } });

        // Other Details tab
        tab = screen.getByText('Other Details');
        fireEvent.click(tab);

        input = screen.getByLabelText('Lead Time Weeks');
        fireEvent.change(input, { target: { value: 333 } });

        const saveButton = await screen.findByRole('button', { name: 'Save' });
        fireEvent.click(saveButton);

        expect(addItemActionSpy).toHaveBeenCalledTimes(1);
        expect(addItemActionSpy).toHaveBeenCalledWith(
            expect.objectContaining({
                createdBy: 33087,
                partNumber: 'SOME PART',
                orderMethodName: 'METHOD',
                currencyCode: 'USD',
                unitOfMeasure: 'ONES',
                minimumOrderQty: 111,
                orderIncrement: 222,
                leadTimeWeeks: 333
            })
        );
    });
});

describe('When new part selected...', () => {
    beforeEach(() => {
        const stateWithPartSearchResults = {
            ...state,
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
        render(<PartSupplier creating />);
    });

    test('Should set designation and part description to be selected results description', () => {
        const input = screen.getByLabelText('Part');
        fireEvent.click(input);
        fireEvent.keyDown(input, { key: 'Enter', keyCode: 13, charCode: 13 });

        const result = screen.getByText('SOME PART');
        fireEvent.click(result);
        expect(screen.getAllByText('SOME DESC')).toHaveLength(2);
    });
});

describe('When new currency unit price entered', () => {
    beforeEach(() => {
        const stateWithPartSearchResults = {
            ...state,
            partSupplier: {
                loading: false
            }
        };
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithPartSearchResults));
        render(<PartSupplier creating />);
        const tab = screen.getByText('Order Details');
        fireEvent.click(tab);
    });

    test('Should request part price conversions', () => {
        const input = screen.getByLabelText('Currency Unit Price');
        input.focus();
        fireEvent.change(input, { target: { value: 123 } });
        input.blur();
        expect(fetchPartPriceConversionsSpy).toHaveBeenCalledWith(
            `${
                itemTypes.partPriceConversions.uri
            }?partNumber=&newPrice=${123}&newCurrency=GBP&ledger=PL&round=FALSE`
        );
    });
});

describe('When currency changed', () => {
    beforeEach(() => {
        const stateWithCurrencies = {
            ...stateWhereCreating,
            currencies: {
                items: [
                    { code: 'GBP', name: 'Sterling' },
                    { code: 'USD', name: 'Dollar Dollar Bills' }
                ]
            },
            parts: {
                searchItems: [{ id: 1, partNumber: 'SOME PART', description: 'SOME DESC' }]
            }
        };
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithCurrencies));
        render(<PartSupplier />);
        const tab = screen.getByText('Order Details');
        fireEvent.click(tab);
        const input = screen.getByLabelText('Currency Unit Price');
        fireEvent.change(input, { target: { value: 123 } });
    });

    test('Should request part price conversions', () => {
        const dropdown = screen.getByLabelText('Currency');
        fireEvent.change(dropdown, { target: { value: 'USD' } });
        expect(fetchPartPriceConversionsSpy).toHaveBeenCalledWith(
            `${
                itemTypes.partPriceConversions.uri
            }?partNumber=&newPrice=${123}&newCurrency=USD&ledger=PL&round=FALSE`
        );
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
        expect(screen.getByLabelText('Lead Time Weeks')).toBeInTheDocument();
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
        useLocation.mockImplementation(() => ({
            search: 'partId=1&supplierId=2&tab=partAndSupplier'
        }));
        useSelector.mockImplementation(callback => callback(stateWithItemLoaded));
        render(<PartSupplier />);
        expect(screen.getByLabelText('Designation')).toBeInTheDocument();
    });

    test('Should render Order Details tab', () => {
        useLocation.mockImplementation(() => ({
            search: 'partId=1&supplierId=2&tab=orderDetails'
        }));
        useSelector.mockImplementation(callback => callback(stateWithItemLoaded));
        render(<PartSupplier />);

        expect(screen.getByLabelText('Order Method')).toBeInTheDocument();
    });

    test('Should render Other Details tab', () => {
        useLocation.mockImplementation(() => ({
            search: 'partId=1&supplierId=2&tab=otherDetails'
        }));
        useSelector.mockImplementation(callback => callback(stateWithItemLoaded));
        render(<PartSupplier />);
        expect(screen.getByLabelText('Lead Time Weeks')).toBeInTheDocument();
    });

    test('Should render Lifecycle tab', () => {
        useLocation.mockImplementation(() => ({
            search: 'partId=1&supplierId=2&tab=lifecycle'
        }));
        useSelector.mockImplementation(callback => callback(stateWithItemLoaded));
        render(<PartSupplier />);
        expect(screen.getByLabelText('Created By')).toBeInTheDocument();
    });

    test('Should render Manufacturer tab', () => {
        useLocation.mockImplementation(() => ({
            search: 'partId=1&supplierId=2&tab=manufacturer'
        }));

        useSelector.mockImplementation(callback => callback(stateWithItemLoaded));
        render(<PartSupplier />);
        expect(screen.getByLabelText('Manufacturer')).toBeInTheDocument();
    });
});

describe('When part has manufacturers...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithPart));
        useLocation.mockImplementation(() => ({
            search: 'partId=1&supplierId=2'
        }));
        render(<PartSupplier />);
    });

    test('Should link to manufacturers', () => {
        expect(screen.getByRole('link', { name: 'Edit Manufacturers' })).toBeInTheDocument();
    });

    test('Should list manufacturers and their part numbers in the Manufacturers box', () => {
        expect(screen.getByText('M1 - P1 M2 - P2')).toBeInTheDocument();
    });
});

describe('When no edit link...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        const stateWithNoEditLink = {
            ...state,
            partSupplier: { item: { links: [] } }
        };
        useLocation.mockImplementation(() => ({
            search: 'partId=1&supplierId=2'
        }));
        useSelector.mockImplementation(callback => callback(stateWithNoEditLink));
        render(<PartSupplier />);
    });

    test('Preferred Supplier button should be disabled', () => {
        expect(screen.getByRole('button', { name: 'Preferred Supplier' })).toBeDisabled();
    });

    test('Change prices button should be disabled', () => {
        const tab = screen.getByText('Order Details');
        fireEvent.click(tab);
        expect(screen.getByRole('button', { name: 'Change Prices' })).toBeDisabled();
    });
});

describe('When edit link...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        const stateWithEditLink = {
            ...state,
            partSupplier: { item: { links: [{ rel: 'edit', href: '/edit' }] } }
        };
        useSelector.mockImplementation(callback => callback(stateWithEditLink));
        render(<PartSupplier />);
    });

    test('Preferred Supplier button should be enabled', () => {
        expect(screen.getByRole('button', { name: 'Preferred Supplier' })).not.toBeDisabled();
    });

    test('Change prices button should be enabled', () => {
        const tab = screen.getByText('Order Details');
        fireEvent.click(tab);
        expect(screen.getByRole('button', { name: 'Change Prices' })).not.toBeDisabled();
    });
});
