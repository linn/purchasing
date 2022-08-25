import * as actionTypes from '../actions/index';
import purchaseOrderActions from '../actions/purchaseOrderActions';

export default ({ dispatch }) =>
    next =>
    action => {
        const result = next(action);
        if (
            action.type ===
            actionTypes.purchaseOrderDeliveriesActionTypes.RECEIVE_POST_PURCHASE_ORDER_DELIVERIES
        ) {
            dispatch(purchaseOrderActions.fetch(action.payload.data[0].orderNumber));
        }

        return result;
    };
