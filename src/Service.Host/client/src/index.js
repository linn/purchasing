import React from 'react';
import ReactDOM from 'react-dom';
import { AppContainer } from 'react-hot-loader';
import { SnackbarProvider } from 'notistack';
import { ThemeProvider, createTheme, StyledEngineProvider } from '@mui/material/styles';

import { grey, red } from '@mui/material/colors';

import configureStore from './configureStore';

import Root from './components/Root';
import userManager from './helpers/userManager';
import 'typeface-roboto';

const NextRoot = require('./components/Root').default;

const theme = createTheme({
    palette: {
        primary: {
            50: '#e0f7fa',
            100: '#b2ebf2',
            200: '#80deea',
            300: '#4dd0e1',
            400: '#26c6da',
            500: '#00bcd4',
            600: '#00acc1',
            700: '#0097a7',
            800: '#00838f',
            900: '#006064',
            A100: '#84ffff',
            A200: '#18ffff',
            A400: '#00e5ff',
            A700: '#00b8d4',
            contrastText: 'rgba(0, 0, 0, 0.87)',
            dark: '#0097a7',
            light: '#4dd0e1',
            main: '#00acc1'
        },
        secondary: red,
        grey
    }
});
const initialState = {};
const store = configureStore(initialState);
const { user } = store.getState().oidc;

const render = Component => {
    ReactDOM.render(
        <StyledEngineProvider injectFirst>
            <ThemeProvider theme={theme}>
                <SnackbarProvider dense maxSnack={5}>
                    <AppContainer>
                        <Component store={store} />
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
