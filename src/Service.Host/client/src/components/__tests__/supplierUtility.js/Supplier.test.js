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
import { supplier } from '../../../itemTypes';

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

const updateSupplierSpy = jest.spyOn(supplierActions, 'update');
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
    liveOnOracle: 'Y',
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
