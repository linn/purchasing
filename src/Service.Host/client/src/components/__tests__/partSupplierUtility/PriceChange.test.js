/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import '@testing-library/jest-dom/extend-expect';
import { cleanup, screen, fireEvent } from '@testing-library/react';
import render from '../../../test-utils';
import preferredSupplierChangeActions from '../../../actions/preferredSupplierChangeActions';
import currenciesActions from '../../../actions/currenciesActions';
import partSuppliersActions from '../../../actions/partSuppliersActions';
import PriceChange from '../../partSupplierUtility/PriceChange';
import partPriceConversionsActions from '../../../actions/partPriceConversionsActions';
import { partPriceConversions } from '../../../itemTypes';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

const fetchCurrenciesSpy = jest.spyOn(currenciesActions, 'fetch');
const state = {
    currencies: {
        items: [
            { code: 'GBP', name: 'Sterling' },
            { code: 'USD', name: 'Dollar Dollar Bills' }
        ]
    }
};

const close = jest.fn();
const changePrices = jest.fn();

beforeEach(() => {
    cleanup();
    jest.clearAllMocks();
    useSelector.mockImplementation(callback => callback(state));
    render(
        <PriceChange
            partNumber="PART"
            supplierId={123}
            supplierName="THE SUPPLIER"
            close={close}
            changePrices={changePrices}
        />
    );
});

describe('When component mounts...', () => {
    test('Initialisation actions are dispatched...', () => {
        expect(fetchCurrenciesSpy).toBeCalledTimes(1);
    });

    test('Old values are displayed in disabled fields', () => {
        expect(screen.getByDisplayValue('PART')).toBeDisabled();
        expect(screen.getByDisplayValue(123)).toBeDisabled();
        expect(screen.getByDisplayValue('THE SUPPLIER')).toBeDisabled();
    });
});
