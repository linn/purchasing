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
import PartSupplier from './partSupplierUtility/PartSupplier';
import OrdersBySupplier from './reports/OrdersBySupplier';
import OrdersBySupplierOptions from './reports/OrdersBySupplierOptions';
import OrdersByPart from './reports/OrdersByPart';
import OrdersByPartOptions from './reports/OrdersByPartOptions';

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
                                    path="/purchasing/part-suppliers"
                                    component={PartSupplierSearch}
                                />
                                <Route
                                    exact
                                    path="/purchasing/part-suppliers/record"
                                    component={PartSupplier}
                                />
                                <Route
                                    exact
                                    path="/purchasing/part-suppliers/create"
                                    component={PartSupplier}
                                />
                                <Route
                                    exact
                                    path="/purchasing/reports/orders-by-supplier"
                                    component={OrdersBySupplierOptions}
                                />
                                <Route
                                    exact
                                    path="/purchasing/reports/orders-by-supplier/:id"
                                    component={OrdersBySupplier}
                                />
                                <Route
                                    exact
                                    path="/purchasing/reports/orders-by-part"
                                    component={OrdersByPartOptions}
                                />
                                <Route
                                    exact
                                    path="/purchasing/reports/orders-by-part/:id"
                                    component={OrdersByPart}
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
