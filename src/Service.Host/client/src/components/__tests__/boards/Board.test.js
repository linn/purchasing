/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import '@testing-library/jest-dom/extend-expect';
import { cleanup, screen } from '@testing-library/react';
import routeData from 'react-router';
import render from '../../../test-utils';
import Board from '../../boards/Board';
import boardActions from '../../../actions/boardActions';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

const fetchSpy = jest.spyOn(boardActions, 'fetch');

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

const stateWithBoard = {
    board: {
        item: {
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
        },
        loading: false
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
