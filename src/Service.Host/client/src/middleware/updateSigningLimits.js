import signingLimitsActions from '../actions/signingLimitsActions';
import * as actionTypes from '../actions';

export default ({ dispatch }) =>
    next =>
    action => {
        const result = next(action);

        switch (action.type) {
            case actionTypes.signingLimitActionTypes.RECEIVE_UPDATED_SIGNING_LIMIT:
            case actionTypes.signingLimitActionTypes.RECEIVE_NEW_SIGNING_LIMIT:
                dispatch(signingLimitsActions.fetch());
                break;
            default:
        }

        return result;
    };
