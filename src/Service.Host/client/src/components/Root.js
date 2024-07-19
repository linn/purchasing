import React from 'react';
import { Provider } from 'react-redux';
import { Route, Routes } from 'react-router';
import { OidcProvider } from 'redux-oidc';
import { Navigate, unstable_HistoryRouter as HistoryRouter } from 'react-router-dom';
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
import PurchaseOrderSearch from './PurchaseOrders/PurchaseOrderSearch';
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
import CreateChangeRequest from './ChangeRequests/CreateChangeRequest';
import ChangeRequest from './ChangeRequests/ChangeRequest';
import ChangeRequestReplace from './ChangeRequests/ChangeRequestReplace';
import BomTypeChange from './BomTypeChange';
import Board from './boards/Board';
import BoardSearch from './boards/BoardSearch';
import PartsOnBomReport from './reports/PartsOnBomReport';
import BomCostReport from './reports/BomCostReport';
import BomUtility from './BomUtility/BomUtility';
import BomCostReportOptions from './reports/BomCostReportOptions';
import BoardsSummary from './boards/BoardsSummary';
import BoardComponents from './boards/BoardComponents';
import BomUtilitySearch from './BomUtility/BomUtilitySearch';
import CreateCreditNote from './plDebitCreditNotes/CreateCreditNote';
import CreateBomVerificationHistory from './BomVerificationHistoryUtility/CreateBomVerificationHistory';
import BomVerificationHistory from './BomVerificationHistoryUtility/BomVerificationHistory';
import SetBomStandardPriceUtility from './SetBomsStandardPriceUtility';
import BomHistoryReport from './BomHistoryReport';
import BoardDifferenceReportOptions from './reports/BoardDifferenceReportOptions';
import BoardDifferenceReport from './reports/BoardDifferenceReport';
import PartDataSheetValues from './PartDataSheetValues';
import BomVerificationHistorySearch from './BomVerificationHistoryUtility/BomVerificationHistorySearch';
import BomDifferenceReport from './reports/BomDifferenceReport';
import ChangeStatusReportOptions from './reports/ChangeStatusReportOptions';
import ChangeStatusReport from './reports/ChangeStatusReport';
import AcceptedChangesReport from './reports/AcceptedChangesReport';
import ProposedChangesReport from './reports/ProposedChangesReport';
import OutstandingChangesReport from './reports/OutstandingChangesReport';
import BoardComponentsSmtCheck from './boards/BoardComponentsSmtCheck';
import CurrentPhaseInWeeksReport from './reports/CurrentPhaseInWeeksReport';
import BoardComponentSummaryReportOptions from './reports/BoardComponentSummaryReportOptions';
import BoardComponentSummaryReport from './reports/BoardComponentSummaryReport';
import BomPrintReport from './reports/BomPrintReport';

function Root({ store }) {
    return (
        <div>
            <div className="padding-top-when-not-printing">
                <Provider store={store}>
                    <OidcProvider store={store} userManager={userManager}>
                        <>
                            <Navigation />
                            <HistoryRouter history={history}>
                                <Routes>
                                    <Route exact path="/purchasing" element={<App />} />
                                    <Route
                                        exact
                                        path="/"
                                        element={<Navigate to="/purchasing" replace />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/signin-oidc-client"
                                        element={<Callback />}
                                    />

                                    <Route
                                        exact
                                        path="/accounts/signin-oidc-client"
                                        element={<Callback />}
                                    />

                                    <Route
                                        exact
                                        path="/purchasing/suppliers"
                                        element={<SuppliersSearch />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/suppliers/create"
                                        element={<Supplier creating />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/suppliers/bulk-lead-times"
                                        element={<BulkLeadTimesUpload />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/suppliers/:id"
                                        element={<Supplier />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/signing-limits"
                                        element={<SigningLimits />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/signin-oidc-client"
                                        element={<Callback />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/part-suppliers"
                                        element={<PartSupplierSearch />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/part-suppliers/record"
                                        element={<PartSupplier />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/part-suppliers/create"
                                        // eslint-disable-next-line react/jsx-props-no-spreading
                                        element={<PartSupplier creating />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/orders-by-supplier"
                                        element={<OrdersBySupplierOptions />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/orders-by-supplier/report"
                                        element={<OrdersBySupplier />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/orders-by-part"
                                        element={<OrdersByPartOptions />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/orders-by-part/report"
                                        element={<OrdersByPart />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/spend-by-supplier"
                                        element={<SpendBySupplierOptions />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/spend-by-supplier/report"
                                        element={<SpendBySupplier />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/spend-by-supplier-by-date-range"
                                        element={<SpendBySupplierByDateRangeOptions />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/spend-by-supplier-by-date-range/report"
                                        element={<SpendBySupplierByDateRange />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/suppliers-with-unacknowledged-orders"
                                        element={<SuppliersWithUnacknowledgedOrders />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/unacknowledged-orders"
                                        element={<UnacknowledgedOrdersReport />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/spend-by-part"
                                        element={<SpendByPartOptions />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/spend-by-part/report"
                                        element={<SpendByPart />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/spend-by-part-by-date/report"
                                        element={<SpendByPartByDate />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/pl-credit-debit-notes"
                                        element={<Search />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/pl-credit-debit-notes/create"
                                        element={<CreateCreditNote />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/pl-credit-debit-notes/:id"
                                        element={<Note />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/boms/boards"
                                        element={<BoardSearch />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/boms/boards/create"
                                        element={<Board creating />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/boms/boards/:id"
                                        element={<Board />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/part-suppliers/create"
                                        element={<Board creating />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/boms/boards-summary"
                                        element={<BoardsSummary />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/boms/board-components"
                                        element={<BoardComponents />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/boms/board-components-smt-check"
                                        element={<BoardComponentsSmtCheck />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/boms/board-components/:id"
                                        element={<BoardComponents />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/open-debit-notes"
                                        element={<OpenDebitNotes />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/purchase-orders/"
                                        element={<PurchaseOrderSearch />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/purchase-orders/auth-or-send"
                                        element={<PurchaseOrdersAuthSend />}
                                    />
                                    <Route
                                        path="/purchasing/purchase-orders/reqs/create"
                                        element={<POReqUtility creating />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/purchase-orders/reqs/print"
                                        element={<POReqSearch print />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/purchase-orders/reqs/:id"
                                        element={<POReqUtility />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/purchase-orders/reqs/:id/print"
                                        element={<POReqPrintout />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/purchase-orders/reqs"
                                        element={<POReqSearch />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/purchase-orders/:orderNumber/allow-over-book"
                                        element={<AllowPurchaseOrderOverbook />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/purchase-orders/allow-over-book"
                                        element={<OverbooksSearch />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/purchase-orders/acknowledge"
                                        element={<AcknowledgeOrdersUtility />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/purchase-orders/quick-create"
                                        element={<CreatePurchaseOrderUt />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/purchase-orders/create"
                                        element={<PurchaseOrderUtility creating />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/purchase-orders/:orderNumber"
                                        element={<PurchaseOrderUtility />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/parts-received"
                                        element={<PartsReceivedReport />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/whats-due-in"
                                        element={<WhatsDueInReport />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/outstanding-po-reqs"
                                        element={<OutstandingPoReqsReport />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/whats-in-inspection"
                                        element={<WhatsInInspectionReport />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/pref-sup-receipts"
                                        element={<PrefSupReceiptsReport />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/material-requirements/run-mrp"
                                        element={<RunMrp />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/forecasting/apply-percentage-change"
                                        element={<ApplyForecastingPercentageChange />}
                                    />
                                    <Route exact path="/purchasing/edi" element={<EdiOrders />} />
                                    <Route
                                        exact
                                        path="/purchasing/material-requirements/used-on-report"
                                        element={<MrUsedOnReport />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/purchase-orders/acknowledge"
                                        element={<AcknowledgeOrdersUtility />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/material-requirements"
                                        element={<MaterialRequirements />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/material-requirements/report"
                                        element={<MaterialRequirementsReport />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/shortages"
                                        element={<ShortagesReportOptions />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/shortages/report"
                                        element={<ShortagesReport />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/shortages-planner/report"
                                        element={<ShortagesPlannerReport />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/mr-order-book"
                                        element={<MrOrderBookReport />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/automatic-purchase-order-suggestions"
                                        element={<AutomaticPurchaseOrderSuggestions />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/automatic-purchase-orders"
                                        element={<AutomaticPurchaseOrderSuggestions />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/automatic-purchase-orders/:id"
                                        element={<AutomaticPurchaseOrders />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/leadtimes"
                                        element={<SupplierLeadTimesReportOptions />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/leadtimes/report"
                                        element={<SupplierLeadTimesReport />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/forecast-order-reports"
                                        element={<ForecastOrdersReport />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/delivery-performance-summary"
                                        element={<DeliveryPerformanceSummary />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/delivery-performance-summary/report"
                                        element={<DeliveryPerformanceSummaryReport />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/delivery-performance-supplier/report"
                                        element={<DeliveryPerformanceSupplierReport />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/delivery-performance-details/report"
                                        element={<DeliveryPerformanceDetailReport />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/boms/tree/options"
                                        element={<BomTreeOptions />}
                                    />

                                    <Route
                                        exact
                                        path="/purchasing/boms/tree"
                                        element={<BomTreeReport />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/change-requests"
                                        element={<ChangeRequestSearch />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/change-requests/create"
                                        element={<CreateChangeRequest />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/change-requests/replace"
                                        element={<ChangeRequestReplace />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/change-requests/:id"
                                        element={<ChangeRequest />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/bom-type-change"
                                        element={<BomTypeChange />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/boms/reports/list"
                                        element={<PartsOnBomReport />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/boms/reports/bom-print"
                                        element={<BomPrintReport />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/boms/reports/cost/options"
                                        element={<BomCostReportOptions />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/boms/reports/cost"
                                        element={<BomCostReport />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/boms"
                                        element={<BomUtilitySearch />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/boms/bom-utility"
                                        element={<BomUtility />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/bom-verification/create"
                                        element={<CreateBomVerificationHistory />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/bom-verification/:id"
                                        element={<BomVerificationHistory />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/boms/standards-set"
                                        element={<SetBomStandardPriceUtility />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/bom-history"
                                        element={<BomHistoryReport />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/board-difference"
                                        element={<BoardDifferenceReportOptions />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/board-difference/report"
                                        element={<BoardDifferenceReport />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/part-data-sheet-values"
                                        element={<PartDataSheetValues />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/bom-verification"
                                        element={<BomVerificationHistorySearch />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/boms/reports/diff"
                                        element={<BomDifferenceReport />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/change-status"
                                        element={<ChangeStatusReportOptions />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/change-status/report"
                                        element={<ChangeStatusReport />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/accepted-changes/report"
                                        element={<AcceptedChangesReport />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/proposed-changes/report"
                                        element={<ProposedChangesReport />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/outstanding-changes/report"
                                        element={<OutstandingChangesReport />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/reports/current-phase-in-weeks/report"
                                        element={<CurrentPhaseInWeeksReport />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/boms/reports/board-component-summary"
                                        element={<BoardComponentSummaryReportOptions />}
                                    />
                                    <Route
                                        exact
                                        path="/purchasing/boms/reports/board-component-summary/report"
                                        element={<BoardComponentSummaryReport />}
                                    />
                                    <Route element={<NotFoundPage />} />
                                </Routes>
                            </HistoryRouter>
                        </>
                    </OidcProvider>
                </Provider>
            </div>
        </div>
    );
}

Root.propTypes = {
    store: PropTypes.shape({}).isRequired
};

export default Root;
