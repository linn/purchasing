import React from 'react';
import { createRoot } from 'react-dom/client';
import { SnackbarProvider } from 'notistack';
import { ThemeProvider, StyledEngineProvider } from '@mui/material/styles';
import { linnTheme } from '@linn-it/linn-form-components-library';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterMoment } from '@mui/x-date-pickers/AdapterMoment';
import configureStore from './configureStore';
import Root from './components/Root';
import userManager from './helpers/userManager';
import 'typeface-roboto';

const NextRoot = require('./components/Root').default;

const initialState = {};
const store = configureStore(initialState);
const { user } = store.getState().oidc;

const container = document.getElementById('root');
const root = createRoot(container);

const render = Component => {
    root.render(
        <StyledEngineProvider injectFirst>
            <ThemeProvider theme={linnTheme}>
                <SnackbarProvider dense maxSnack={5}>
                        <LocalizationProvider dateAdapter={AdapterMoment} locale="en-GB">
                            <Component store={store} />
                        </LocalizationProvider>
                </SnackbarProvider>
            </ThemeProvider>
        </StyledEngineProvider>
    );
};

document.body.style.margin = '0';

if ((!user || user.expired) && window.location.pathname !== '/purchasing/signin-oidc-client') {
    userManager.signinRedirect({
        data: { redirect: window.location.pathname + window.location.search }
    });
} else {
    render(Root);

    // Hot Module Replacement API
    if (module.hot) {
        //module.hot.accept('./reducers', () => store.replaceReducer(reducer));
        module.hot.accept('./components/Root', () => {
            render(NextRoot);
        });
    }
}
