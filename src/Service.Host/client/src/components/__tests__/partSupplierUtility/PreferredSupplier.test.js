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
            { supplierId: 2, supplierName: 'NEW SUPPLIER' }
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
        const loadingState = {
            preferredSupplierChange: { loading: false },
            errors: {
                itemErrors: [{ item: 'preferredSupplierChange', details: 'An Error Occurred' }]
            }
        };
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(loadingState));
        render(<PreferredSupplier partNumber={partNumber} partLoading={false} />);
    });

    test('Should render ErrorCard', () => {
        expect(screen.getByText('An Error Occurred')).toBeInTheDocument();
    });
});

describe('When Part already has a preferred supplier...', () => {
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

    test('Should not render price input fields', () => {
        expect(screen.queryByLabelText('New Price')).not.toBeInTheDocument();
        expect(screen.queryByLabelText('Base New Price')).not.toBeInTheDocument();
        expect(screen.queryByLabelText('New Currency')).not.toBeInTheDocument();
    });

    test('Should allow new supplier to be saved', () => {
        const saveButton = screen.getByRole('button', { name: 'Save' });
        const supplierDropdown = screen.getByLabelText('Select a New Supplier');
        fireEvent.change(supplierDropdown, { target: { value: 2 } });
        expect(saveButton).not.toHaveClass('Mui-disabled');

        const remarks = screen.getByLabelText('Remarks');
        fireEvent.change(remarks, { target: { value: 'REMARKABLE' } });

        const reasonDropdown = screen.getByLabelText('Reason');
        fireEvent.change(reasonDropdown, { target: { value: 'NEW' } });

        fireEvent.click(saveButton);

        expect(addPreferredSupplierSpy).toHaveBeenCalledWith(
            expect.objectContaining({
                remarks: 'REMARKABLE',
                newSupplierId: 2,
                changeReasonCode: 'NEW'
            })
        );
    });
});

describe('When Part bomType A and not new Supplier not 4415 (Linn)...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(state));
        render(<PreferredSupplier partNumber={partNumber} bomType="A" />);
        const supplierDropdown = screen.getByLabelText('Select a New Supplier');
        fireEvent.change(supplierDropdown, { target: { value: 2 } });
    });

    test('Should show message', () => {
        expect(
            screen.getByText('Tell production to put a labour price on this.')
        ).toBeInTheDocument();
    });
});

describe('When Part has no preferred supplier...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(state));
        render(<PreferredSupplier partNumber={partNumber} />);
    });

    test('Should render price input fields', () => {
        expect(screen.queryByLabelText('New Price')).toBeInTheDocument();
        expect(screen.queryByLabelText('Base New Price')).toBeInTheDocument();
        expect(screen.queryByLabelText('New Currency')).toBeInTheDocument();
    });

    test('Should allow new supplier to be saved with prices', () => {
        const saveButton = screen.getByRole('button', { name: 'Save' });
        const supplierDropdown = screen.getByLabelText('Select a New Supplier');
        fireEvent.change(supplierDropdown, { target: { value: 2 } });
        expect(saveButton).not.toHaveClass('Mui-disabled');

        const remarks = screen.getByLabelText('Remarks');
        fireEvent.change(remarks, { target: { value: 'REMARKABLE' } });

        const reasonDropdown = screen.getByLabelText('Reason');
        fireEvent.change(reasonDropdown, { target: { value: 'NEW' } });

        const newPrice = screen.getByLabelText('New Price');
        fireEvent.change(newPrice, { target: { value: 321 } });

        const baseNewPrice = screen.getByLabelText('Base New Price');
        fireEvent.change(baseNewPrice, { target: { value: 567 } });

        const currencyDropdown = screen.getByLabelText('New Currency');
        fireEvent.change(currencyDropdown, { target: { value: 'USD' } });

        fireEvent.click(saveButton);

        expect(addPreferredSupplierSpy).toHaveBeenCalledWith(
            expect.objectContaining({
                remarks: 'REMARKABLE',
                newSupplierId: 2,
                changeReasonCode: 'NEW',
                newPrice: 321,
                baseNewPrice: 567,
                newCurrency: 'USD'
            })
        );
    });
});
