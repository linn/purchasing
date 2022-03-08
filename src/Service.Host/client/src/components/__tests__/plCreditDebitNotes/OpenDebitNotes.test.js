/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import '@testing-library/jest-dom/extend-expect';
import { screen, fireEvent } from '@testing-library/react';
import render from '../../../test-utils';
import openDebitNotesActions from '../../../actions/openDebitNotesActions';
import plCreditDebitNoteActions from '../../../actions/plCreditDebitNoteActions';
import OpenDebitNotes from '../../plDebitCreditNotes/OpenDebitNotes';

const state = {
    openDebitNotes: {
        loading: false,
        items: [
            {
                noteNumber: 1,
                partNumber: 'PART',
                dateCreated: new Date().toISOString(),
                supplierName: 'SUPPLIER',
                orderQty: 123,
                originalOrderNumber: 1234567,
                returnsOrderNumber: 7654321,
                netTotal: 111.111,
                notes: 'A COMMENT WE WILL CHANGE'
            },
            {
                noteNumber: 2,
                partNumber: 'PART 2',
                dateCreated: new Date().toISOString(),
                supplierName: 'SUPPLIER 2',
                orderQty: 123,
                originalOrderNumber: 1234567,
                returnsOrderNumber: 7654321,
                netTotal: 222.222,
                notes: 'A COMMENT'
            },
            {
                noteNumber: 3,
                partNumber: 'PART 3',
                dateCreated: new Date().toISOString(),
                supplierName: 'SUPPLIER 3',
                orderQty: 123,
                originalOrderNumber: 1234567,
                returnsOrderNumber: 7654321,
                netTotal: 333.333,
                notes: 'A COMMENT'
            }
        ]
    }
};

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

const fetchOpenDebitNotesSpy = jest.spyOn(openDebitNotesActions, 'fetch');
const updateDebitNoteSpy = jest.spyOn(plCreditDebitNoteActions, 'update');

describe('On initialise...', () => {
    beforeEach(() => {
        render(<OpenDebitNotes />);
    });

    test('should fetch openb debit notes', () => {
        expect(fetchOpenDebitNotesSpy).toBeCalledTimes(1);
    });
});

describe('When loading...', () => {
    beforeEach(() => {
        const loadingState = { openDebitNotes: { loading: true, items: [] } };
        useSelector.mockImplementation(callback => callback(loadingState));
        render(<OpenDebitNotes />);
    });

    test('should render loading spinner', () => {
        expect(screen.getByRole('progressbar')).toBeInTheDocument();
    });
});

describe('When data arrives...', () => {
    beforeEach(() => {
        useSelector.mockImplementation(callback => callback(state));
        render(<OpenDebitNotes />);
    });

    test('should render in DataGrid', () => {
        expect(screen.getAllByRole('row')).toHaveLength(4); // headers + 3 rows of data
        expect(screen.getByText('SUPPLIER')).toBeInTheDocument();
        expect(screen.getByText('SUPPLIER 2')).toBeInTheDocument();
    });

    test('should calculate total to 2DP', () => {
        expect(screen.getByText('Total Outstanding: £666.67')).toBeInTheDocument();
    });
});

describe('When closing notes...', () => {
    beforeEach(() => {
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(state));
        render(<OpenDebitNotes />);
        const firstCheckbox = screen.getAllByRole('checkbox')[1];
        fireEvent.click(firstCheckbox);

        const secondCheckbox = screen.getAllByRole('checkbox')[2];
        fireEvent.click(secondCheckbox);

        const closeButton = screen.getByRole('button', { name: 'Close Selected' });
        fireEvent.click(closeButton);
    });

    test('should open reason dialog', () => {
        expect(screen.getByLabelText('Reason? (optional)')).toBeInTheDocument();
    });

    test('should close selected notes with reason', () => {
        const reasonInput = screen.getByLabelText('Reason? (optional)');

        fireEvent.change(reasonInput, { target: { value: 'SOME REASON' } });

        const confirmButton = screen.getByRole('button', { name: 'Confirm' });
        fireEvent.click(confirmButton);

        expect(updateDebitNoteSpy).toHaveBeenCalledTimes(2);
        expect(updateDebitNoteSpy).toBeCalledWith(
            1,
            expect.objectContaining({
                reasonClosed: 'SOME REASON',
                close: true
            })
        );
        expect(updateDebitNoteSpy).toBeCalledWith(
            2,
            expect.objectContaining({
                reasonClosed: 'SOME REASON',
                close: true
            })
        );
    });
});

describe('When updating comment...', () => {
    beforeEach(() => {
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(state));
        render(<OpenDebitNotes />);

        // select the row
        const firstCheckbox = screen.getAllByRole('checkbox')[1];
        fireEvent.click(firstCheckbox);

        // double click the cell to put it in edit mode
        let comment = screen.getByText('A COMMENT WE WILL CHANGE');
        fireEvent.doubleClick(comment);

        // change the value
        comment = screen.getByDisplayValue('A COMMENT WE WILL CHANGE');
        fireEvent.change(comment, { target: { value: 'NEW COMMENT' } });

        //save
        const saveButton = screen.getByRole('button', { name: 'Save Comments' });
        fireEvent.click(saveButton);
    });

    test('should call update', () => {
        expect(updateDebitNoteSpy).toHaveBeenCalledTimes(1);
        expect(updateDebitNoteSpy).toBeCalledWith(
            1,
            expect.objectContaining({
                notes: 'NEW COMMENT'
            })
        );
    });
});
