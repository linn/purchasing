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

const errors = fetchErrorReducer({ ...itemTypes, ...reportTypes });

const rootReducer = history =>
    combineReducers({
        oidc,
        historyStore,
        accountingCompanies,
        address,
        addresses,
        countries,
        currencies,
        deliveryAddresses,
        employees,
        manufacturers,
        orderMethods,
        ordersByPart,
        ordersBySupplier,
        packagingGroups,
        part,
        partCategories,
        partPriceConversions,
        parts,
        partSupplier,
        partSuppliers,
        planners,
        preferredSupplierChange,
        priceChangeReasons,
        putSupplierOnHold,
        router: connectRouter(history),
        signingLimit,
        signingLimits,
        spendBySupplierReport,
        spendByPartReport,
        supplier,
        suppliers,
        suppliersWithUnacknowledgedOrders,
        tariffs,
        unacknowledgedOrdersReport,
        unitsOfMeasure,
        vendorManagers,
        ...sharedLibraryReducers,
        errors
    });

export default rootReducer;
