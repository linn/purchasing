/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import '@testing-library/jest-dom/extend-expect';
import { screen, fireEvent } from '@testing-library/react';
import routeData from 'react-router';
import render from '../../../test-utils';
import plCreditDebitNoteActions from '../../../actions/plCreditDebitNoteActions';
import Note from '../../plDebitCreditNotes/Note';
import { emailPdf, savePdf } from '../../../helpers/pdf';

jest.mock('../../../helpers/pdf', () => ({
    savePdf: jest.fn(),
    emailPdf: jest.fn()
}));

const mockParams = {
    id: 123
};
beforeEach(() => {
    jest.spyOn(routeData, 'useParams').mockReturnValue(mockParams);
});

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

const fetchNoteSpy = jest.spyOn(plCreditDebitNoteActions, 'fetch');
const updateNoteSpy = jest.spyOn(plCreditDebitNoteActions, 'update');

const commonData = {
    noteNumber: 123,
    cancelled: false,
    supplierName: 'SUPPLIER',
    dateCreated: new Date().toISOString(),
    orderContactName: 'Supplier McSupplierson',
    returnsOrderNumber: 7654321,
    returnsOrderLine: 1,
    orderDetails: [
        {
            originalOrderNumber: 1234567,
            line: 1,
            partNumber: 'SOME PART',
            partDescription: 'DESC'
        }
    ],
    orderQty: 1,
    orderUnitOfMeasure: 'ONES',
    orderUnitPrice: 100,
    currency: 'GBP',
    netTotal: 100,
    vatTotal: 20,
    vatRate: 20,
    total: 120,
    notes: 'NOTES'
};
const debitNoteState = {
    plCreditDebitNote: {
        item: {
            ...commonData,
            noteType: 'D',
            typePrintDescription: 'DEBIT NOTE'
        }
    }
};
const creditNoteState = {
    plCreditDebitNote: {
        item: {
            ...commonData,
            noteType: 'C',
            typePrintDescription: 'CREDIT NOTE'
        }
    }
};

const cancelledNoteState = {
    plCreditDebitNote: {
        item: {
            ...commonData,
            cancelled: true
        }
    }
};

describe('On initialise...', () => {
    beforeEach(() => {
        render(<Note />);
    });

    test('should fetch note', () => {
        expect(fetchNoteSpy).toBeCalledTimes(1);
        expect(fetchNoteSpy).toBeCalledWith(123);
    });
});

describe('When loading...', () => {
    beforeEach(() => {
        useSelector.mockImplementation(callback =>
            callback({ plCreditDebitNote: { loading: true } })
        );
        render(<Note />);
    });

    test('should render loading spinner', () => {
        expect(screen.getByRole('progressbar')).toBeInTheDocument();
    });
});

describe('When debit note...', () => {
    beforeEach(() => {
        useSelector.mockImplementation(callback => callback(debitNoteState));
        render(<Note />);
    });

    test('should render debit note', () => {
        expect(screen.getByText('Linn DEBIT NOTE 123')).toBeInTheDocument();
        expect(screen.getByText('THESE ITEMS ARE RETURNED FOR CREDIT')).toBeInTheDocument();
        expect(screen.getByText('DO NOT RESSUPPLY')).toBeInTheDocument();
        expect(screen.getByText('THIS IS A PURCHASE LEDGER DEBIT NOTE')).toBeInTheDocument();
    });
});

describe('When credit note...', () => {
    beforeEach(() => {
        useSelector.mockImplementation(callback => callback(creditNoteState));
        render(<Note />);
    });

    test('should render credit note', () => {
        expect(screen.getByText('Linn CREDIT NOTE 123')).toBeInTheDocument();
        expect(screen.queryByText('THESE ITEMS ARE RETURNED FOR CREDIT')).not.toBeInTheDocument();
        expect(screen.queryByText('DO NOT RESSUPPLY')).not.toBeInTheDocument();
    });
});

describe('When cancelling...', () => {
    beforeEach(() => {
        useSelector.mockImplementation(callback => callback(creditNoteState));
        render(<Note />);
        const cancelButton = screen.getByRole('button', { name: 'Cancel' });
        fireEvent.click(cancelButton);

        const reasonInput = screen.getByLabelText('Must give a reason:');
        fireEvent.change(reasonInput, { target: { value: 'SOME REASON' } });

        const saveButton = screen.getByRole('button', { name: 'Save' });
        fireEvent.click(saveButton);
    });

    test('should dispatch action', () => {
        expect(updateNoteSpy).toHaveBeenCalledWith(
            123,
            expect.objectContaining({
                reasonCancelled: 'SOME REASON',
                noteNumber: 123
            })
        );
    });
});

describe('When saving pdf...', () => {
    beforeEach(() => {
        useSelector.mockImplementation(callback => callback(creditNoteState));
        render(<Note />);

        const pdfButton = screen.getByRole('button', { name: 'pdf' });
        fireEvent.click(pdfButton);
    });

    test('should call save pdf function', () => {
        expect(savePdf).toHaveBeenCalled();
    });
});

describe('When sending email...', () => {
    beforeEach(() => {
        useSelector.mockImplementation(callback => callback(creditNoteState));
        render(<Note />);

        const emailButton = screen.getByRole('button', { name: 'email' });
        fireEvent.click(emailButton);
    });

    test('should call email pdf function', () => {
        expect(emailPdf).toHaveBeenCalled();
    });
});

describe('When cancelled note...', () => {
    beforeEach(() => {
        useSelector.mockImplementation(callback => callback(cancelledNoteState));
        render(<Note />);

        const emailButton = screen.getByRole('button', { name: 'email' });
        fireEvent.click(emailButton);
    });

    test('should display cancelled status', () => {
        expect(screen.getByText('CANCELLED')).toBeInTheDocument();
    });
    test('should disable buttons', () => {
        expect(screen.getByRole('button', { name: 'Cancel' })).toBeDisabled();
        expect(screen.getByRole('button', { name: 'email' })).toBeDisabled();
    });
});
