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
import supplier from './supplier';
import partCategories from './partCategories';

const errors = fetchErrorReducer({ ...itemTypes, ...reportTypes });

const rootReducer = history =>
    combineReducers({
        oidc,
        historyStore,
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
        preferredSupplierChange,
        priceChangeReasons,
        router: connectRouter(history),
        signingLimit,
        signingLimits,
        supplier,
        suppliers,
        tariffs,
        unitsOfMeasure,
        ...sharedLibraryReducers,
        errors
    });

export default rootReducer;
