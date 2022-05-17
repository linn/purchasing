import { collectionStoreFactory } from '@linn-it/linn-form-components-library';
import { sendEdiEmailActionTypes as actionTypes } from '../actions';
import * as itemTypes from '../itemTypes';

const defaultState = {
    success: null,
    message: ''
};

export default collectionStoreFactory(itemTypes.sendEdiEmail.actionType, actionTypes, defaultState);
