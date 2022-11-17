/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import '@testing-library/jest-dom/extend-expect';
import { cleanup, screen, fireEvent } from '@testing-library/react';
import render from '../../../test-utils';
import history from '../../../history';
import BoardSearch from '../../boards/BoardSearch';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

const historySpy = jest.spyOn(history, 'push');

const initialState = {
    boards: {
        items: [],
        loading: false
    }
};

describe('When component mounts...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(initialState));
        render(<BoardSearch />);
    });

    test('Should render input and button', () => {
        expect(screen.getByText('Search or select PCAS board')).toBeInTheDocument();
        expect(screen.getByLabelText('Select Board')).toBeInTheDocument();
        expect(screen.getByRole('button', { name: 'Go' })).toBeEnabled();
    });
});

describe('When board is selected...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(initialState));
        render(<BoardSearch />);
        const input = screen.getByLabelText('Select Board');
        fireEvent.change(input, { target: { value: 12345 } });

        const button = screen.getByRole('button', { name: 'Go' });
        fireEvent.click(button);
    });

    test('Should run report', () => {
        expect(historySpy).toBeCalledTimes(1);
        expect(historySpy).toBeCalledWith('/purchasing/boms/boards/12345');
    });
});
