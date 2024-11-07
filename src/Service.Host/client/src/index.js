import React, { useState, useEffect } from 'react';
import { createRoot } from 'react-dom/client';
import { SnackbarProvider } from 'notistack';
import { ThemeProvider, StyledEngineProvider } from '@mui/material/styles';
import { linnTheme } from '@linn-it/linn-form-components-library';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { loadUser } from 'redux-oidc';
import { AdapterMoment } from '@mui/x-date-pickers/AdapterMoment';
import PropTypes from 'prop-types';

import configureStore from './configureStore';
import Root from './components/Root';
import userManager from './helpers/userManager';
import 'typeface-roboto';
import LoadingPage from './components/LoadingPage';

const initialState = {};
const store = configureStore(initialState);
const container = document.getElementById('root');
const root = createRoot(container);

function AppContent({ isLoading }) {
    return isLoading ? <LoadingPage /> : <Root store={store} />;
}

function App() {
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        const checkAuthStatus = async () => {
            const { user } = store.getState().oidc;

            if (
                (!user || user.expired) &&
                window.location.pathname !== '/purchasing/signin-oidc-client'
            ) {
                try {
                    await userManager.signinSilent();
                } catch (error) {
                    // if silent renew fails, user will need to be redirected to sign in
                    await userManager.signinRedirect({
                        data: { redirect: window.location.pathname + window.location.search }
                    });
                }
            }

            // share user information with Redux store
            loadUser(store, userManager);

            setIsLoading(false); // authentication complete, app is ready to render
        };

        checkAuthStatus();
    }, []);

    document.body.style.margin = '0';

    return (
        <StyledEngineProvider injectFirst>
            <ThemeProvider theme={linnTheme}>
                <SnackbarProvider dense maxSnack={5}>
                    <LocalizationProvider dateAdapter={AdapterMoment} locale="en-GB">
                        <AppContent isLoading={isLoading} />
                    </LocalizationProvider>
                </SnackbarProvider>
            </ThemeProvider>
        </StyledEngineProvider>
    );
}

AppContent.propTypes = { isLoading: PropTypes.bool.isRequired };

root.render(<App />);
