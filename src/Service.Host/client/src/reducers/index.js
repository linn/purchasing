import {
    reducers as sharedLibraryReducers,
    fetchErrorReducer
} from '@linn-it/linn-form-components-library';
import { connectRouter } from 'connected-react-router';
import { combineReducers } from 'redux';
import { reducer as oidc } from 'redux-oidc';
import historyStore from './history';
import * as itemTypes from '../itemTypes';
import partSupplier from './partSupplier';
import partSuppliers from './partSuppliers';
import signingLimit from './signingLimit';
import signingLimits from './signingLimits';
import employees from './employees';
import suppliers from './suppliers';
import parts from './parts';
import currencies from './currencies';
import orderMethods from './orderMethods';
import deliveryAddresses from './deliveryAddresses';
import unitsOfMeasure from './unitsOfMeasure';

const errors = fetchErrorReducer({ ...itemTypes });

const rootReducer = history =>
    combineReducers({
        oidc,
        historyStore,
        currencies,
        deliveryAddresses,
        orderMethods,
        parts,
        partSupplier,
        partSuppliers,
        router: connectRouter(history),
        employees,
        signingLimit,
        signingLimits,
        suppliers,
        unitsOfMeasure,
        ...sharedLibraryReducers,
        errors
    });

export default rootReducer;
