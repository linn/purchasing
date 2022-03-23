import { collectionStoreFactory } from '@linn-it/linn-form-components-library';
import { purchaseOrderReqsActionTypes as actionTypes } from '../actions';
import * as itemTypes from '../itemTypes';

const defaultState = {
    loading: false,
    item: null,
    editStatus: 'view'
};

export default collectionStoreFactory(
    itemTypes.purchaseOrderReqs.actionType,
    actionTypes,
    defaultState
);
