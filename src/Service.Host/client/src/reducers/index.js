import {
    reducers as sharedLibraryReducers,
    fetchErrorReducer
} from '@linn-it/linn-form-components-library';
import { connectRouter } from 'connected-react-router';
import { combineReducers } from 'redux';
import { reducer as oidc } from 'redux-oidc';
import historyStore from './history';
import * as itemTypes from '../itemTypes';
import * as reportTypes from '../reportTypes';
import partSupplier from './partSupplier';
import partSuppliers from './partSuppliers';
import signingLimit from './signingLimit';
import signingLimits from './signingLimits';
import employees from './employees';
import suppliers from './suppliers';
import parts from './parts';
import part from './part';
import currencies from './currencies';
import orderMethods from './orderMethods';
import deliveryAddresses from './deliveryAddresses';
import unitsOfMeasure from './unitsOfMeasure';
import tariffs from './tariffs';
import packagingGroups from './packagingGroups';
import manufacturers from './manufacturers';
import ordersBySupplier from './ordersBySupplierReport';
import preferredSupplierChange from './preferredSupplierChange';
import priceChangeReasons from './priceChangeReasons';
import partPriceConversions from './partPriceConversions';
import ordersByPart from './ordersByPartReport';
import vendorManagers from './vendorManagers';
import spendBySupplierReport from './spendBySupplierReport';
import spendBySupplierByDateRangeReport from './spendBySupplierByDateRangeReport';
import supplier from './supplier';
import partCategories from './partCategories';
import accountingCompanies from './accountingCompanies';
import putSupplierOnHold from './putSupplierOnHold';
import address from './address';
import addresses from './addresses';
import countries from './countries';
import suppliersWithUnacknowledgedOrders from './suppliersWithUnacknowledgedOrders';
import unacknowledgedOrdersReport from './unacknowledgedOrdersReport';
import planners from './planners';
import spendByPartReport from './spendByPartReport';
import plCreditDebitNote from './plCreditDebitNote';
import plCreditDebitNotes from './plCreditDebitNotes';
import openDebitNotes from './openDebitNotes';
import sendPlNoteEmail from './sendPlNoteEmail';
import bulkLeadTimesUpload from './bulkLeadTimesUpload';
import supplierGroups from './supplierGroups.';
import tqmsJobrefs from './tqmsJobrefs';
import partsReceivedReport from './partsReceivedReport';
import purchaseOrderReq from './purchaseOrderReq';
import purchaseOrderReqs from './purchaseOrderReqs';
import departments from './departments';
import nominals from './nominals';
import purchaseOrder from './purchaseOrder';
import purchaseOrders from './purchaseOrders';
import whatsDueInReport from './whatsDueInReport';
import purchaseOrderReqStates from './purchaseOrderReqStates';
import outstandingPoReqsReport from './outstandingPoReqsReport';
import sendEdiEmail from './sendEdiEmail';
import sendPurchaseOrderReqEmail from './sendPurchaseOrderReqEmail';
import sendPurchaseOrderReqAuthEmail from './sendPurchaseOrderReqAuthEmail';
import sendPurchaseOrderReqFinanceEmail from './sendPurchaseOrderReqFinanceEmail';
import prefSupReceiptsReport from './prefSupReceiptsReport';
import whatsInInspectionReport from './whatsInInspectionReport';
import mrpRunLog from './mrpRunLog';
import runMrp from './runMrp';
import mrMaster from './mrMaster';
import applyForecastingPercentageChange from './applyForecastingPercentageChange';
import ediOrders from './ediOrders';
import mrUsedOnReport from './mrUsedOnReport';
import purchaseOrderDelivery from './purchaseOrderDelivery';
import purchaseOrderDeliveries from './purchaseOrderDeliveries';
import batchPurchaseOrderDeliveriesUpload from './batchPurchaseOrderDeliveriesUpload';
import mrReport from './mrReport';
import mrReportOptions from './mrReportOptions';
import pOReqCheckIfCanAuthOrder from './pOReqCheckIfCanAuthOrder';
import mrReportOrders from './mrReportOrders';
import ediSuppliers from './ediSuppliers';
import shortagesReport from './shortagesReport';
import shortagesPlannerReport from './shortagesPlannerReport';
import mrOrderBookReport from './mrOrderBookReport';
import sendPurchaseOrderPdfEmail from './sendPurchaseOrderPdfEmail';
import automaticPurchaseOrder from './automaticPurchaseOrder';
import automaticPurchaseOrderSuggestions from './automaticPurchaseOrderSuggestions';
import supplierLeadTimesReport from './supplierLeadTimesReport';
import ledgerPeriods from './ledgerPeriods';
import deliveryPerformanceSummaryReport from './deliveryPerformanceSummaryReport';
import deliveryPerformanceSupplierReport from './deliveryPerformanceSupplierReport';
import deliveryPerformanceDetailReport from './deliveryPerformanceDetailReport';
import authoriseMultiplePurchaseOrders from './authoriseMultiplePurchaseOrders';
import emailMultiplePurchaseOrders from './emailMultiplePurchaseOrders';
import forecastWeekChangesReport from './forecastWeekChangesReport';
import sendPurchaseOrderAuthEmail from './sendPurchaseOrderAuthEmail';
import changeRequest from './changeRequest';
import bomTreeNodes from './bomTreeNodes';

const errors = fetchErrorReducer({ ...itemTypes, ...reportTypes });

const rootReducer = history =>
    combineReducers({
        oidc,
        historyStore,
        accountingCompanies,
        address,
        addresses,
        applyForecastingPercentageChange,
        authoriseMultiplePurchaseOrders,
        automaticPurchaseOrder,
        automaticPurchaseOrderSuggestions,
        batchPurchaseOrderDeliveriesUpload,
        bomTreeNodes,
        bulkLeadTimesUpload,
        changeRequest,
        countries,
        currencies,
        deliveryAddresses,
        deliveryPerformanceDetailReport,
        deliveryPerformanceSummaryReport,
        deliveryPerformanceSupplierReport,
        departments,
        ediOrders,
        ediSuppliers,
        emailMultiplePurchaseOrders,
        employees,
        forecastWeekChangesReport,
        ledgerPeriods,
        manufacturers,
        mrOrderBookReport,
        mrMaster,
        mrpRunLog,
        mrReport,
        mrReportOptions,
        mrReportOrders,
        mrUsedOnReport,
        nominals,
        openDebitNotes,
        orderMethods,
        ordersByPart,
        ordersBySupplier,
        outstandingPoReqsReport,
        packagingGroups,
        part,
        partCategories,
        partPriceConversions,
        parts,
        partsReceivedReport,
        partSupplier,
        partSuppliers,
        planners,
        plCreditDebitNote,
        plCreditDebitNotes,
        pOReqCheckIfCanAuthOrder,
        preferredSupplierChange,
        prefSupReceiptsReport,
        priceChangeReasons,
        purchaseOrder,
        purchaseOrderReq,
        purchaseOrderReqs,
        purchaseOrderReqStates,
        purchaseOrderDelivery,
        purchaseOrderDeliveries,
        purchaseOrders,
        putSupplierOnHold,
        router: connectRouter(history),
        runMrp,
        sendEdiEmail,
        sendPlNoteEmail,
        sendPurchaseOrderAuthEmail,
        sendPurchaseOrderPdfEmail,
        sendPurchaseOrderReqEmail,
        sendPurchaseOrderReqAuthEmail,
        sendPurchaseOrderReqFinanceEmail,
        shortagesReport,
        shortagesPlannerReport,
        signingLimit,
        signingLimits,
        spendBySupplierReport,
        spendBySupplierByDateRangeReport,
        spendByPartReport,
        supplier,
        supplierGroups,
        suppliers,
        supplierLeadTimesReport,
        suppliersWithUnacknowledgedOrders,
        tariffs,
        tqmsJobrefs,
        unacknowledgedOrdersReport,
        unitsOfMeasure,
        vendorManagers,
        whatsDueInReport,
        whatsInInspectionReport,
        ...sharedLibraryReducers,
        errors
    });

export default rootReducer;
