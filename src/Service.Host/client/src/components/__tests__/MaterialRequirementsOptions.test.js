/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import '@testing-library/jest-dom/extend-expect';
import { cleanup, screen, fireEvent } from '@testing-library/react';
import render from '../../test-utils';
import MaterialRequirementsOptions from '../materialRequirements/MaterialRequirementsOptions';
import mrMasterActions from '../../actions/mrMasterActions';
import mrReportOptionsActions from '../../actions/mrReportOptionsActions';
import mrReportActions from '../../actions/mrReportActions';
import history from '../../history';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

const fetchMrMasterSpy = jest.spyOn(mrMasterActions, 'fetchByHref');
const fetchOptionsSpy = jest.spyOn(mrReportOptionsActions, 'fetchByHref');
const clearReportSpy = jest.spyOn(mrReportActions, 'clearItem');
const historySpy = jest.spyOn(history, 'push');

const initialState = {};
const stateWithOptions = {
    mrMaster: {
        item: {
            jobRef: 'AAJADE',
            runDate: '2022-05-04T09:48:49.0000000',
            runLogIdCurrentlyInProgress: null
        },
        loading: false
    },
    mrReportOptions: {
        item: {
            partSelectorOptions: [
                {
                    option: 'Select Parts',
                    displayText: 'Select Parts',
                    displaySequence: 0,
                    dataTag: 'parts'
                },
                {
                    option: 'Planner123',
                    displayText: 'Harrys Parts',
                    displaySequence: 1,
                    dataTag: 'planner'
                }
            ],
            stockLevelOptions: [
                {
                    option: '0-4',
                    displayText: 'Danger Levels 0 - 4',
                    displaySequence: 0
                },
                {
                    option: '0-2',
                    displayText: 'Danger Levels 0 - 2',
                    displaySequence: 1
                }
            ]
        }
    },
    mrReport: {}
};

describe('When component mounts...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(initialState));
        render(<MaterialRequirementsOptions />);
    });

    test('Should fetch master details', () => {
        expect(fetchMrMasterSpy).toBeCalledTimes(1);
    });

    test('Should fetch master details', () => {
        expect(fetchOptionsSpy).toBeCalledTimes(1);
    });

    test('Should show run button', () => {
        expect(screen.getByText('MR Options')).toBeInTheDocument();
    });

    test('Run button should be disabled', () => {
        expect(screen.getByRole('button', { name: 'Run Report' })).toBeDisabled();
    });
});

describe('When report options are returned...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithOptions));
        render(<MaterialRequirementsOptions />);
    });

    test('Run button should be disabled', () => {
        expect(screen.getByRole('button', { name: 'Run Report' })).toBeDisabled();
    });

    test('Should show part selector dropdown', () => {
        expect(screen.getByText('Select Parts')).toBeInTheDocument();
    });

    test('Should show stock dropdown', () => {
        expect(screen.getByText('Danger Levels 0 - 4')).toBeInTheDocument();
    });
});

describe('When report options are returned...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithOptions));
        render(<MaterialRequirementsOptions />);
        const partsDropdown = screen.getByLabelText('Parts Options');
        fireEvent.change(partsDropdown, { target: { value: 'Planner123' } });
    });

    test('Run button should be enabled', () => {
        expect(screen.getByRole('button', { name: 'Run Report' })).toBeEnabled();
    });
});

describe('When report is run...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithOptions));
        render(<MaterialRequirementsOptions />);
        const partsDropdown = screen.getByLabelText('Parts Options');
        fireEvent.change(partsDropdown, { target: { value: 'Planner123' } });

        const runButton = screen.getByRole('button', { name: 'Run Report' });
        fireEvent.click(runButton);
    });

    test('Should clear existing report data', () => {
        expect(clearReportSpy).toBeCalledTimes(1);
    });

    test('Should run report', () => {
        expect(historySpy).toBeCalledTimes(1);
        expect(historySpy).toBeCalledWith('/purchasing/material-requirements/report', {
            jobRef: 'AAJADE',
            typeOfReport: 'MR',
            partNumbers: [],
            partSelector: 'Planner123',
            stockLevelSelector: '0-4',
            orderBySelector: 'supplier/part'
        });
    });
});
