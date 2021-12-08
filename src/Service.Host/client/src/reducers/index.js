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

const errors = fetchErrorReducer({ ...itemTypes });

const rootReducer = history =>
    combineReducers({
        oidc,
        historyStore,
        partSupplier,
        partSuppliers,
        router: connectRouter(history),
        signingLimit,
        signingLimits,
        ...sharedLibraryReducers,
        errors
    });

export default rootReducer;
