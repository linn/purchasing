import React from 'react';
import ReactDOM from 'react-dom';
import { AppContainer } from 'react-hot-loader';
import { SnackbarProvider } from 'notistack';
import { ThemeProvider, StyledEngineProvider } from '@mui/material/styles';
import { linnTheme } from '@linn-it/linn-form-components-library';
import LocalizationProvider from '@mui/lab/LocalizationProvider';

import AdapterDateMoment from '@mui/lab/AdapterMoment';
import configureStore from './configureStore';
import Root from './components/Root';
import userManager from './helpers/userManager';
import 'typeface-roboto';

const NextRoot = require('./components/Root').default;

const initialState = {};
const store = configureStore(initialState);
const { user } = store.getState().oidc;

const render = Component => {
    ReactDOM.render(
        <StyledEngineProvider injectFirst>
            <ThemeProvider theme={linnTheme}>
                <SnackbarProvider dense maxSnack={5}>
                    <AppContainer>
                        <LocalizationProvider dateAdapter={AdapterDateMoment} locale="de-DE">
                            <Component store={store} />
                        </LocalizationProvider>
                    </AppContainer>
                </SnackbarProvider>
            </ThemeProvider>
        </StyledEngineProvider>,
        document.getElementById('root')
    );
};

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
