/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import routeData from 'react-router';
import '@testing-library/jest-dom/extend-expect';
import { cleanup, screen, fireEvent } from '@testing-library/react';
import render from '../../test-utils';
import CreateBomVerificationHistory from '../BomVerificationHistoryUtility/CreateBomVerificationHistory';
import BomVerificationHistory from '../BomVerificationHistoryUtility/BomVerificationHistory';
import bomVerificationHistoryActions from '../../actions/bomVerificationHistoryActions';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

jest.mock('react-router', () => ({
    ...jest.requireActual('react-router'),
    useLocation: jest.fn()
}));

const addSpy = jest.spyOn(bomVerificationHistoryActions, 'add');
const fetchSpy = jest.spyOn(bomVerificationHistoryActions, 'fetch');
const idParam = {
    id: 123456
};

jest.spyOn(routeData, 'useParams').mockReturnValue(idParam);

const initialState = {
    bomVerificationHistory: {
        item: null,
        loading: false
    }
};

const bomVerificationHistoryItem = {
    tRef: 123456,
    partNumber: 'SK HUB',
    Remarks: 'B.Slime',
    verifiedBy: 33084,
    dateVerified: '2023-01-26T13:55:20.0000000'
};

const stateWithItem = {
    bomVerificationHistory: {
        item: bomVerificationHistoryItem,
        loading: false
    }
};

const stateWithUser = {
    oidc: { user: { profile: { name: 'User Name', employee: '/employees/33087' } } }
};

describe('When component mounts...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(initialState));
        render(<BomVerificationHistory />);
    });

    test('Should get Bom Ver record', () => {
        expect(fetchSpy).toBeCalledTimes(1);
        expect(fetchSpy).toBeCalledWith(123456);
    });
});

describe('When item received...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithItem));
        render(<BomVerificationHistory />);
    });

    test('Should render details', () => {
        expect(screen.getByDisplayValue('SK HUB')).toBeInTheDocument();
        expect(screen.getByDisplayValue('2023-01-26T13:55:20.0000000')).toBeInTheDocument();
        expect(screen.getByDisplayValue('123456')).toBeInTheDocument();
    });
});

describe('When creating...', () => {
    beforeEach(() => {
        cleanup();
        jest.clearAllMocks();
        useSelector.mockImplementation(callback => callback(stateWithUser));
        render(<CreateBomVerificationHistory />);
    });
    test('Should add with values on create...', async () => {
        let input = screen.getByLabelText('Part Number');
        fireEvent.change(input, { target: { value: 'SK HUB' } });

        input = screen.getByLabelText('Remarks');
        fireEvent.change(input, { target: { value: 'B. Slime' } });

        const button = screen.getByRole('button', { name: 'Create' });
        fireEvent.click(button);

        expect(addSpy).toHaveBeenCalledTimes(1);
    });
});
