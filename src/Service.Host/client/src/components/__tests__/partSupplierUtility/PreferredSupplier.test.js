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
import priceChangeReasonsActions from '../../../actions/priceChangeReasonsActions';
import PreferredSupplier from '../../partSupplierUtility/PreferredSupplier';
import partPriceConversionsActions from '../../../actions/partPriceConversionsActions';
import { partPriceConversions } from '../../../itemTypes';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

const partNumber = 'PART';
const supplierId = 123;
const supplierName = 'THE SUPPLIER';
const price = 456;
const basePrice = 400;
const currency = 'USD';

const state = {
    oidc: { user: { profile: { name: 'User Name', employee: '/employees/33087' } } },
    partSuppliers: {
        searchItems: [
            { supplierId: 1, supplierName: 'THE SUPPLIER' },
            {
                supplierId: 2,
                supplierName: 'NEW SUPPLIER',
                currencyUnitPrice: 123,
                baseOurUnitPrice: 456,
                currencyCode: 'USD',
                partNumber: 'PART'
            }
        ]
    },
    currencies: {
        items: [
            { code: 'GBP', name: 'Sterling' },
            { code: 'USD', name: 'Dollar Dollar Bills' }
        ]
    },
    priceChangeReasons: {
        items: [
            { reasonCode: 'NEW', description: 'New Supplier' },
            { reasonCode: 'SE', description: 'Supplier Enforced' }
        ]
    }
};

const fetchCurrenciesSpy = jest.spyOn(currenciesActions, 'fetch');
const fetchReasonsSpy = jest.spyOn(priceChangeReasonsActions, 'fetch');
const fetchPartSuppliersSpy = jest.spyOn(partSuppliersActions, 'searchWithOptions');
const addPreferredSupplierSpy = jest.spyOn(preferredSupplierChangeActions, 'add');
const clearErrorsSpy = jest.spyOn(preferredSupplierChangeActions, 'clearErrorsForItem');
const fetchPartPriceConversionsSpy = jest.spyOn(partPriceConversionsActions, 'fetchByHref');

describe('When component mounts...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(state));
        render(<PreferredSupplier partNumber={partNumber} />);
    });

    test('Initialisation actions are dispatched...', () => {
        expect(fetchCurrenciesSpy).toBeCalledTimes(1);
        expect(fetchReasonsSpy).toBeCalledTimes(1);
        expect(fetchPartSuppliersSpy).toBeCalledTimes(1);
        expect(clearErrorsSpy).toBeCalledTimes(1);
        expect(fetchPartSuppliersSpy).toBeCalledWith(
            null,
            `&partNumber=${partNumber}&supplierName=`
        );
    });
});

describe('When part Loading...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(state));
        render(<PreferredSupplier partNumber={partNumber} partLoading />);
    });

    test('Should render loading spinner', () => {
        expect(screen.getByRole('progressbar')).toBeInTheDocument();
    });
});

describe('When safetyCriticalPart...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(state));
        render(<PreferredSupplier partNumber={partNumber} safetyCriticalPart />);
    });

    test('Should render message', () => {
        expect(
            screen.getByText(
                'WARNING: This is a safety critical part. Please ensure new part number has been verified for this function.'
            )
        ).toBeInTheDocument();
    });
});

describe('When result Loading...', () => {
    beforeEach(() => {
        const loadingState = {
            preferredSupplierChange: { loading: true }
        };
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(loadingState));
        render(<PreferredSupplier partNumber={partNumber} partLoading={false} />);
    });

    test('Should render loading spinner', () => {
        expect(screen.getByRole('progressbar')).toBeInTheDocument();
    });
});

describe('When changePreferredSupplier error...', () => {
    beforeEach(() => {
        const errorState = {
            preferredSupplierChange: { loading: false },
            errors: {
                itemErrors: [{ item: 'preferredSupplierChange', details: 'An Error Occurred' }]
            }
        };
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(errorState));
        render(<PreferredSupplier partNumber={partNumber} partLoading={false} />);
    });

    test('Should render ErrorCard', () => {
        expect(screen.getByText('An Error Occurred')).toBeInTheDocument();
    });
});

describe('When submitting a new preferred Supplier...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(state));
        render(
            <PreferredSupplier
                partNumber={partNumber}
                oldSupplierId={supplierId}
                oldSupplierName={supplierName}
                oldPrice={price}
                baseOldPrice={basePrice}
                oldCurrencyCode={currency}
            />
        );
    });

    test('Current values should be displayed in disabled fields', () => {
        expect(screen.getByDisplayValue('PART')).toBeDisabled();
        expect(screen.getByDisplayValue(123)).toBeDisabled();
        expect(screen.getByDisplayValue('THE SUPPLIER')).toBeDisabled();
        expect(screen.getByDisplayValue(456)).toBeDisabled();
        expect(screen.getByDisplayValue(400)).toBeDisabled();
        expect(screen.getByDisplayValue('USD')).toBeDisabled();
    });

    test('Should allow new supplier to be saved', () => {
        const saveButton = screen.getByRole('button', { name: 'Save' });
        const supplierDropdown = screen.getByLabelText('Select a New Supplier');
        fireEvent.change(supplierDropdown, { target: { value: 2 } });
        expect(saveButton).not.toBeDisabled();

        const remarks = screen.getByLabelText('Remarks');
        fireEvent.change(remarks, { target: { value: 'REMARKABLE' } });

        const reasonDropdown = screen.getByLabelText('Reason');
        fireEvent.change(reasonDropdown, { target: { value: 'NEW' } });

        fireEvent.click(saveButton);

        expect(addPreferredSupplierSpy).toHaveBeenCalledWith(
            expect.objectContaining({
                remarks: 'REMARKABLE',
                newSupplierId: 2,
                changeReasonCode: 'NEW',
                newCurrency: 'USD'
            })
        );
    });
});

describe('When new preferred supplier selected...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(state));
        render(
            <PreferredSupplier
                partNumber={partNumber}
                oldSupplierId={supplierId}
                oldSupplierName={supplierName}
                oldPrice={price}
                baseOldPrice={basePrice}
                oldCurrencyCode={currency}
            />
        );
        const supplierDropdown = screen.getByLabelText('Select a New Supplier');
        fireEvent.change(supplierDropdown, { target: { value: 2 } });
    });

    test('Should request price conversions', () => {
        expect(fetchPartPriceConversionsSpy).toHaveBeenCalledWith(
            `${partPriceConversions.uri}?partNumber=PART&newPrice=123&newCurrency=USD`
        );
    });
});

describe('When price conversions arrive...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        render(
            <PreferredSupplier
                partNumber={partNumber}
                oldSupplierId={supplierId}
                oldSupplierName={supplierName}
                oldPrice={price}
                baseOldPrice={basePrice}
                oldCurrencyCode={currency}
                clearErrors={jest.fn()}
            />
        );
        const stateWithPriceConversions = {
            partPriceConversions: { item: { newPrice: 666, baseNewPrice: 777 } }
        };

        useSelector.mockImplementation(callback => callback(stateWithPriceConversions));
        const supplierDropdown = screen.getByLabelText('Select a New Supplier');
        fireEvent.change(supplierDropdown, { target: { value: 2 } });
    });

    test('Should show new results', () => {
        expect(screen.getByDisplayValue(666)).toBeInTheDocument();
        expect(screen.getByDisplayValue(777)).toBeInTheDocument();
    });
});
