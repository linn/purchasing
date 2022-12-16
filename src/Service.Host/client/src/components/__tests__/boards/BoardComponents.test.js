/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import routeData from 'react-router';
import '@testing-library/jest-dom/extend-expect';
import { cleanup, screen, fireEvent } from '@testing-library/react';
import render from '../../../test-utils';
import BoardComponents from '../../boards/BoardComponents';
import boardComponentsActions from '../../../actions/boardComponentsActions';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

const fetchSpy = jest.spyOn(boardComponentsActions, 'fetch');
const idParam = {
    id: '649'
};

jest.spyOn(routeData, 'useParams').mockReturnValue(idParam);

const stateAtInitialLoad = {
    boardComponents: {
        item: null,
        loading: false
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
    idBoard: 'N',
    layouts: [
        {
            boardCode: '649',
            layoutCode: 'L1',
            layoutSequence: 1,
            revisions: [{ layoutCode: 'L1', revisionCode: 'L1R1' }]
        }
    ],
    components: [
        {
            boardCode: '649',
            boardLine: 234,
            cRef: 'C001',
            partNumber: 'RES 488',
            assemblyTechnology: 'SM',
            quantity: 1,
            addChangeId: 678,
            changeState: 'LIVE',
            fromLayoutVersion: 1,
            toLayoutVersion: null
        }
    ]
};

const stateWithBoard = {
    boardComponents: {
        item: boardItem,
        loading: false
    }
};

describe('When component mounts...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateAtInitialLoad));
        render(<BoardComponents />);
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
        useSelector.mockImplementation(callback => callback(stateAtInitialLoad));
        render(<BoardComponents />);
        jest.clearAllMocks();
        const input = screen.getByLabelText('Select Board');
        fireEvent.change(input, { target: { value: 12345 } });

        const button = screen.getByRole('button', { name: 'Go' });
        fireEvent.click(button);
    });

    test('Should get board', () => {
        expect(fetchSpy).toBeCalledTimes(1);
        expect(fetchSpy).toBeCalledWith('12345');
    });
});

describe('When board received...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithBoard));
        render(<BoardComponents />);
    });

    test('Should render details', () => {
        expect(screen.getByDisplayValue('649')).toBeInTheDocument();
        expect(screen.getByText('L1')).toBeInTheDocument();
        expect(screen.getByText('L1R1')).toBeInTheDocument();
        expect(screen.getByText('RES 488')).toBeInTheDocument();
        expect(screen.getByText('C001')).toBeInTheDocument();
    });
});
