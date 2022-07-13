/**
 * @jest-environment jsdom
 */
import React from 'react';
import { useSelector } from 'react-redux';
import '@testing-library/jest-dom/extend-expect';
import { screen, fireEvent } from '@testing-library/react';
import routeData from 'react-router';
import render from '../../../test-utils';
import suppliersActions from '../../../actions/suppliersActions';
// import ediOrdersActions from '../../../actions/ediOrdersActions';
// import sendEdiEmailActions from '../../../actions/sendEdiEmailActions';
import EdiOrder from '../../EdiOrders';

jest.mock('react-redux', () => ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn()
}));

const fetchAppStateActionsSpy = jest.spyOn(suppliersActions, 'fetchState');

describe('On initial page load', () => {
    beforeEach(() => {
        jest.clearAllMocks();
        const initialState = {
            ediOrders: {
                loading: false,
                items: []
            },
            suppliers: {
                loading: false,
                items: []
            }
        };
        useSelector.mockImplementation(callback => callback(initialState));
        render(<EdiOrder />);
    });

});
