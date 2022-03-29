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

const errors = fetchErrorReducer({ ...itemTypes, ...reportTypes });

const rootReducer = history =>
    combineReducers({
        oidc,
        historyStore,
        accountingCompanies,
        address,
        addresses,
        bulkLeadTimesUpload,
        countries,
        currencies,
        deliveryAddresses,
        departments,
        employees,
        manufacturers,
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
        partSupplier,
        partSuppliers,
        planners,
        plCreditDebitNote,
        plCreditDebitNotes,
        partsReceivedReport,
        preferredSupplierChange,
        priceChangeReasons,
        purchaseOrder,
        purchaseOrderReq,
        purchaseOrderReqs,
        purchaseOrderReqStates,
        purchaseOrders,
        putSupplierOnHold,
        router: connectRouter(history),
        sendPlNoteEmail,
        signingLimit,
        signingLimits,
        spendBySupplierReport,
        spendByPartReport,
        supplier,
        supplierGroups,
        suppliers,
        suppliersWithUnacknowledgedOrders,
        tariffs,
        tqmsJobrefs,
        unacknowledgedOrdersReport,
        unitsOfMeasure,
        vendorManagers,
        whatsDueInReport,
        ...sharedLibraryReducers,
        errors
    });

export default rootReducer;
