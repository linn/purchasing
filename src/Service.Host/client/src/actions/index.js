import {
    makeActionTypes,
    makeReportActionTypes,
    makeProcessActionTypes
} from '@linn-it/linn-form-components-library';
import * as itemTypes from '../itemTypes';
import * as reportTypes from '../reportTypes';
import * as processTypes from '../processTypes';

export const signingLimitActionTypes = makeActionTypes(
    itemTypes.signingLimit.actionType,
    true,
    true
);
export const signingLimitsActionTypes = makeActionTypes(itemTypes.signingLimits.actionType, false);

export const partSuppliersActionTypes = makeActionTypes(itemTypes.partSuppliers.actionType, false);
export const partSupplierActionTypes = makeActionTypes(itemTypes.partSupplier.actionType);

export const employeesActionTypes = makeActionTypes(itemTypes.employees.actionType);

export const supplierActionTypes = makeActionTypes(itemTypes.supplier.actionType);

export const suppliersActionTypes = makeActionTypes(itemTypes.suppliers.actionType);

export const partsActionTypes = makeActionTypes(itemTypes.parts.actionType);

export const partActionTypes = makeActionTypes(itemTypes.part.actionType);

export const currenciesActionTypes = makeActionTypes(itemTypes.currencies.actionType);

export const orderMethodsActionTypes = makeActionTypes(itemTypes.orderMethods.actionType);

export const deliveryAddressesActionTypes = makeActionTypes(itemTypes.deliveryAddresses.actionType);

export const unitsOfMeasureActionTypes = makeActionTypes(itemTypes.unitsOfMeasure.actionType);

export const tariffsActionTypes = makeActionTypes(itemTypes.tariffs.actionType);

export const packagingGroupsActionTypes = makeActionTypes(itemTypes.packagingGroups.actionType);

export const manufacturersActionTypes = makeActionTypes(itemTypes.manufacturers.actionType);

export const ordersBySupplierReportActionTypes = makeActionTypes(
    reportTypes.ordersBySupplierReport.actionType,
    false
);

export const preferredSupplierChangeActionTypes = makeActionTypes(
    itemTypes.preferredSupplierChange.actionType
);

export const priceChangeReasonsActionTypes = makeActionTypes(
    itemTypes.priceChangeReasons.actionType,
    false
);

export const partPriceConversionsActionTypes = makeActionTypes(
    itemTypes.partPriceConversions.actionType,
    false
);

export const ordersByPartReportActionTypes = makeActionTypes(
    reportTypes.ordersByPartReport.actionType,
    false
);

export const partCategoriesActionTypes = makeActionTypes(
    itemTypes.partCategories.actionType,
    false
);

export const accountingCompaniesActionTypes = makeActionTypes(
    itemTypes.accountingCompanies.actionType,
    false
);

export const putSupplierOnHoldActionTypes = makeActionTypes(
    itemTypes.putSupplierOnHold.actionType,
    true
);

export const addressActionTypes = makeActionTypes(itemTypes.address.actionType, true);

export const addressesActionTypes = makeActionTypes(itemTypes.addresses.actionType, false);

export const countriesActionTypes = makeActionTypes(itemTypes.countries.actionType, false);

export const vendorManagersActionTypes = makeActionTypes(
    itemTypes.vendorManagers.actionType,
    false
);

export const spendBySupplierReportActionTypes = makeActionTypes(
    reportTypes.spendBySupplierReport.actionType,
    false
);

export const spendBySupplierByDateRangeReportActionTypes = makeActionTypes(
    reportTypes.spendBySupplierByDateRangeReport.actionType,
    false
);

export const suppliersWithUnacknowledgedOrdersActionTypes = makeReportActionTypes(
    reportTypes.suppliersWithUnacknowledgedOrders.actionType
);

export const unacknowledgedOrdersReportActionTypes = makeReportActionTypes(
    reportTypes.unacknowledgedOrdersReport.actionType
);

export const plannersActionTypes = makeActionTypes(itemTypes.planners.actionType);

export const spendByPartReportActionTypes = makeActionTypes(
    reportTypes.spendByPartReport.actionType,
    false
);

export const spendByPartByDateReportActionTypes = makeReportActionTypes(
    reportTypes.spendByPartByDateReport.actionType,
    false
);

export const plCreditDebitNoteActionTypes = makeActionTypes(itemTypes.plCreditDebitNote.actionType);

export const plCreditDebitNotesActionTypes = makeActionTypes(
    itemTypes.plCreditDebitNotes.actionType
);

export const openDebitNotesActionTypes = makeActionTypes(itemTypes.openDebitNotes.actionType);

export const sendPlNoteEmailActionTypes = makeProcessActionTypes(
    itemTypes.sendPlNoteEmail.actionType
);

export const sendPurchaseOrderReqEmailActionTypes = makeProcessActionTypes(
    itemTypes.sendPurchaseOrderReqEmail.actionType
);

export const bulkLeadTimesUploadActionTypes = makeProcessActionTypes(
    itemTypes.bulkLeadTimesUpload.actionType
);

export const supplierGroupsActionTypes = makeActionTypes(itemTypes.supplierGroups.actionType);

export const tqmsJobrefsActionTypes = makeActionTypes(itemTypes.tqmsJobrefs.actionType);

export const partsReceivedReportActionTypes = makeActionTypes(
    reportTypes.partsReceivedReport.actionType,
    false
);

export const purchaseOrderReqActionTypes = makeActionTypes(
    itemTypes.purchaseOrderReq.actionType,
    true
);

export const departmentsActionTypes = makeActionTypes(itemTypes.departments.actionType);

export const nominalsActionTypes = makeActionTypes(itemTypes.nominals.actionType);

export const purchaseOrderReqsActionTypes = makeActionTypes(
    itemTypes.purchaseOrderReqs.actionType,
    true
);
export const purchaseOrderActionTypes = makeActionTypes(itemTypes.purchaseOrder.actionType, true);

export const purchaseOrdersActionTypes = makeActionTypes(
    itemTypes.purchaseOrders.actionType,
    false
);

export const whatsDueInReportActionTypes = makeActionTypes(
    reportTypes.whatsDueInReport.actionType,
    false
);

export const purchaseOrderReqStatesActionTypes = makeActionTypes(
    itemTypes.purchaseOrderReqStates.actionType
);

export const outstandingPoReqsReportActionTypes = makeReportActionTypes(
    reportTypes.outstandingPoReqsReport.actionType,
    false
);

export const sendPurchaseOrderReqAuthEmailActionTypes = makeProcessActionTypes(
    itemTypes.sendPurchaseOrderReqAuthEmail.actionType
);

export const sendPurchaseOrderReqFinanceEmailActionTypes = makeProcessActionTypes(
    itemTypes.sendPurchaseOrderReqFinanceEmail.actionType
);

export const pOReqCheckIfCanAuthOrderActionTypes = makeProcessActionTypes(
    itemTypes.pOReqCheckIfCanAuthOrder.actionType
);

export const prefSupReceiptsReportActionTypes = makeReportActionTypes(
    reportTypes.prefSupReceiptsReport.actionType
);

export const whatsInInspectionReportActionTypes = makeReportActionTypes(
    reportTypes.whatsInInspectionReport.actionType
);

export const mrpRunLogActionTypes = makeActionTypes(itemTypes.mrpRunLog.actionType, true);

export const runMrpActionTypes = makeProcessActionTypes(itemTypes.runMrp.actionType);

export const mrMasterActionTypes = makeActionTypes(itemTypes.mrMaster.actionType, false);

export const applyForecastingPercentageChangeActionTypes = makeProcessActionTypes(
    itemTypes.applyForecastingPercentageChange.actionType
);

export const ediOrdersActionTypes = makeActionTypes(itemTypes.ediOrders.actionType, true);

export const sendEdiEmailActionTypes = makeActionTypes(itemTypes.sendEdiEmail.actionType);

export const mrUsedOnReportActionTypes = makeReportActionTypes(
    reportTypes.mrUsedOnReport.actionType
);

export const mrReportActionTypes = makeActionTypes(itemTypes.mrReport.actionType);
export const mrReportOptionsActionTypes = makeActionTypes(itemTypes.mrReportOptions.actionType);
export const mrReportOrdersActionTypes = makeActionTypes(itemTypes.mrReportOrders.actionType);

export const purchaseOrderDeliveryActionTypes = makeActionTypes(
    itemTypes.purchaseOrderDelivery.actionType
);

export const purchaseOrderDeliveriesActionTypes = makeActionTypes(
    itemTypes.purchaseOrderDeliveries.actionType,
    true
);

export const batchPurchaseOrderDeliveriesUploadActionTypes = makeProcessActionTypes(
    itemTypes.batchPurchaseOrderDeliveriesUpload.actionType,
    true
);

export const ediSuppliersActionTypes = makeActionTypes(itemTypes.ediSuppliers.actionType, true);

export const shortagesReportActionTypes = makeActionTypes(
    reportTypes.shortagesReport.actionType,
    false
);

export const shortagesPlannerReportActionTypes = makeActionTypes(
    reportTypes.shortagesPlannerReport.actionType,
    false
);

export const mrOrderBookReportActionTypes = makeActionTypes(
    reportTypes.mrOrderBookReport.actionType,
    false
);

export const sendPurchaseOrderPdfEmailActionTypes = makeProcessActionTypes(
    itemTypes.sendPurchaseOrderPdfEmail.actionType
);

export const automaticPurchaseOrderActionTypes = makeActionTypes(
    itemTypes.automaticPurchaseOrder.actionType,
    true
);

export const automaticPurchaseOrderSuggestionsActionTypes = makeActionTypes(
    itemTypes.automaticPurchaseOrderSuggestions.actionType,
    false
);

export const sendPurchaseOrderSupplierAssActionTypes = makeProcessActionTypes(
    itemTypes.sendPurchaseOrderSupplierAssEmail.actionType
);

export const supplierLeadTimesReportActionTypes = makeActionTypes(
    reportTypes.supplierLeadTimesReport.actionType,
    false
);

export const ledgerPeriodsActionTypes = makeActionTypes(itemTypes.ledgerPeriods.actionType, false);

export const deliveryPerformanceSummaryReportActionTypes = makeReportActionTypes(
    reportTypes.deliveryPerformanceSummaryReport.actionType
);

export const deliveryPerformanceSupplierReportActionTypes = makeReportActionTypes(
    reportTypes.deliveryPerformanceSupplierReport.actionType
);

export const deliveryPerformanceDetailReportActionTypes = makeReportActionTypes(
    reportTypes.deliveryPerformanceDetailReport.actionType
);

export const authoriseMultiplePurchaseOrdersActionTypes = makeProcessActionTypes(
    processTypes.authoriseMultiplePurchaseOrders.actionType
);

export const emailMultiplePurchaseOrdersActionTypes = makeProcessActionTypes(
    processTypes.emailMultiplePurchaseOrders.actionType
);

export const forecastWeekChangesReportActionTypes = makeReportActionTypes(
    reportTypes.forecastWeekChangesReport.actionType
);

export const changeRequestActionTypes = makeActionTypes(itemTypes.changeRequest.actionType);

export const changeRequestsActionTypes = makeActionTypes(itemTypes.changeRequests.actionType);

export const sendPurchaseOrderAuthEmailActionTypes = makeProcessActionTypes(
    itemTypes.sendPurchaseOrderAuthEmail.actionType
);

export const sendPurchaseOrderDeptEmailActionTypes = makeActionTypes(
    itemTypes.sendPurchaseOrderDeptEmail.actionType
);

export const suggestedPurchaseOrderValuesActionTypes = makeActionTypes(
    itemTypes.suggestedPurchaseOrderValues.actionType
);

export const bomTypeChangeActionTypes = makeActionTypes(itemTypes.bomTypeChange.actionType);

export const bomTreeActionTypes = makeActionTypes(itemTypes.bomTree.actionType);

export const boardActionTypes = makeActionTypes(itemTypes.board.actionType);
export const boardComponentsActionTypes = makeActionTypes(itemTypes.boardComponents.actionType);
export const boardsActionTypes = makeActionTypes(itemTypes.boards.actionType);

export const changeRequestStatusChangeActionTypes = makeActionTypes(
    itemTypes.changeRequestStatusChange.actionType
);

export const partsOnBomReportActionTypes = makeReportActionTypes(
    reportTypes.partsOnBomReport.actionType
);

export const bomCostReportActionTypes = makeReportActionTypes(reportTypes.bomCostReport.actionType);

export const subAssemblyActionTypes = makeActionTypes(itemTypes.subAssembly.actionType);

export const boardComponentSummariesActionTypes = makeActionTypes(
    itemTypes.boardComponentSummaries.actionType,
    false
);

export const copyBomActionTypes = makeProcessActionTypes(processTypes.copyBom.actionType);
