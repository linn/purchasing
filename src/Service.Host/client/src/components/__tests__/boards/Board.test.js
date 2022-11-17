/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import '@testing-library/jest-dom/extend-expect';
import { cleanup, screen, fireEvent } from '@testing-library/react';
import routeData from 'react-router';
import render from '../../../test-utils';
import Board from '../../boards/Board';
import boardActions from '../../../actions/boardActions';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

const fetchSpy = jest.spyOn(boardActions, 'fetch');
const updateSpy = jest.spyOn(boardActions, 'update');
const addSpy = jest.spyOn(boardActions, 'add');

const idParam = {
    id: 808
};

jest.spyOn(routeData, 'useParams').mockReturnValue(idParam);

const initialState = {
    board: {
        item: null,
        loading: false
    }
};

const createState = {
    board: {
        item: null,
        loading: false,
        editStatus: 'create'
    }
};

const boardItem = {
    boardCode: '649',
    description: 'PSU BOARD FOR 328A/318A (USES PCB 454)',
    changeId: null,
    changeState: 'LIVE',
    splitBom: 'Y',
    defaultPcbNumber: '454',
    variantOfBoardCode: null,
    loadDirectory: null,
    boardsPerSheet: null,
    coreBoard: 'N',
    clusterBoard: 'N',
    idBoard: 'N'
};

const stateWithBoard = {
    board: {
        item: boardItem,
        loading: false
    }
};

const stateInEdit = {
    board: {
        item: boardItem,
        loading: false,
        editStatus: 'edit'
    }
};

describe('When component mounts...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(initialState));
        render(<Board />);
    });

    test('Should get board', () => {
        expect(fetchSpy).toBeCalledTimes(1);
        expect(fetchSpy).toBeCalledWith(808);
    });
});

describe('When board received...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithBoard));
        render(<Board />);
    });

    test('Should render details', () => {
        expect(screen.getAllByDisplayValue('649')).toHaveLength(2);
        expect(screen.getAllByDisplayValue('PSU BOARD FOR 328A/318A (USES PCB 454)')).toHaveLength(
            2
        );
        expect(screen.getByDisplayValue('454')).toBeInTheDocument();
        expect(screen.getAllByDisplayValue('Yes')).toHaveLength(1);
        expect(screen.getAllByDisplayValue('No')).toHaveLength(3);
    });
});

describe('When editing...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateInEdit));
        render(<Board />);
    });
    test('Shoud update values on save...', async () => {
        const input = screen.getByLabelText('Description');
        fireEvent.change(input, { target: { value: 'New description of 0123' } });

        const saveButton = await screen.findByRole('button', { name: 'Save' });
        fireEvent.click(saveButton);

        expect(updateSpy).toHaveBeenCalledTimes(1);
        expect(updateSpy).toHaveBeenCalledWith(
            '649',
            expect.objectContaining({
                description: 'New description of 0123'
            })
        );
    });
});

describe('When creating...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(createState));
        render(<Board creating />);
    });
    test('Shoud add with values on save...', async () => {
        let input = screen.getByLabelText('Code');
        fireEvent.change(input, { target: { value: '0123' } });

        input = screen.getByLabelText('Description');
        fireEvent.change(input, { target: { value: 'Description of 0123' } });

        input = screen.getByLabelText('PCB Number');
        fireEvent.change(input, { target: { value: '456' } });

        const saveButton = await screen.findByRole('button', { name: 'Save' });
        fireEvent.click(saveButton);

        expect(addSpy).toHaveBeenCalledTimes(1);
        expect(addSpy).toHaveBeenCalledWith(
            expect.objectContaining({
                boardCode: '0123',
                description: 'Description of 0123',
                defaultPcbNumber: '456',
                coreBoard: 'N'
            })
        );
    });
});
