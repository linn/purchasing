/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import '@testing-library/jest-dom/extend-expect';
import { cleanup, screen, fireEvent } from '@testing-library/react';
import render from '../../../test-utils';
import CreateCreditNote from '../../plDebitCreditNotes/CreateCreditNote';
import plCreditDebitNoteActions from '../../../actions/plCreditDebitNoteActions';
import returnsOrder from '../fakeData/returnsOrder';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

const addSpy = jest.spyOn(plCreditDebitNoteActions, 'add');

const state = {
    purchaseOrders: { loading: false, searchItems: [returnsOrder] }
};

describe('When creating...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(state));
        render(<CreateCreditNote />);
    });

    test('should dispatch add action with correct data', async () => {
        const roOrderSearchInput = screen.getByLabelText('Returns Order');
        fireEvent.change(roOrderSearchInput, { target: { value: 743638 } });
        fireEvent.keyDown(roOrderSearchInput, { key: 'Enter', code: 'Enter', keyCode: 13 });
        const result = screen.getByText('RETURNS ORDER');
        fireEvent.click(result);

        const notesInput = screen.getByLabelText('Notes');
        fireEvent.change(notesInput, { target: { value: 'A NICE NOTE' } });
        const dropdown = screen.getByLabelText('Credit/Replace');
        fireEvent.change(dropdown, { target: { value: 'CREDIT' } });
        const saveButton = await screen.findByRole('button', { name: 'Save' });
        fireEvent.click(saveButton);

        expect(addSpy).toHaveBeenCalledWith(
            expect.objectContaining({
                notes: 'A NICE NOTE',
                creditOrReplace: 'CREDIT',
                returnsOrderNumber: 743638,
                returnsOrderLine: 1,
                partNumber: 'EK 092P/1',
                orderQty: 10,
                originalOrderNumber: 742872,
                originalOrderLine: 1,
                netTotal: 150,
                vatTotal: 26.25,
                total: 176.25,
                orderUnitOfMeasure: 'ONES',
                orderUnitPrice: 15,
                currency: 'GBP',
                supplierId: 32889
            })
        );
    });
});
