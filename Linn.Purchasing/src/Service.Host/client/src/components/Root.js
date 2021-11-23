import React from 'react';
import { Provider } from 'react-redux';
import { Route, Redirect, Switch } from 'react-router';
import { OidcProvider } from 'redux-oidc';
import { ConnectedRouter } from 'connected-react-router';
import CssBaseline from '@material-ui/core/CssBaseline';
import { Navigation } from '@linn-it/linn-form-components-library';
import { MuiPickersUtilsProvider } from '@material-ui/pickers';
import MomentUtils from '@date-io/moment';
import PropTypes from 'prop-types';
import history from '../history';
import App from './App';
import Callback from './Callback';
import userManager from '../helpers/userManager';
import 'typeface-roboto';
import NotFoundPage from './NotFoundPage';

const Root = ({ store }) => (
    <div>
        <div className="padding-top-when-not-printing">
            <Provider store={store}>
                <OidcProvider store={store} userManager={userManager}>
                    <MuiPickersUtilsProvider utils={MomentUtils}>
                        <ConnectedRouter history={history}>
                            <div>
                                <Navigation />
                                <CssBaseline />

                                <Route exact path="/" render={() => <Redirect to="/template" />} />

                                <Route
                                    path="/"
                                    render={() => {
                                        document.title = 'Template';
                                        return false;
                                    }}
                                />

                                <Switch>
                                    <Route exact path="/template" component={App} />
                                    <Route
                                        exact
                                        path="/template/signin-oidc-client"
                                        component={Callback}
                                    />
                                    <Route component={NotFoundPage} />
                                </Switch>
                            </div>
                        </ConnectedRouter>
                    </MuiPickersUtilsProvider>
                </OidcProvider>
            </Provider>
        </div>
    </div>
);

Root.propTypes = {
    store: PropTypes.shape({}).isRequired
};

export default Root;
