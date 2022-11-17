/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import '@testing-library/jest-dom/extend-expect';
import { cleanup, screen } from '@testing-library/react';
import render from '../../test-utils';
import BomTypeChange from '../BomTypeChange';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

const stateInitially = {
    bomTypeChange: {},
    partSuppliers: []
};

const stateWithBomTypeLoading = {
    bomTypeChange: {
        item: {},
        loading: true
    },
    partSuppliers: []
};

const stateWithBomTypeLoaded = {
    bomTypeChange: {
        item: {
            partNumber: 'CAP 001',
            partDescription: 'Some Capacitor',
            oldBomType: 'C',
            preferredSuppliedId: 122,
            preferredSupplierName: 'Tescos',
            partCurrency: 'LTD',
            links: [{ href: '/purchasing/bom-type-change', rel: 'change-bom-type' }]
        },
        loading: false
    },
    partSuppliers: []
};

describe('When component mounts...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateInitially));
        render(<BomTypeChange />);
    });

    test('Save button should be disabled', () => {
        expect(screen.getByRole('button', { name: 'Save' })).toBeDisabled();
    });
});

describe('When part information loading', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithBomTypeLoading));
        render(<BomTypeChange />);
    });

    test('should render loading spinner', () => {
        expect(screen.getByRole('progressbar')).toBeInTheDocument();
    });
});

describe('When part information loaded', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithBomTypeLoaded));
        render(<BomTypeChange />);
    });

    test('Save button should be disabled', () => {
        expect(screen.getByRole('button', { name: 'Save' })).toBeDisabled();
    });
});
