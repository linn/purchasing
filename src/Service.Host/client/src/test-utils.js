import { render } from '@testing-library/react';
import { Provider } from 'react-redux';
import React from 'react';
import { ThemeProvider, createTheme } from '@mui/material/styles';
import configureMockStore from 'redux-mock-store';
import { MemoryRouter } from 'react-router-dom';
import { SnackbarProvider } from 'notistack';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterMoment } from '@mui/x-date-pickers/AdapterMoment';
import { apiMiddleware as api } from 'redux-api-middleware';
import thunkMiddleware from 'redux-thunk';

const middleware = [api, thunkMiddleware];

// eslint-disable-next-line react/prop-types
const Providers = ({ children }) => {
    global.fetch = jest.fn(() =>
        Promise.resolve({
            json: () => Promise.resolve({})
        })
    );
    const mockStore = configureMockStore(middleware);
    const store = mockStore({ oidc: { user: { profile: {} } }, historyStore: { push: jest.fn() } });
    return (
        <Provider store={store}>
            <ThemeProvider theme={createTheme()}>
                <SnackbarProvider dense maxSnack={5}>
                    <MemoryRouter>
                        <LocalizationProvider dateAdapter={AdapterMoment}>
                            {children}
                        </LocalizationProvider>
                    </MemoryRouter>
                </SnackbarProvider>
            </ThemeProvider>
        </Provider>
    );
};

const customRender = (ui, options) => render(ui, { wrapper: Providers, ...options });

export default customRender;
