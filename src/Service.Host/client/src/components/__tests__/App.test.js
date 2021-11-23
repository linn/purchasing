/**
 * @jest-environment jsdom
 */
import React from 'react';
import '@testing-library/jest-dom/extend-expect';
import render from '../../test-utils';
import App from '../App';

test('App renders without crashing...', () => {
    const { getByText } = render(<App />);
    expect(getByText('App')).toBeInTheDocument();
});
