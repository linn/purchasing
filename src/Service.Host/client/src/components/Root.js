import React from 'react';
import { Provider } from 'react-redux';
import { Route, Redirect, Switch } from 'react-router';
import { OidcProvider } from 'redux-oidc';
import { ConnectedRouter } from 'connected-react-router';
import { Navigation } from '@linn-it/linn-form-components-library';
import PropTypes from 'prop-types';
import history from '../history';
import App from './App';
import Callback from './Callback';
import userManager from '../helpers/userManager';
import 'typeface-roboto';
import NotFoundPage from './NotFoundPage';
import SigningLimits from './SigningLimits';
import PartSupplierSearch from './partSupplierUtility/PartSupplierSearch';

const Root = ({ store }) => (
    <div>
        <div className="padding-top-when-not-printing">
            <Provider store={store}>
                <OidcProvider store={store} userManager={userManager}>
                    <ConnectedRouter history={history}>
                        <div>
                            <Navigation />
                            <Route exact path="/" render={() => <Redirect to="/purchasing" />} />

                            <Route
                                path="/"
                                render={() => {
                                    document.title = 'Purchasing';
                                    return false;
                                }}
                            />
                            <Switch>
                                <Route exact path="/purchasing" component={App} />
                                <Route
                                    exact
                                    path="/purchasing/signing-limits"
                                    component={SigningLimits}
                                />
                                <Route
                                    exact
                                    path="/purchasing/signin-oidc-client"
                                    component={Callback}
                                />
                                <Route
                                    exact
                                    path="/purchasing/part-supplier-search"
                                    component={PartSupplierSearch}
                                />
                                <Route component={NotFoundPage} />
                            </Switch>
                        </div>
                    </ConnectedRouter>
                </OidcProvider>
            </Provider>
        </div>
    </div>
);

Root.propTypes = {
    store: PropTypes.shape({}).isRequired
};

export default Root;
