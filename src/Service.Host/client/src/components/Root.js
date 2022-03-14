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
import SuppliersSearch from './supplierUtility/SuppliersSearch';
import OrdersByPartOptions from './reports/OrdersByPartOptions';
import SpendBySupplierOptions from './reports/SpendBySupplierOptions';
import SpendBySupplier from './reports/SpendBySupplier';
import Supplier from './supplierUtility/Supplier';
import AddressUtility from './AdressUtility';
import SuppliersWithUnacknowledgedOrders from './reports/SuppliersWithUnacknowledgedOrders';
import UnacknowledgedOrdersReport from './reports/UnacknowledgedOrdersReport';
import SpendByPartOptions from './reports/SpendByPartOptions';
import SpendByPart from './reports/SpendByPart';
import OpenDebitNotes from './plDebitCreditNotes/OpenDebitNotes';
import Search from './plDebitCreditNotes/Search';
import Note from './plDebitCreditNotes/Note';
import BulkLeadTimesUpload from './partSupplierUtility/BulkLeadTimesUpload';

const Root = ({ store }) => (
    <div>
        <div className="padding-top-when-not-printing">
            <Provider store={store}>
                <OidcProvider store={store} userManager={userManager}>
                    <ConnectedRouter history={history}>
                        <div>
                            <Navigation />
                            <Switch>
                                <Route exact path="/purchasing" component={App} />
                                <Redirect exact from="/" to="/purchasing" />
                                <Redirect exact from="/purchasing/reports" to="/purchasing" />
                                <Route
                                    exact
                                    path="/purchasing/suppliers"
                                    component={SuppliersSearch}
                                />
                                <Route
                                    exact
                                    path="/purchasing/suppliers/create"
                                    // eslint-disable-next-line react/jsx-props-no-spreading
                                    render={props => <Supplier creating {...props} />}
                                />
                                <Route
                                    exact
                                    path="/purchasing/suppliers/:id"
                                    component={Supplier}
                                />
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
                                    path="/purchasing/part-suppliers/bulk-lead-times"
                                    component={BulkLeadTimesUpload}
                                />
                                <Route
                                    exact
                                    path="/purchasing/part-suppliers/record"
                                    component={PartSupplier}
                                />
                                <Route
                                    exact
                                    path="/purchasing/part-suppliers/create"
                                    // eslint-disable-next-line react/jsx-props-no-spreading
                                    render={props => <PartSupplier creating {...props} />}
                                />
                                <Route
                                    exact
                                    path="/purchasing/addresses"
                                    component={AddressUtility}
                                />
                                <Route
                                    exact
                                    path="/purchasing/reports/orders-by-supplier"
                                    component={OrdersBySupplierOptions}
                                />
                                <Route
                                    exact
                                    path="/purchasing/reports/orders-by-supplier/report"
                                    component={OrdersBySupplier}
                                />
                                <Route
                                    exact
                                    path="/purchasing/reports/orders-by-part"
                                    component={OrdersByPartOptions}
                                />
                                <Route
                                    exact
                                    path="/purchasing/reports/orders-by-part/report"
                                    component={OrdersByPart}
                                />
                                <Route
                                    exact
                                    path="/purchasing/reports/spend-by-supplier"
                                    component={SpendBySupplierOptions}
                                />
                                <Route
                                    exact
                                    path="/purchasing/reports/spend-by-supplier/report"
                                    component={SpendBySupplier}
                                />
                                <Route
                                    exact
                                    path="/purchasing/reports/suppliers-with-unacknowledged-orders"
                                    component={SuppliersWithUnacknowledgedOrders}
                                />
                                <Route
                                    exact
                                    path="/purchasing/reports/unacknowledged-orders"
                                    component={UnacknowledgedOrdersReport}
                                />
                                <Route
                                    exact
                                    path="/purchasing/reports/spend-by-part"
                                    component={SpendByPartOptions}
                                />
                                <Route
                                    exact
                                    path="/purchasing/reports/spend-by-part/report"
                                    component={SpendByPart}
                                />
                                <Route
                                    exact
                                    path="/purchasing/pl-credit-debit-notes"
                                    component={Search}
                                />
                                <Route
                                    exact
                                    path="/purchasing/pl-credit-debit-notes/:id"
                                    component={Note}
                                />
                                <Route
                                    exact
                                    path="/purchasing/open-debit-notes"
                                    component={OpenDebitNotes}
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
