import { itemStoreFactory } from '@linn-it/linn-form-components-library';
import { vendorManagerActionTypes as actionTypes } from '../actions';
import * as itemTypes from '../itemTypes';

const defaultState = {
    loading: false,
    items: []
};

export default itemStoreFactory(itemTypes.vendorManager.actionType, actionTypes, defaultState);
