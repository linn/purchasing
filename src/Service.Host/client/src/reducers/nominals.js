import { collectionStoreFactory } from '@linn-it/linn-form-components-library';
import { nominalsActionTypes as actionTypes } from '../actions';
import * as itemTypes from '../itemTypes';

const defaultState = {
    loading: false,
    items: []
};

export default collectionStoreFactory(itemTypes.nominals.actionType, actionTypes, defaultState);
