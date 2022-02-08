/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import '@testing-library/jest-dom/extend-expect';
import render from '../../test-utils';

import App from '../App';
import * as actions from '../../actions';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

const testActionSpy = jest.spyOn(actions, 'testAction');

const mockAppState = { oidc: { user: { profile: { name: 'User Name' } } } };

describe('App tests', () => {
    beforeEach(() => {
        useSelector.mockClear();
        testActionSpy.mockClear();
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

    test('App dispatches the test Action...', () => {
        render(<App />);
        expect(testActionSpy).toBeCalledTimes(1);
        expect(testActionSpy).toBeCalledWith('args');
    });
});
