import { render } from '@testing-library/react';
import { Provider } from 'react-redux';
import React from 'react';
import { MuiThemeProvider, createTheme } from '@material-ui/core/styles';
import configureMockStore from 'redux-mock-store';
import { MemoryRouter } from 'react-router-dom';
import { SnackbarProvider } from 'notistack';
import { MuiPickersUtilsProvider } from '@material-ui/pickers';
import MomentUtils from '@date-io/moment';
import { apiMiddleware as api } from 'redux-api-middleware';
import thunkMiddleware from 'redux-thunk';

const middleware = [api, thunkMiddleware];

// eslint-disable-next-line react/prop-types
const Providers = ({ children }) => {
    const mockStore = configureMockStore(middleware);
    const store = mockStore({ oidc: { user: { profile: {} } }, historyStore: { push: jest.fn() } });
    return (
        <Provider store={store}>
            <MuiThemeProvider theme={createTheme()}>
                <SnackbarProvider dense maxSnack={5}>
                    <MemoryRouter>
                        <MuiPickersUtilsProvider utils={MomentUtils}>
                            {children}
                        </MuiPickersUtilsProvider>
                    </MemoryRouter>
                </SnackbarProvider>
            </MuiThemeProvider>
        </Provider>
    );
};

const customRender = (ui, options) => render(ui, { wrapper: Providers, ...options });

export default customRender;
