/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import '@testing-library/jest-dom/extend-expect';
import { cleanup, screen, fireEvent } from '@testing-library/react';
import render from '../../../test-utils';
import currenciesActions from '../../../actions/currenciesActions';
import PriceChange from '../../partSupplierUtility/PriceChange';
import partPriceConversionsActions from '../../../actions/partPriceConversionsActions';
import { partPriceConversions } from '../../../itemTypes';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

const fetchCurrenciesSpy = jest.spyOn(currenciesActions, 'fetch');
const fetchPartPriceConversionsSpy = jest.spyOn(partPriceConversionsActions, 'fetchByHref');

const state = {
    currencies: {
        items: [
            { code: 'GBP', name: 'Sterling' },
            { code: 'USD', name: 'Dollar Dollar Bills' }
        ]
    }
};
const stateWithPriceConversions = {
    ...state,
    partPriceConversions: { item: { newPrice: 666, baseNewPrice: 567 } }
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
            oldPrice={666}
            baseOldPrice={777}
            oldCurrencyCode="USD"
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
        expect(screen.getByDisplayValue(666)).toBeDisabled();
        expect(screen.getByDisplayValue(777)).toBeDisabled();
        expect(screen.getByDisplayValue('USD')).toBeDisabled();
    });

    test('Save should be disabled', () => {
        const saveButton = screen.getByRole('button', { name: 'Save' });
        expect(saveButton).toBeDisabled();
    });
});

describe('When changing price...', () => {
    beforeEach(() => {
        const input = screen.getByLabelText('New Price');
        input.focus();
        fireEvent.change(input, { target: { value: 999 } });
        input.blur();
    });

    test('Should request part price conversions', () => {
        expect(fetchPartPriceConversionsSpy).toBeCalledWith(
            `${
                partPriceConversions.uri
            }?partNumber=PART&newPrice=${999}&newCurrency=USD&ledger=SL&round=TRUE`
        );
    });
});

describe('When changing currency and new price has no value...', () => {
    beforeEach(() => {
        const input = screen.getByLabelText('New Currency');
        fireEvent.change(input, { target: { value: 'GBP' } });
    });

    test('Should not request part price conversions', () => {
        expect(fetchPartPriceConversionsSpy).not.toBeCalled();
    });
});

describe('When changing and new price has value...', () => {
    beforeEach(() => {
        let input = screen.getByLabelText('New Price');
        fireEvent.change(input, { target: { value: 999 } });
        input = screen.getByLabelText('New Currency');
        fireEvent.change(input, { target: { value: 'GBP' } });
    });

    test('Should request part price conversions with new currency', () => {
        expect(fetchPartPriceConversionsSpy).toBeCalledWith(
            `${
                partPriceConversions.uri
            }?partNumber=PART&newPrice=${999}&newCurrency=GBP&ledger=SL&round=TRUE`
        );
    });
});

describe('When price conversions arrive change...', () => {
    beforeEach(() => {
        useSelector.mockImplementation(callback => callback(stateWithPriceConversions));
        render(
            <PriceChange
                partNumber="PART"
                supplierId={123}
                supplierName="THE SUPPLIER"
                close={close}
                changePrices={changePrices}
                oldPrice={666}
                baseOldPrice={777}
                oldCurrencyCode="USD"
            />
        );
    });

    test('Should show base price result', () => {
        expect(screen.getByDisplayValue(567)).toBeInTheDocument();
    });
});

describe('When saving new price and currency...', () => {
    beforeEach(() => {
        cleanup();
        useSelector.mockImplementation(callback => callback(stateWithPriceConversions));
        render(
            <PriceChange
                partNumber="PART"
                supplierId={123}
                supplierName="THE SUPPLIER"
                close={close}
                changePrices={changePrices}
                oldPrice={666}
                baseOldPrice={777}
                oldCurrencyCode="USD"
            />
        );
        let input = screen.getByLabelText('New Price');
        fireEvent.change(input, { target: { value: 999 } });
        input = screen.getByLabelText('New Currency');
        fireEvent.change(input, { target: { value: 'GBP' } });
        const saveButton = screen.getByRole('button', { name: 'Save' });
        fireEvent.click(saveButton);
    });

    test('Should request part price conversions with new currency', () => {
        expect(changePrices).toBeCalledWith(
            expect.objectContaining({
                newPrice: 999,
                baseNewPrice: 567,
                newCurrency: 'GBP'
            })
        );
    });
});
