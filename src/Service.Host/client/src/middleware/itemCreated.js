import { utilities } from '@linn-it/linn-form-components-library';
import history from '../history';
import * as actionTypes from '../actions/index';

export default () => next => action => {
    const result = next(action);

    if (action.type.startsWith('RECEIVE_NEW_')) {
        if (
            action.type !== actionTypes.signingLimitActionTypes.RECEIVE_NEW_SIGNING_LIMIT &&
            action.type !==
                actionTypes.suggestedPurchaseOrderValuesActionTypes
                    .RECEIVE_NEW_SUGGESTED_PURCHASE_ORDER_VALUES &&
            action.type !==
                actionTypes.preferredSupplierChangeActionTypes
                    .RECEIVE_NEW_PREFERRED_SUPPLIER_CHANGE &&
            action.type !== actionTypes.addressActionTypes.RECEIVE_NEW_ADDRESS &&
            action.type !== actionTypes.addressActionTypes.RECEIVE_NEW_BOM_TREE
        ) {
            history.push(utilities.getSelfHref(action.payload.data));
        }
    }

    return result;
};
