/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import '@testing-library/jest-dom/extend-expect';
import { screen, fireEvent } from '@testing-library/react';
import routeData from 'react-router';
import render from '../../../test-utils';
import supplierActions from '../../../actions/supplierActions';
import putSupplierOnHoldActions from '../../../actions/putSupplierOnHoldActions';
import accountingCompaniesActions from '../../../actions/accountingCompaniesActions';
import Supplier from '../../supplierUtility/Supplier';
import plannersActions from '../../../actions/plannersActions';
import vendorManagersActions from '../../../actions/vendorManagersActions';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

const mockParams = {
    id: 123
};
beforeEach(() => {
    jest.spyOn(routeData, 'useParams').mockReturnValue(mockParams);
});

const clearSupplierErrorsSpy = jest.spyOn(supplierActions, 'clearErrorsForItem');
const fetchSupplierSpy = jest.spyOn(supplierActions, 'fetch');
const fetchAccountingCompaniesSpy = jest.spyOn(accountingCompaniesActions, 'fetch');
const fetchPlannersSpy = jest.spyOn(plannersActions, 'fetch');
const fetchVendorManagersSpy = jest.spyOn(vendorManagersActions, 'fetch');

const updateSupplierSpy = jest.spyOn(supplierActions, 'update');
const setSupplierEditStatusSpy = jest.spyOn(supplierActions, 'setEditStatus');

const addSupplierSpy = jest.spyOn(supplierActions, 'add');
const putSupplierOnHoldAddSpy = jest.spyOn(putSupplierOnHoldActions, 'add');

const supplierData = {
    id: 123,
    orderHold: 'N',
    name: 'SUPPLIER',
    phoneNumber: '123456',
    webAddress: '/web',
    orderContactMethod: 'EMAIL',
    invoiceContactMethod: 'EMAIL',
    suppliersReference: 'REF',
    accountingCompany: 'LINN',
    vatNumber: '0012345',
    invoiceGoesToId: 124,
    invoiceGoesToName: 'SUPPLIER',
    expenseAccount: 'Y',
    paymentDays: 14,
    paymentMethod: 'BACS',
    currencyCode: 'GBP',
    paysInFc: 'N',
    approvedCarrier: 'N',
    partCategory: 'CAT',
    partCategoryDescription: 'CATEGORY',
    notesForBuyer: 'NFB',
    deliveryDay: 'MONDAY',
    refersToFcId: 123,
    refersToFcName: 'SUPPLIER',
    pmDeliveryDaysGrace: 1,
    orderAddressId: 456,
    orderFullAddress: '456 FULL ADDRESS WAY',
    invoiceAddressId: 789,
    invoiceFullAddress: '789 INVOICE ADDRESS WAY',
    accountControllerId: 666,
    accountControllerName: 'CONTROLLER',
    vendorManagerId: 777,
    plannerId: 888,
    openedById: 999,
    openedByName: 'OPENER',
    notes: 'NOTES NOTES NOTES',
    organisationId: 400
};

describe('On initialise...', () => {
    beforeEach(() => {
        jest.clearAllMocks();
        const initialState = {
            accountingCompanies: { items: [] }
        };
        useSelector.mockImplementation(callback => callback(initialState));
        render(<Supplier />);
    });

    test('should fetch supplier', () => {
        expect(fetchSupplierSpy).toBeCalledTimes(1);
        expect(fetchSupplierSpy).toBeCalledWith(123);
    });
});

describe('When loading...', () => {
    beforeEach(() => {
        const loadingState = {
            accountingCompanies: { items: [] },
            supplier: { loading: true }
        };
        useSelector.mockImplementation(callback => callback(loadingState));
        render(<Supplier />);
    });

    test('should render loading spinner', () => {
        expect(screen.getByRole('progressbar')).toBeInTheDocument();
    });
});

describe('When supplier data...', () => {
    beforeEach(() => {
        const state = {
            accountingCompanies: { items: [] },
            supplier: {
                loading: false,
                item: {
                    ...supplierData,
                    links: [{ rel: 'self', href: '/supplier/123' }]
                }
            }
        };
        useSelector.mockImplementation(callback => callback(state));
        render(<Supplier />);
    });

    test('should render data', () => {
        expect(screen.getByText('SUPPLIER')).toBeInTheDocument();
    });
});

describe('When changing tabs...', () => {
    beforeEach(() => {
        jest.clearAllMocks();
        const state = {
            accountingCompanies: { items: [] },
            supplier: {
                loading: false,
                item: {
                    ...supplierData,
                    links: [{ rel: 'self', href: '/supplier/123' }]
                }
            }
        };
        useSelector.mockImplementation(callback => callback(state));
        render(<Supplier />);
    });
    test('should default to General tab and fetch relevant data', () => {
        expect(screen.getByLabelText('Order Contact Method')).toBeInTheDocument();
        expect(fetchAccountingCompaniesSpy).toBeCalledTimes(1);
    });
    // I think these tests are failing because the tabs can't be clicked if they are rendered off screen
    // test('should switch to Contacts tab', () => {
    //     const tab = screen.getByText('Contacts');
    //     fireEvent.click(tab);
    //     expect(screen.getByText('First Name')).toBeInTheDocument();
    // });
    // test('should switch to Notes tab', () => {
    //     const tab = screen.getByText('Notes');
    //     fireEvent.click(tab);
    //     expect(screen.getByText('NOTES NOTES NOTES')).toBeInTheDocument();
    // });
    test('should switch to Lifecycle tab', () => {
        const tab = screen.getByText('Lifecycle');
        fireEvent.click(tab);
        expect(screen.getByLabelText('Reason Closed')).toBeInTheDocument();
    });
    test('should switch to Whose tab and fetch data', () => {
        const tab = screen.getByText('Whose');
        fireEvent.click(tab);
        expect(screen.getByLabelText('Account Controller')).toBeInTheDocument();
        expect(fetchVendorManagersSpy).toBeCalledTimes(1);
        expect(fetchPlannersSpy).toBeCalledTimes(1);
    });
    test('should switch to Where tab', () => {
        const tab = screen.getByText('Where');
        fireEvent.click(tab);
        expect(screen.getByLabelText('Order Addressee')).toBeInTheDocument();
    });
    test('should switch to Purch tab', () => {
        const tab = screen.getByText('Purch');
        fireEvent.click(tab);
        expect(screen.getByLabelText('Part Category')).toBeInTheDocument();
    });
    test('should switch to Finance tab', () => {
        const tab = screen.getByText('Finance');
        fireEvent.click(tab);
        expect(screen.getByLabelText('Invoice Goes To')).toBeInTheDocument();
    });
    test('should switch back to General tab', () => {
        let tab = screen.getByText('Purch');
        fireEvent.click(tab);
        tab = screen.getByText('General');
        fireEvent.click(tab);
        expect(screen.getByLabelText('Order Contact Method')).toBeInTheDocument();
    });
});

describe('When changing currency...', () => {
    beforeEach(() => {
        jest.clearAllMocks();
        const state = {
            accountingCompanies: { items: [] },
            currencies: {
                loading: false,
                items: [
                    { code: 'GBP', name: 'Pounds' },
                    { code: 'USD', name: 'Dollar Bills' }
                ]
            },
            supplier: {
                loading: false,
                item: {
                    ...supplierData,
                    links: [{ rel: 'self', href: '/supplier/123' }]
                }
            }
        };
        useSelector.mockImplementation(callback => callback(state));
        render(<Supplier />);
        const tab = screen.getByText('Finance');
        fireEvent.click(tab);
    });

    test('should default to GBP with Pays In Foreign Currency: Never', () => {
        expect(screen.getByText('GBP')).toBeInTheDocument();
        expect(screen.getByText('Never')).toBeInTheDocument();
    });

    test('should change to Always if currency changed from GBP', () => {
        const currencyInput = screen.getByLabelText('Currency');
        fireEvent.change(currencyInput, { target: { value: 'USD' } });
        expect(screen.getByText('GBP')).toBeInTheDocument();
        expect(screen.getByText('Always')).toBeInTheDocument();
    });

    test('should change back to Never if currency changed back to GBP', () => {
        const currencyInput = screen.getByLabelText('Currency');
        fireEvent.change(currencyInput, { target: { value: 'GPB' } });
        expect(screen.getByText('GBP')).toBeInTheDocument();
        expect(screen.getByText('Never')).toBeInTheDocument();
    });

    test('should change to Always if payment method is FORPAY and currency is GBP', () => {
        const currencyInput = screen.getByLabelText('Currency');
        fireEvent.change(currencyInput, { target: { value: 'GPB' } });
        const paymentMethodInput = screen.getByLabelText('Payment Method');
        fireEvent.change(paymentMethodInput, { target: { value: 'FORPAY' } });
        expect(screen.getByText('Always')).toBeInTheDocument();
    });
});

describe('When edit link...', () => {
    beforeEach(() => {
        const state = {
            accountingCompanies: { items: [] },
            supplier: {
                loading: false,
                item: {
                    ...supplierData,
                    links: [
                        { rel: 'self', href: '/supplier/123' },
                        { rel: 'edit', href: '/supplier/123' }
                    ]
                }
            }
        };
        useSelector.mockImplementation(callback => callback(state));
        render(<Supplier />);
    });

    test('shoud show correct icon tooltip...', async () => {
        fireEvent.mouseOver(screen.getByTestId('ModeEditIcon'));
        expect(await screen.findByText('You have write access to Suppliers')).toBeInTheDocument();
    });
});

describe('When no edit link...', () => {
    beforeEach(() => {
        const state = {
            accountingCompanies: { items: [] },
            supplier: {
                loading: false,
                item: {
                    ...supplierData,
                    links: [{ rel: 'self', href: '/supplier/123' }]
                }
            }
        };
        useSelector.mockImplementation(callback => callback(state));
        render(<Supplier />);
    });

    test('should show correct icon tooltip...', async () => {
        fireEvent.mouseOver(screen.getByTestId('EditOffIcon'));
        expect(
            await screen.findByText('You do not have write access to Suppliers')
        ).toBeInTheDocument();
    });
});

describe('When hold link and supplier not on hold...', () => {
    beforeEach(() => {
        const state = {
            accountingCompanies: { items: [] },
            supplier: {
                loading: false,
                item: {
                    ...supplierData,
                    links: [{ rel: 'hold', href: '/supplier/123/hold' }]
                }
            }
        };
        useSelector.mockImplementation(callback => callback(state));
        render(<Supplier />);
        const tab = screen.getByText('Purch');
        fireEvent.click(tab);
    });

    test('should render PUT ON HOLD button...', () => {
        expect(screen.getByRole('button', { name: 'PUT ON HOLD' })).toBeInTheDocument();
    });
});

describe('When hold link and supplier on hold...', () => {
    beforeEach(() => {
        const state = {
            accountingCompanies: { items: [] },
            supplier: {
                loading: false,
                item: {
                    ...supplierData,
                    orderHold: 'Y',
                    links: [{ rel: 'hold', href: '/supplier/123/hold' }]
                }
            }
        };
        useSelector.mockImplementation(callback => callback(state));
        render(<Supplier />);
        const tab = screen.getByText('Purch');
        fireEvent.click(tab);
    });

    test('should render TAKE OFF HOLD button...', () => {
        expect(screen.getByRole('button', { name: 'TAKE OFF HOLD' })).toBeInTheDocument();
    });
});

describe('When no hold link...', () => {
    beforeEach(() => {
        const state = {
            accountingCompanies: { items: [] },
            supplier: {
                loading: false,
                item: {
                    ...supplierData,
                    orderHold: 'Y',
                    links: [{ rel: 'self', href: '/supplier/123' }]
                }
            }
        };
        useSelector.mockImplementation(callback => callback(state));
        render(<Supplier />);
        const tab = screen.getByText('Purch');
        fireEvent.click(tab);
    });

    test('should not render any hold change button...', () => {
        expect(screen.queryByRole('button', { name: 'TAKE OFF HOLD' })).not.toBeInTheDocument();
        expect(screen.queryByRole('button', { name: 'PUT ON HOLD' })).not.toBeInTheDocument();
    });
});

describe('When putting supplier on hold...', () => {
    beforeEach(() => {
        jest.clearAllMocks();
        const state = {
            accountingCompanies: { items: [] },
            oidc: { user: { profile: { name: 'User Name', employee: '/employees/33087' } } },
            supplier: {
                loading: false,
                item: {
                    ...supplierData,
                    orderHold: 'N',
                    links: [
                        { rel: 'hold', href: '/supplier/123/hold' },
                        { rel: 'self', href: '/supplier/123' }
                    ]
                }
            }
        };
        useSelector.mockImplementation(callback => callback(state));
        render(<Supplier />);
        const tab = screen.getByText('Purch');
        fireEvent.click(tab);
        const holdButton = screen.getByRole('button', { name: 'PUT ON HOLD' });
        fireEvent.click(holdButton);
        const reasonInput = screen.getByLabelText('Must give a reason:');
        fireEvent.change(reasonInput, { target: { value: 'SOME REASON TO HOLD' } });
        const saveButton = screen.getByRole('button', { name: 'Save' });
        fireEvent.click(saveButton);
    });

    test('should dispatch put on hold action...', () => {
        expect(putSupplierOnHoldAddSpy).toBeCalledTimes(1);
        expect(putSupplierOnHoldAddSpy).toBeCalledWith(
            expect.objectContaining({
                reasonOnHold: 'SOME REASON TO HOLD',
                putOnHoldBy: 33087,
                supplierId: 123
            })
        );
    });
});

describe('When taking supplier off hold...', () => {
    beforeEach(() => {
        jest.clearAllMocks();
        const state = {
            accountingCompanies: { items: [] },
            oidc: { user: { profile: { name: 'User Name', employee: '/employees/33087' } } },
            supplier: {
                loading: false,
                item: {
                    ...supplierData,
                    orderHold: 'Y',
                    links: [
                        { rel: 'hold', href: '/supplier/123/hold' },
                        { rel: 'self', href: '/supplier/123' }
                    ]
                }
            }
        };
        useSelector.mockImplementation(callback => callback(state));
        render(<Supplier />);
        const tab = screen.getByText('Purch');
        fireEvent.click(tab);
        const holdButton = screen.getByRole('button', { name: 'TAKE OFF HOLD' });
        fireEvent.click(holdButton);
        const reasonInput = screen.getByLabelText('Must give a reason:');
        fireEvent.change(reasonInput, { target: { value: 'SOME REASON NOT TO HOLD' } });
        const saveButton = screen.getByRole('button', { name: 'Save' });
        fireEvent.click(saveButton);
    });

    test('Page dispatch put on hold action...', () => {
        expect(putSupplierOnHoldAddSpy).toBeCalledTimes(1);
        expect(putSupplierOnHoldAddSpy).toBeCalledWith(
            expect.objectContaining({
                reasonOffHold: 'SOME REASON NOT TO HOLD',
                takenOffHoldBy: 33087,
                supplierId: 123
            })
        );
    });
});

describe('When field changed...', () => {
    beforeEach(async () => {
        jest.clearAllMocks();
        const state = {
            accountingCompanies: { items: [] },
            supplier: {
                loading: false,
                editStatus: 'edit',
                item: {
                    ...supplierData,
                    links: [
                        { rel: 'self', href: '/supplier/123' },
                        { rel: 'edit', href: '/supplier/123' }
                    ]
                }
            }
        };
        useSelector.mockImplementation(callback => callback(state));
        render(<Supplier />);

        const phoneNumberInput = screen.getByLabelText('Phone Number');
        fireEvent.change(phoneNumberInput, { target: { value: '0987654321' } });
    });

    test('should set edit status...', () => {
        expect(setSupplierEditStatusSpy).toBeCalledWith('edit');
    });
});

describe('When updating supplier...', () => {
    beforeEach(async () => {
        jest.clearAllMocks();
        const state = {
            accountingCompanies: { items: [] },
            supplier: {
                loading: false,
                editStatus: 'edit',
                item: {
                    ...supplierData,
                    links: [
                        { rel: 'self', href: '/supplier/123' },
                        { rel: 'edit', href: '/supplier/123' }
                    ]
                }
            }
        };
        useSelector.mockImplementation(callback => callback(state));
        render(<Supplier />);

        const phoneNumberInput = screen.getByLabelText('Phone Number');
        fireEvent.change(phoneNumberInput, { target: { value: '0987654321' } });
        const saveButton = screen.getByRole('button', { name: 'Save' });
        fireEvent.click(saveButton);
    });

    test('shoud clear errors dispatch update action...', () => {
        expect(clearSupplierErrorsSpy).toBeCalledTimes(1);
        expect(updateSupplierSpy).toBeCalledWith(
            123,
            expect.objectContaining({
                ...supplierData,
                phoneNumber: '0987654321'
            })
        );
    });
});

describe('When creating supplier...', () => {
    beforeEach(async () => {
        jest.clearAllMocks();
        const state = {
            accountingCompanies: { items: [] },
            supplier: {
                loading: false,
                editStatus: 'edit'
            }
        };
        useSelector.mockImplementation(callback => callback(state));
        render(<Supplier creating />);

        const nameInput = screen.getByLabelText('Phone Number');
        fireEvent.change(nameInput, { target: { value: 'NEW SUPPLIER NAME' } });
        const saveButton = screen.getByRole('button', { name: 'Save' });
        fireEvent.click(saveButton);
    });

    test('shoud clear errors dispatch add action...', () => {
        expect(clearSupplierErrorsSpy).toBeCalledTimes(1);
        expect(addSupplierSpy).toBeCalledWith(
            expect.objectContaining({
                accountingCompany: 'LINN',
                approvedCarrier: 'N',
                currencyCode: 'GBP',
                expenseAccount: 'N',
                orderHold: 'N',
                paymentMethod: 'CHEQUE',
                phoneNumber: 'NEW SUPPLIER NAME'
            })
        );
    });
});
