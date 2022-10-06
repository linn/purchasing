/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import '@testing-library/jest-dom/extend-expect';
import { cleanup, fireEvent, screen } from '@testing-library/react';
import { utilities } from '@linn-it/linn-form-components-library';
import { sendPurchaseOrderDeptEmail, suggestedPurchaseOrderValues } from '../../../itemTypes';
import render from '../../../test-utils';
import order from '../fakeData/order';
import PurchaseOrderUtility from '../../PurchaseOrders/PurchaseOrderUtility';
import sendPurchaseOrderDeptEmailActions from '../../../actions/sendPurchaseOrderDeptEmailActions';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

const sendDeptEmailSpy = jest.spyOn(sendPurchaseOrderDeptEmailActions, 'postByHref');

const reduxState = {
    purchaseOrder: { item: order },
    sendPurchaseOrderDeptEmail: {},
    unitsOfMeasure: { items: [] },
    currencies: { items: [] }
};

const stateWithDeptLink = {
    ...reduxState,
    purchaseOrder: {
        item: {
            ...order,
            links: [
                {
                    href: '/purchasing/purchase-orders/100157/email-dept',
                    rel: 'email-dept'
                }
            ]
        }
    }
};

const stateWithDeptEmailError = {
    ...reduxState,
    sendPurchaseOrderDeptEmail: {},
    errors: {
        itemErrors: [{ item: sendPurchaseOrderDeptEmail.item, details: 'COULD NOT SEND EMAIL' }]
    }
};

const stateWithDeptEmailLoading = {
    ...reduxState,
    sendPurchaseOrderDeptEmail: { loading: true }
};

const stateWithDeptEmailSuccess = {
    ...reduxState,
    sendPurchaseOrderDeptEmail: { snackbarVisible: true, item: { message: 'Success! Woohooo!' } }
};

const stateWithSuggestedValues = {
    ...reduxState,
    [suggestedPurchaseOrderValues.item]: {
        item: { supplier: { name: 'A SUPPLIER OF GOOD STUFF' } }
    }
};

describe('When order...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(reduxState));
        render(<PurchaseOrderUtility />);
    });

    test('Should render order', () => {
        expect(screen.getByDisplayValue(100157)).toBeInTheDocument();
    });
});

describe('When no email dept link...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(reduxState));
        render(<PurchaseOrderUtility />);
    });

    test('Should Disable Button', () => {
        expect(screen.getByRole('button', { name: 'Email Dept' })).toBeDisabled();
    });
});

describe('When email dept link...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithDeptLink));
        render(<PurchaseOrderUtility />);
    });

    test('Should Enable Button', () => {
        expect(screen.getByRole('button', { name: 'Email Dept' })).not.toBeDisabled();
    });

    test('Should dispatch action to send email when button clicked', () => {
        const button = screen.getByRole('button', { name: 'Email Dept' });
        fireEvent.click(button);
        expect(sendDeptEmailSpy).toBeCalledTimes(1);
        expect(sendDeptEmailSpy).toBeCalledWith(
            utilities.getHref(stateWithDeptLink.purchaseOrder.item, 'email-dept'),
            {}
        );
    });
});

describe('When sendPurchaseOrderDeptEmail error...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithDeptEmailError));
        render(<PurchaseOrderUtility />);
    });

    test('Should Display Error', () => {
        expect(
            screen.getByText(stateWithDeptEmailError.errors.itemErrors[0].details)
        ).toBeInTheDocument();
    });
});

describe('When sendPurchaseOrderDeptEmail loading...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithDeptEmailLoading));
        render(<PurchaseOrderUtility />);
    });

    test('Should Display Loading Bar', () => {
        expect(screen.getByRole('progressbar')).toBeInTheDocument();
    });
});

describe('When sendPurchaseOrderDeptEmail success...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithDeptEmailSuccess));
        render(<PurchaseOrderUtility />);
    });

    test('Should Display Snackbar', () => {
        expect(
            screen.getByText(
                stateWithDeptEmailSuccess[sendPurchaseOrderDeptEmail.item].item.message
            )
        ).toBeInTheDocument();
    });
});

describe('When creating from suggested Values', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithSuggestedValues));
        render(<PurchaseOrderUtility creating />);
    });

    test('Should render expected values', () => {
        expect(
            screen.getByDisplayValue(
                stateWithSuggestedValues[suggestedPurchaseOrderValues.item].item.supplier.name
            )
        ).toBeInTheDocument();
    });
});
