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

const errors = fetchErrorReducer({ ...itemTypes });

const rootReducer = history =>
    combineReducers({
        oidc,
        historyStore,
        currencies,
        parts,
        partSupplier,
        partSuppliers,
        router: connectRouter(history),
        employees,
        signingLimit,
        signingLimits,
        suppliers,
        ...sharedLibraryReducers,
        errors
    });

export default rootReducer;
