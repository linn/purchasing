import supplierActions from '../actions/supplierActions';
import * as actionTypes from '../actions/index';

export default ({ dispatch }) =>
    next =>
    action => {
        const result = next(action);

        if (
            action.type ===
            actionTypes.putSupplierOnHoldActionTypes.RECEIVE_NEW_PUT_SUPPLIER_ON_HOLD
        ) {
            dispatch(supplierActions.fetch(action.payload.data.id));
        }

        return result;
    };
