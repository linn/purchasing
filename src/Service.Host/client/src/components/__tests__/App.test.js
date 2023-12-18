/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import '@testing-library/jest-dom/extend-expect';
import render from '../../test-utils';

import App from '../App';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

const mockAppState = { oidc: { user: { profile: { name: 'User Name' } } } };

describe('App tests', () => {
    beforeEach(() => {
        useSelector.mockClear();
        useSelector.mockImplementation(callback => callback(mockAppState));
    });

    test('App renders without crashing...', () => {
        const { getByText } = render(<App />);
        expect(getByText('Purchasing')).toBeInTheDocument();
    });

    test('App loads data from redux store...', () => {
        const { getByText } = render(<App />);
        expect(getByText('Hello User Name')).toBeInTheDocument();
    });
});
