/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import '@testing-library/jest-dom/extend-expect';
import { cleanup, screen, fireEvent } from '@testing-library/react';
import render from '../../../test-utils';

import BoardsSummary from '../../boards/BoardsSummary';
import boardComponentSummariesActions from '../../../actions/boardComponentSummariesActions';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

const searchSpy = jest.spyOn(boardComponentSummariesActions, 'searchWithOptions');

const stateAtTheStart = {
    boardComponentSummaries: {
        searchItems: [],
        searchLoading: false
    }
};

const stateWithBoards = {
    boardComponentSummaries: {
        searchItems: [
            {
                boardCode: '123',
                revisionCode: 'L1',
                cref: 'C0',
                partNumber: 'MISS 318'
            },
            {
                boardCode: '456',
                revisionCode: 'L2',
                cref: 'C1',
                partNumber: 'CAP 448'
            }
        ],
        searchLoading: false
    }
};

describe('When component mounts...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateAtTheStart));
        render(<BoardsSummary />);
    });

    test('Should render inputs and button', () => {
        expect(screen.getByLabelText('Board Code Search')).toBeInTheDocument();
        expect(screen.getByLabelText('Revision Code Search')).toBeInTheDocument();
        expect(screen.getByLabelText('CRef Search')).toBeInTheDocument();
        expect(screen.getByLabelText('Part Number Search')).toBeInTheDocument();
        expect(screen.getByRole('button', { name: 'Show Components' })).toBeEnabled();
    });
});

describe('When button is clicked...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateAtTheStart));
        render(<BoardsSummary />);
        let input = screen.getByLabelText('Board Code Search');
        fireEvent.change(input, { target: { value: '123' } });
        input = screen.getByLabelText('Revision Code Search');
        fireEvent.change(input, { target: { value: 'L1' } });
        input = screen.getByLabelText('CRef Search');
        fireEvent.change(input, { target: { value: 'C0' } });

        const button = screen.getByRole('button', { name: 'Show Components' });
        fireEvent.click(button);
    });

    test('Should fetch data', () => {
        expect(searchSpy).toBeCalledTimes(1);
        expect(searchSpy).toBeCalledWith('&boardCode=123&revisionCode=L1&cref=C0&');
    });
});

describe('When boards are received...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithBoards));
        render(<BoardsSummary />);
    });

    test('Should display components', () => {
        expect(screen.getByText('123')).toBeInTheDocument();
        expect(screen.getByText('L1')).toBeInTheDocument();
        expect(screen.getByText('C0')).toBeInTheDocument();
        expect(screen.getByText('MISS 318')).toBeInTheDocument();
        expect(screen.getByText('456')).toBeInTheDocument();
        expect(screen.getByText('L2')).toBeInTheDocument();
        expect(screen.getByText('C1')).toBeInTheDocument();
        expect(screen.getByText('CAP 448')).toBeInTheDocument();
    });
});
