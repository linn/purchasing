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
import detailWithReceived from '../fakeData/detailWithReceived';
import returnsOrder from '../fakeData/returnsOrder';
import PurchaseOrderUtility from '../../PurchaseOrders/PurchaseOrderUtility';
import sendPurchaseOrderDeptEmailActions from '../../../actions/sendPurchaseOrderDeptEmailActions';
import purchaseOrderActions from '../../../actions/purchaseOrderActions';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

const sendDeptEmailSpy = jest.spyOn(sendPurchaseOrderDeptEmailActions, 'postByHref');
const patchPurchaseOrderSpy = jest.spyOn(purchaseOrderActions, 'patch');

const reduxState = {
    purchaseOrder: { item: order },
    sendPurchaseOrderDeptEmail: {},
    unitsOfMeasure: { items: [] },
    currencies: { items: [] },
    nominals: {},
    departments: {},
    nominalAccounts: {}
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

const stateWithCancelledOrder = {
    ...reduxState,
    purchaseOrder: {
        item: {
            ...order,
            cancelled: 'Y'
        }
    }
};

const stateWithDelivery = {
    ...reduxState,
    purchaseOrder: {
        item: {
            ...order,
            details: [detailWithReceived]
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

const stateWithCreateForOtherUserLink = {
    ...stateWithSuggestedValues,
    purchaseOrder: {
        applicationState: {
            links: [
                {
                    href: '/purchasing/purchase-orders/create',
                    rel: 'create-for-other-user'
                }
            ]
        }
    }
};

const stateWithReturnsOrder = {
    ...reduxState,
    purchaseOrder: {
        item: {
            ...returnsOrder
        }
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
        expect(screen.getByDisplayValue('Rev 123')).toBeInTheDocument();
    });

    test('Should enable price change as no deliveries yet', () => {
        expect(screen.getByLabelText('Our Price (unit, currency) *')).toBeInTheDocument();
        expect(screen.getByLabelText('Our Price (unit, currency) *')).toBeEnabled();
    });
});

describe('When order line has items received...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithDelivery));
        render(<PurchaseOrderUtility />);
    });

    test('Should disable price change', () => {
        expect(screen.getByLabelText('Our Price (unit, currency) *')).toBeInTheDocument();
        expect(screen.getByLabelText('Our Price (unit, currency) *')).toBeDisabled();
    });
});

describe('When updating our unit price to 0...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(reduxState));
        render(<PurchaseOrderUtility />);
    });

    test('Should set field to zero', () => {
        const input = screen.getByRole('spinbutton', { name: 'Our Price (unit, currency)' });
        fireEvent.change(input, { target: { value: 0 } });
        expect(input.value).toBe('0');
    });
});

describe('When cancelling order...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(reduxState));
        render(<PurchaseOrderUtility />);
    });

    test('Should render cancel order button', () => {
        expect(screen.getByRole('button', { name: 'Cancel' })).toBeInTheDocument();
    });

    test('Should Open Cancel Dialog when button clicked', () => {
        const cancelButton = screen.getByRole('button', { name: 'Cancel' });
        fireEvent.click(cancelButton);
        expect(screen.getByText('Cancel this order?')).toBeInTheDocument();
    });

    test('Should dispatch patch action with cancel body when reason entered and confirm clicked', () => {
        const cancelButton = screen.getByRole('button', { name: 'Cancel' });
        fireEvent.click(cancelButton);

        const reasonInput = screen.getByLabelText('Enter a reason');
        fireEvent.change(reasonInput, { target: { value: 'SOME REASON' } });

        const confirmButton = screen.getByRole('button', { name: 'confirm' });
        fireEvent.click(confirmButton);

        expect(patchPurchaseOrderSpy).toHaveBeenCalledWith(
            100157,
            expect.objectContaining({
                to: expect.objectContaining({
                    cancelled: 'Y',
                    reasonCancelled: 'SOME REASON'
                })
            })
        );
    });
});

describe('When un-cancelling order...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithCancelledOrder));
        render(<PurchaseOrderUtility />);
    });

    test('Should render uncancel order button', () => {
        expect(screen.getByRole('button', { name: 'UnCancel' })).toBeInTheDocument();
    });

    test('Should Open Cancel Dialog when button clicked', () => {
        const uncancelButton = screen.getByRole('button', { name: 'UnCancel' });
        fireEvent.click(uncancelButton);
        expect(screen.getByText('Un-cancel this order?')).toBeInTheDocument();
    });

    test('Should dispatch patch action with uncancel body when confirm clicked', () => {
        const uncancelButton = screen.getByRole('button', { name: 'UnCancel' });
        fireEvent.click(uncancelButton);

        const confirmButton = screen.getByRole('button', { name: 'confirm' });
        fireEvent.click(confirmButton);

        expect(patchPurchaseOrderSpy).toHaveBeenCalledWith(
            100157,
            expect.objectContaining({
                to: expect.objectContaining({
                    cancelled: 'N'
                })
            })
        );
    });
});

describe('When fil cancelling order...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(reduxState));
        render(<PurchaseOrderUtility />);
    });

    test('Should render fil cancel  button', () => {
        expect(screen.getByRole('button', { name: 'Fil Cancel' })).toBeInTheDocument();
    });

    test('Should Open File Cancel Dialog when button clicked', () => {
        const cancelButton = screen.getByRole('button', { name: 'Fil Cancel' });
        fireEvent.click(cancelButton);
        expect(screen.getByText('Fil Cancel order line 1?')).toBeInTheDocument();
    });

    test('Should dispatch patch action with cancel body when reason entered and confirm clicked', () => {
        const cancelButton = screen.getByRole('button', { name: 'Fil Cancel' });
        fireEvent.click(cancelButton);

        const reasonInput = screen.getByLabelText('Enter a reason');
        fireEvent.change(reasonInput, { target: { value: 'SOME REASON' } });

        const confirmButton = screen.getByRole('button', { name: 'confirm' });
        fireEvent.click(confirmButton);

        expect(patchPurchaseOrderSpy).toHaveBeenCalledWith(
            100157,
            expect.objectContaining({
                to: expect.objectContaining({
                    details: [{ filCancelled: 'Y', line: 1, reasonFilCancelled: 'SOME REASON' }]
                })
            })
        );
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

describe('When creating and create-for-other-user link...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithCreateForOtherUserLink));
        render(<PurchaseOrderUtility creating />);
    });

    test('Should Render Dropdown for requestedBy field', () => {
        expect(
            screen.getByLabelText('Requested By (Leave blank to set as you)')
        ).toBeInTheDocument();
    });
});

describe('When creating and no create-for-other-user link...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithSuggestedValues));
        render(<PurchaseOrderUtility creating />);
    });

    test('Should render readonly input for requestedBy field ', () => {
        expect(screen.getByLabelText('Requested By')).toBeInTheDocument();
        expect(screen.getByLabelText('Requested By')).toBeDisabled();
    });
});

describe('When order is a Returns Order...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithReturnsOrder));
        render(<PurchaseOrderUtility />);
    });

    test('Should disable price change', () => {
        expect(screen.getByLabelText('Our Price (unit, currency) *')).toBeInTheDocument();
        expect(screen.getByLabelText('Our Price (unit, currency) *')).toBeDisabled();
    });
});
