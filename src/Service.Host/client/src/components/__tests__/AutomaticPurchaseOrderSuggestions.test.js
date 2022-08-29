/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import '@testing-library/jest-dom/extend-expect';
import { cleanup, screen, fireEvent } from '@testing-library/react';
import render from '../../test-utils';
import plannersActions from '../../actions/plannersActions';
import automaticPurchaseOrderSuggestionsActions from '../../actions/automaticPurchaseOrderSuggestionsActions';
import automaticPurchaseOrderActions from '../../actions/automaticPurchaseOrderActions';
import AutomaticPurchaseOrderSuggestions from '../AutomaticPurchaseOrderSuggestions';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

const plannersFetchSpy = jest.spyOn(plannersActions, 'fetch');
const suggestionsSpy = jest.spyOn(automaticPurchaseOrderSuggestionsActions, 'searchWithOptions');
const createSpy = jest.spyOn(automaticPurchaseOrderActions, 'add');

const initialState = {
    automaticPurchaseOrderSuggestions: {
        searchItems: []
    }
};
const stateWithPlanners = {
    automaticPurchaseOrderSuggestions: {
        searchItems: []
    },
    planners: {
        items: [
            { id: 123, employeeName: 'EMP A', showAsMrOption: 'Y' },
            { id: 456, employeeName: 'EMP B', showAsMrOption: 'Y' }
        ]
    }
};

const stateWithSuggestions = {
    oidc: { user: { profile: { name: 'User', employee: '/employees/1123' } } },
    automaticPurchaseOrderSuggestions: {
        searchItems: [
            {
                partNumber: 'CAP 372',
                preferredSupplierId: 5177,
                recommendedQuantity: 20000,
                recommendedDate: '2023-01-02T00:00:00.0000000',
                recommendationCode: 'POLICY',
                currencyCode: 'GBP',
                ourPrice: 0.002,
                supplierName: 'ARROW COMPONENTS',
                orderMethod: 'MANUAL',
                jitReorderNumber: 649842,
                vendorManager: 'O',
                planner: 32886,
                jobRef: 'AAJBBH',
                href: null
            },
            {
                partNumber: 'CAP 451',
                preferredSupplierId: 5177,
                recommendedQuantity: 500,
                recommendedDate: '2022-07-18T00:00:00.0000000',
                recommendationCode: 'CHECK1',
                currencyCode: 'GBP',
                ourPrice: 0.135,
                supplierName: 'ARROW COMPONENTS',
                orderMethod: 'MANUAL',
                jitReorderNumber: null,
                vendorManager: 'O',
                planner: 32886,
                jobRef: 'AAJBBH',
                href: null
            }
        ]
    },
    planners: {
        items: [
            { id: 123, employeeName: 'EMP A', showAsMrOption: 'Y' },
            { id: 456, employeeName: 'EMP B', showAsMrOption: 'Y' }
        ]
    }
};

describe('When suggestion screen mounts...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(initialState));
        render(<AutomaticPurchaseOrderSuggestions />);
    });

    test('Should fetch planners', () => {
        expect(plannersFetchSpy).toBeCalled();
    });
});

describe('When planners returned...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithPlanners));
        render(<AutomaticPurchaseOrderSuggestions />);
    });

    test('Run button should be enabled', () => {
        expect(screen.getByRole('button', { name: 'Fetch Suggested Orders' })).toBeEnabled();
    });

    test('Create orders button should be disabled', () => {
        expect(screen.getByRole('button', { name: 'Create Orders' })).toBeDisabled();
    });
});

describe('When fetching suggestions...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithPlanners));
        render(<AutomaticPurchaseOrderSuggestions />);
        const plannersDropDown = screen.getByLabelText('Planner');
        fireEvent.change(plannersDropDown, { target: { value: '123' } });

        const fetchButton = screen.getByRole('button', { name: 'Fetch Suggested Orders' });
        fireEvent.click(fetchButton);
    });

    test('Should fetch suggested orders', () => {
        expect(suggestionsSpy).toBeCalledTimes(1);
        expect(suggestionsSpy).toBeCalledWith(null, '&planner=123');
    });
});

describe('When suggestions are returned...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithSuggestions));
        render(<AutomaticPurchaseOrderSuggestions />);
    });

    test('Create orders button should be enabled', () => {
        expect(screen.getByRole('button', { name: 'Create Orders' })).toBeEnabled();
    });
});

describe('When creating orders...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithSuggestions));
        render(<AutomaticPurchaseOrderSuggestions />);

        const createButton = screen.getByRole('button', { name: 'Create Orders' });
        fireEvent.click(createButton);
    });

    test('Should create orders', () => {
        expect(createSpy).toBeCalledTimes(1);
        expect(createSpy).toBeCalledWith({
            jobRef: 'AAJBBH',
            startedBy: '1123',
            details: [
                {
                    currencyCode: 'GBP',
                    currencyPrice: 40,
                    orderMethod: 'MANUAL',
                    partNumber: 'CAP 372',
                    quantity: 20000,
                    quantityRecommended: 20000,
                    recommendationCode: 'POLICY',
                    requestedDate: '2023-01-02T00:00:00.0000000',
                    supplierId: 5177,
                    supplierName: 'ARROW COMPONENTS'
                },
                {
                    currencyCode: 'GBP',
                    currencyPrice: 67.5,
                    orderMethod: 'MANUAL',
                    partNumber: 'CAP 451',
                    quantity: 500,
                    quantityRecommended: 500,
                    recommendationCode: 'CHECK1',
                    requestedDate: '2022-07-18T00:00:00.0000000',
                    supplierId: 5177,
                    supplierName: 'ARROW COMPONENTS'
                }
            ]
        });
    });
});
