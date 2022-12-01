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
import SpendBySupplierByDateRangeOptions from './reports/SpendBySupplierByDateRangeOptions';
import SpendBySupplierByDateRange from './reports/SpendBySupplierByDateRange';
import Supplier from './supplierUtility/Supplier';
import AddressUtility from './AddressUtility';
import SuppliersWithUnacknowledgedOrders from './reports/SuppliersWithUnacknowledgedOrders';
import UnacknowledgedOrdersReport from './reports/UnacknowledgedOrdersReport';
import SpendByPartOptions from './reports/SpendByPartOptions';
import SpendByPart from './reports/SpendByPart';
import SpendByPartByDate from './reports/SpendByPartByDate';
import OpenDebitNotes from './plDebitCreditNotes/OpenDebitNotes';
import Search from './plDebitCreditNotes/Search';
import Note from './plDebitCreditNotes/Note';
import BulkLeadTimesUpload from './partSupplierUtility/BulkLeadTimesUpload';
import PartsReceivedReport from './reports/PartsReceivedReport';
import POReqUtility from './POReqs/POReqUtility';
import POReqSearch from './POReqs/POReqSearch';
import OverbooksSearch from './PurchaseOrders/OverbooksSearch';
import AllowPurchaseOrderOverbook from './PurchaseOrders/AllowPurchaseOrderOverbook';
import WhatsDueInReport from './reports/WhatsDueInReport';
import OutstandingPoReqsReport from './reports/OutstandingPoReqsReport';
import PrefSupReceiptsReport from './reports/PrefSupReceiptsReport';
import POReqPrintout from './POReqs/POReqPrintout';
import WhatsInInspectionReport from './reports/WhatsInInspectionReport';
import RunMrp from './RunMrp';
import ApplyForecastingPercentageChange from './ApplyForecastingPercentageChange';
import EdiOrders from './EdiOrders';
import PurchaseOrderUtility from './PurchaseOrders/PurchaseOrderUtility';
import PurchaseOrdersSearch from './PurchaseOrders/PurchaseOrdersSearch';
import MrUsedOnReport from './reports/MrUsedOnReport';
import AcknowledgeOrdersUtility from './AcknowledgeOrdersUtility';
import MaterialRequirements from './materialRequirements/MaterialRequirementsOptions';
import MaterialRequirementsReport from './materialRequirements/MaterialRequirementsReport';
import ShortagesReport from './reports/ShortagesReport';
import ShortagesReportOptions from './reports/ShortagesReportOptions';
import ShortagesPlannerReport from './reports/ShortagesPlannerReport';
import MrOrderBookReport from './reports/MrOrderBookReport';
import AutomaticPurchaseOrderSuggestions from './AutomaticPurchaseOrderSuggestions';
import AutomaticPurchaseOrders from './AutomaticPurchaseOrders';
import SupplierLeadTimesReport from './reports/SupplierLeadTimesReport';
import SupplierLeadTimesReportOptions from './reports/SupplierLeadTimesReportOptions';
import ForecastOrdersReport from './reports/ForecastOrdersReports';
import DeliveryPerformanceSummary from './reports/DeliveryPerformanceSummary';
import DeliveryPerformanceSummaryReport from './reports/DeliveryPerformanceSummaryReport';
import DeliveryPerformanceSupplierReport from './reports/DeliveryPerformanceSupplierReport';
import DeliveryPerformanceDetailReport from './reports/DeliveryPerformanceDetailReport';
import CreatePurchaseOrderUt from './PurchaseOrders/CreatePurchaseOrderUt';
import PurchaseOrdersAuthSend from './PurchaseOrders/PurchaseOrdersAuthSend';
import BomTreeReport from './BomTreeReport';
import BomTreeOptions from './BomTreeOptions';
import ChangeRequestSearch from './ChangeRequests/ChangeRequestsSearch';
import ChangeRequest from './ChangeRequests/ChangeRequest';
import BomTypeChange from './BomTypeChange';
import Board from './boards/Board';
import BoardSearch from './boards/BoardSearch';
import PartsOnBomReport from './reports/PartsOnBomReport';
import BomCostReport from './reports/BomCostReport';
import BomUtility from './BomUtility/BomUtility';
import BomCostReportOptions from './reports/BomCostReportOptions';

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
                                    path="/purchasing/suppliers/bulk-lead-times"
                                    component={BulkLeadTimesUpload}
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
                                    path="/purchasing/reports/spend-by-supplier-by-date-range"
                                    component={SpendBySupplierByDateRangeOptions}
                                />
                                <Route
                                    exact
                                    path="/purchasing/reports/spend-by-supplier-by-date-range/report"
                                    component={SpendBySupplierByDateRange}
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
                                    path="/purchasing/reports/spend-by-part-by-date/report"
                                    component={SpendByPartByDate}
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
                                    path="/purchasing/boms/boards"
                                    component={BoardSearch}
                                />
                                <Route
                                    exact
                                    path="/purchasing/boms/boards/create"
                                    render={() => <Board creating />}
                                />
                                <Route exact path="/purchasing/boms/boards/:id" component={Board} />
                                <Route
                                    exact
                                    path="/purchasing/part-suppliers/create"
                                    render={() => <Board creating />}
                                />
                                <Route
                                    exact
                                    path="/purchasing/open-debit-notes"
                                    component={OpenDebitNotes}
                                />
                                <Route
                                    exact
                                    path="/purchasing/purchase-orders/"
                                    component={PurchaseOrdersSearch}
                                />
                                <Route
                                    exact
                                    path="/purchasing/purchase-orders/auth-or-send"
                                    component={PurchaseOrdersAuthSend}
                                />
                                <Route
                                    path="/purchasing/purchase-orders/reqs/create"
                                    // eslint-disable-next-line react/jsx-props-no-spreading
                                    render={props => <POReqUtility creating {...props} />}
                                />
                                <Route
                                    exact
                                    path="/purchasing/purchase-orders/reqs/print"
                                    // eslint-disable-next-line react/jsx-props-no-spreading
                                    render={props => <POReqSearch print {...props} />}
                                />
                                <Route
                                    exact
                                    path="/purchasing/purchase-orders/reqs/:id"
                                    component={POReqUtility}
                                />
                                <Route
                                    exact
                                    path="/purchasing/purchase-orders/reqs/:id/print"
                                    component={POReqPrintout}
                                />
                                <Route
                                    exact
                                    path="/purchasing/purchase-orders/reqs"
                                    component={POReqSearch}
                                />
                                <Route
                                    exact
                                    path="/purchasing/purchase-orders/:orderNumber/allow-over-book"
                                    component={AllowPurchaseOrderOverbook}
                                />
                                <Route
                                    exact
                                    path="/purchasing/purchase-orders/allow-over-book"
                                    component={OverbooksSearch}
                                />
                                <Route
                                    exact
                                    path="/purchasing/purchase-orders/acknowledge"
                                    component={AcknowledgeOrdersUtility}
                                />
                                <Route
                                    exact
                                    path="/purchasing/purchase-orders/quick-create"
                                    component={CreatePurchaseOrderUt}
                                />
                                <Route
                                    exact
                                    path="/purchasing/purchase-orders/create"
                                    // eslint-disable-next-line react/jsx-props-no-spreading
                                    render={props => <PurchaseOrderUtility creating {...props} />}
                                />
                                <Route
                                    exact
                                    path="/purchasing/purchase-orders/:orderNumber"
                                    component={PurchaseOrderUtility}
                                />
                                <Route
                                    exact
                                    path="/purchasing/reports/parts-received"
                                    component={PartsReceivedReport}
                                />
                                <Route
                                    exact
                                    path="/purchasing/reports/whats-due-in"
                                    component={WhatsDueInReport}
                                />
                                <Route
                                    exact
                                    path="/purchasing/reports/outstanding-po-reqs"
                                    component={OutstandingPoReqsReport}
                                />
                                <Route
                                    exact
                                    path="/purchasing/reports/whats-in-inspection"
                                    component={WhatsInInspectionReport}
                                />
                                <Route
                                    exact
                                    path="/purchasing/reports/pref-sup-receipts"
                                    component={PrefSupReceiptsReport}
                                />
                                <Route
                                    exact
                                    path="/purchasing/material-requirements/run-mrp"
                                    component={RunMrp}
                                />
                                <Route
                                    exact
                                    path="/purchasing/forecasting/apply-percentage-change"
                                    component={ApplyForecastingPercentageChange}
                                />
                                <Route exact path="/purchasing/edi" component={EdiOrders} />
                                <Route
                                    exact
                                    path="/purchasing/material-requirements/used-on-report"
                                    component={MrUsedOnReport}
                                />
                                <Route
                                    exact
                                    path="/purchasing/purchase-orders/acknowledge"
                                    component={AcknowledgeOrdersUtility}
                                />
                                <Route
                                    exact
                                    path="/purchasing/material-requirements"
                                    component={MaterialRequirements}
                                />
                                <Route
                                    exact
                                    path="/purchasing/material-requirements/report"
                                    component={MaterialRequirementsReport}
                                />
                                <Route
                                    exact
                                    path="/purchasing/reports/shortages"
                                    component={ShortagesReportOptions}
                                />
                                <Route
                                    exact
                                    path="/purchasing/reports/shortages/report"
                                    component={ShortagesReport}
                                />
                                <Route
                                    exact
                                    path="/purchasing/reports/shortages-planner/report"
                                    component={ShortagesPlannerReport}
                                />
                                <Route
                                    exact
                                    path="/purchasing/reports/mr-order-book"
                                    component={MrOrderBookReport}
                                />
                                <Route
                                    exact
                                    path="/purchasing/automatic-purchase-order-suggestions"
                                    component={AutomaticPurchaseOrderSuggestions}
                                />
                                <Route
                                    exact
                                    path="/purchasing/automatic-purchase-orders"
                                    component={AutomaticPurchaseOrderSuggestions}
                                />
                                <Route
                                    exact
                                    path="/purchasing/automatic-purchase-orders/:id"
                                    component={AutomaticPurchaseOrders}
                                />
                                <Route
                                    exact
                                    path="/purchasing/reports/leadtimes"
                                    component={SupplierLeadTimesReportOptions}
                                />
                                <Route
                                    exact
                                    path="/purchasing/reports/leadtimes/report"
                                    component={SupplierLeadTimesReport}
                                />
                                <Route
                                    exact
                                    path="/purchasing/reports/forecast-order-reports"
                                    component={ForecastOrdersReport}
                                />
                                <Route
                                    exact
                                    path="/purchasing/reports/delivery-performance-summary"
                                    component={DeliveryPerformanceSummary}
                                />
                                <Route
                                    exact
                                    path="/purchasing/reports/delivery-performance-summary/report"
                                    component={DeliveryPerformanceSummaryReport}
                                />
                                <Route
                                    exact
                                    path="/purchasing/reports/delivery-performance-supplier/report"
                                    component={DeliveryPerformanceSupplierReport}
                                />
                                <Route
                                    exact
                                    path="/purchasing/reports/delivery-performance-details/report"
                                    component={DeliveryPerformanceDetailReport}
                                />
                                <Route
                                    exact
                                    path="/purchasing/boms/tree/options"
                                    component={BomTreeOptions}
                                />

                                <Route
                                    exact
                                    path="/purchasing/boms/tree"
                                    component={BomTreeReport}
                                />
                                <Route
                                    exact
                                    path="/purchasing/change-requests"
                                    component={ChangeRequestSearch}
                                />
                                <Route
                                    exact
                                    path="/purchasing/change-requests/:id"
                                    component={ChangeRequest}
                                />
                                <Route
                                    exact
                                    path="/purchasing/bom-type-change"
                                    component={BomTypeChange}
                                />
                                <Route
                                    exact
                                    path="/purchasing/boms/reports/list"
                                    component={PartsOnBomReport}
                                />
                                <Route
                                    exact
                                    path="/purchasing/boms/reports/cost/options"
                                    component={BomCostReportOptions}
                                />
                                <Route
                                    exact
                                    path="/purchasing/boms/reports/cost"
                                    component={BomCostReport}
                                />
                                <Route
                                    exact
                                    path="/purchasing/boms/bom-utility"
                                    component={BomUtility}
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
