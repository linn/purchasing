/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import { useLocation } from 'react-router';

import '@testing-library/jest-dom/extend-expect';
import { cleanup, screen, fireEvent } from '@testing-library/react';
import render from '../../../test-utils';
import bomTree from '../fakeData/bomTree';
import BomUtility from '../../BomUtility/BomUtility';
import bomTreeActions from '../../../actions/bomTreeActions';
import changeRequestsActions from '../../../actions/changeRequestsActions';
import subAssemblyActions from '../../../actions/subAssemblyActions';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

jest.mock('react-router', () => ({
    ...jest.requireActual('react-router'),
    useLocation: jest.fn()
}));

const fetchSpy = jest.spyOn(bomTreeActions, 'fetchByHref');
const postSpy = jest.spyOn(bomTreeActions, 'postByHref');
const fetchChangeRequestsSpy = jest.spyOn(changeRequestsActions, 'searchWithOptions');
const fetchSubAssemblySpy = jest.spyOn(subAssemblyActions, 'fetchByHref');

const initialState = {
    changeRequests: { searchItems: [] },
    bomTree: { item: null, loading: false }
};

const changeRequests = [
    {
        documentType: 'CRF',
        documentNumber: 48420,
        newPartNumber: 'PCAS LEWIS3',
        dateEntered: '2023-01-13T12:55:20.0000000',
        dateAccepted: null,
        changeState: 'PROPOS'
    }
];

describe('When component mounts...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();

        useSelector.mockImplementation(callback => callback(initialState));
        useLocation.mockImplementation(() => ({
            search: 'bomName=PCAS%20LEWIS3'
        }));
        render(<BomUtility />);
    });

    test('Should fetch bom', () => {
        expect(fetchSpy).toBeCalledTimes(1);
        expect(fetchSpy).toBeCalledWith(
            `/purchasing/boms/tree?bomName=PCAS LEWIS3&levels=0&requirementOnly=false&showChanges=true&treeType=bom`
        );
    });

    test('Should look up change requests', () => {
        expect(fetchChangeRequestsSpy).toBeCalledTimes(1);
        expect(fetchChangeRequestsSpy).toBeCalledWith('PCAS LEWIS3', '&includeAllForBom=True');
    });
});

describe('When loading...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();

        useSelector.mockImplementation(callback =>
            callback({
                ...initialState,
                bomTree: { loading: true },
                changeRequests: { searchLoading: true }
            })
        );
        useLocation.mockImplementation(() => ({
            search: 'bomName=PCAS%20LEWIS3'
        }));
        render(<BomUtility />);
    });

    test('Should render loading indicators', () => {
        expect(screen.getAllByRole('progressbar').length).toBe(3);
    });
});

describe('When data arrives...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();

        useSelector.mockImplementation(callback =>
            callback({
                ...initialState,
                bomTree: { loading: false, item: bomTree },
                changeRequests: { searchLoading: false }
            })
        );
        useLocation.mockImplementation(() => ({
            search: 'bomName=PCAS%20LEWIS3'
        }));
        render(<BomUtility />);
    });

    test('Should render bom tree', () => {
        expect(screen.getByText('PCAS LEWIS3')).toBeInTheDocument();
        expect(screen.getByRole('heading', { name: 'SEKRIT BITS' })).toBeInTheDocument();
    });

    test('Should select root note and render corresponding data grid', () => {
        expect(screen.getByRole('cell', { name: 'CAP 530' })).toBeInTheDocument();
    });

    test('Change actions should be disabled since no change request selected', () => {
        expect(screen.getByRole('button', { name: '+' })).toBeDisabled();
    });
});

describe('When change request selected...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();

        useSelector.mockImplementation(callback =>
            callback({
                ...initialState,
                bomTree: { loading: false, item: bomTree },
                changeRequests: { searchLoading: false, searchItems: changeRequests }
            })
        );
        useLocation.mockImplementation(() => ({
            search: 'bomName=PCAS%20LEWIS3'
        }));
        render(<BomUtility />);
        const crfDropdown = screen.getByLabelText('CRF Number');
        fireEvent.change(crfDropdown, { target: { value: 48420 } });
    });

    test('should render change request basic details in dropdown', () => {
        expect(screen.getByText('CRF48420 / PCAS LEWIS3 / PROPOS')).toBeInTheDocument();
    });

    test('should enable changes', () => {
        expect(screen.getByRole('button', { name: '+' })).not.toBeDisabled();
    });
});

describe('When adding a component to the bom...', () => {
    test('should post bom with new part', async () => {
        cleanup();
        jest.clearAllMocks();

        useSelector.mockImplementation(callback =>
            callback({
                ...initialState,
                bomTree: { loading: false, item: bomTree },
                changeRequests: { searchLoading: false, searchItems: changeRequests },
                parts: {
                    searchItems: [
                        {
                            partNumber: 'CAP 100',
                            description: 'THE NEW CAP',
                            id: 666,
                            bomType: 'C'
                        }
                    ]
                }
            })
        );
        useLocation.mockImplementation(() => ({
            search: 'bomName=PCAS%20LEWIS3'
        }));
        render(<BomUtility />);

        // simulate all the input steps a user goes through to add a component
        const crfDropdown = screen.getByLabelText('CRF Number');
        fireEvent.change(crfDropdown, { target: { value: 48420 } });
        const addButton = screen.getByRole('button', { name: '+' });
        fireEvent.click(addButton);
        const partLookupButton = screen.getAllByTestId('part-lookup-button')[2];
        fireEvent.click(partLookupButton);
        const searchInput = screen.getByLabelText('Part Number');
        fireEvent.change(searchInput, { target: { value: 'CAP 100' } });
        fireEvent.keyDown(searchInput, { key: 'Enter', code: 'Enter', keyCode: 13 });
        const result = screen.getByText('CAP 100');
        fireEvent.click(result);

        // need to wait for modal to close
        await screen.findAllByRole('cell', undefined, { timeout: 5000 });

        const saveButton = screen.getByRole('button', { name: 'Save' });
        fireEvent.click(saveButton);
        expect(postSpy).toHaveBeenCalledWith(
            '/purchasing/boms/tree',
            expect.objectContaining({
                treeRoot: expect.objectContaining({
                    name: 'PCAS LEWIS3',
                    hasChanged: true,
                    children: expect.arrayContaining([
                        expect.objectContaining({
                            name: 'SEKRIT BITS',
                            description: 'BITS KIT FOR LS6000 SEKRIT LOUDSPEAKER ',
                            qty: 1,
                            type: 'A',
                            changeState: 'PROPOS'
                        }),
                        expect.objectContaining({
                            name: 'CAP 530',
                            changeState: 'PROPOS',
                            qty: 1,
                            description: '68PF,,,,,TH,,,,,,',
                            type: 'C'
                        }),
                        expect.objectContaining({
                            name: 'CAP 100',
                            type: 'C',
                            changeState: 'PROPOS',
                            qty: 1,
                            description: 'THE NEW CAP'
                        })
                    ])
                }),
                crNumber: '48420'
            })
        );
    });
});

describe('When adding an assembly to the bom...', () => {
    test('should look up assembly', async () => {
        cleanup();
        jest.clearAllMocks();

        useSelector.mockImplementation(callback =>
            callback({
                ...initialState,
                bomTree: { loading: false, item: bomTree },
                changeRequests: { searchLoading: false, searchItems: changeRequests },
                parts: {
                    searchItems: [
                        {
                            partNumber: 'NEW ASSEMBLY',
                            description: 'THE NEW ASSEMBLY',
                            id: 666,
                            bomType: 'A',
                            bomId: 110
                        }
                    ]
                }
            })
        );
        useLocation.mockImplementation(() => ({
            search: 'bomName=PCAS%20LEWIS3'
        }));
        render(<BomUtility />);

        // simulate all the input steps a user goes through to add an assembly
        const crfDropdown = screen.getByLabelText('CRF Number');
        fireEvent.change(crfDropdown, { target: { value: 48420 } });
        const addButton = screen.getByRole('button', { name: '+' });
        fireEvent.click(addButton);
        const partLookupButton = screen.getAllByTestId('part-lookup-button')[2];
        fireEvent.click(partLookupButton);
        const searchInput = screen.getByLabelText('Part Number');
        fireEvent.change(searchInput, { target: { value: 'NEW ASSEMBLY' } });
        fireEvent.keyDown(searchInput, { key: 'Enter', code: 'Enter', keyCode: 13 });
        const result = screen.getByText('NEW ASSEMBLY');
        fireEvent.click(result);
        await screen.findAllByRole('cell', undefined, { timeout: 5000 });

        expect(fetchSubAssemblySpy).toHaveBeenCalledWith(
            '/purchasing/boms/tree?bomName=NEW ASSEMBLY&levels=0&requirementOnly=false&showChanges=false&treeType=bom'
        );
    });
});

describe('When added assembly data arrives...', () => {
    beforeEach(() => jest.setTimeout(30000));
    test('should add new assembly to bom tree', async () => {
        cleanup();
        jest.clearAllMocks();

        useSelector.mockImplementation(callback =>
            callback({
                ...initialState,
                bomTree: { loading: false, item: bomTree },
                changeRequests: { searchLoading: false, searchItems: changeRequests },
                parts: {
                    searchItems: [
                        {
                            partNumber: 'NEW ASSEMBLY',
                            description: 'THE NEW ASSEMBLY',
                            id: 666,
                            bomType: 'A'
                        }
                    ]
                },
                subAssembly: { item: { name: 'NEW ASSEMBLY' }, loading: false }
            })
        );
        useLocation.mockImplementation(() => ({
            search: 'bomName=PCAS%20LEWIS3'
        }));
        render(<BomUtility />);

        // simulate all the input steps a user goes through to open the partLookUp for a row
        // need to do this so we know where in the tree to add the new assembly
        const crfDropdown = screen.getByLabelText('CRF Number');
        fireEvent.change(crfDropdown, { target: { value: 48420 } });
        const addButton = screen.getByRole('button', { name: '+' });
        fireEvent.click(addButton);
        const partLookupButton = screen.getAllByTestId('part-lookup-button')[2];
        fireEvent.click(partLookupButton);
        const cancel = screen.getByRole('button', { name: 'Cancel' });
        fireEvent.click(cancel);

        // need to wait for modal to close
        await screen.findAllByRole('cell', undefined, { timeout: 5000 });

        expect(true).toBeTruthy();
        expect(screen.getByRole('heading', { name: 'NEW ASSEMBLY' })).toBeInTheDocument();
    });
});
