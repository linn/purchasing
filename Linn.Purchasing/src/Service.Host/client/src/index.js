import React from 'react';
import ReactDOM from 'react-dom';
import { AppContainer } from 'react-hot-loader';
import { SnackbarProvider } from 'notistack';
import { linnTheme } from '@linn-it/linn-form-components-library';
import { ThemeProvider } from '@material-ui/styles';
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
        <ThemeProvider theme={linnTheme}>
            <SnackbarProvider dense maxSnack={5}>
                <AppContainer>
                    <Component store={store} />
                </AppContainer>
            </SnackbarProvider>
        </ThemeProvider>,
        document.getElementById('root')
    );
};

if ((!user || user.expired) && window.location.pathname !== '/template/signin-oidc-client') {
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
