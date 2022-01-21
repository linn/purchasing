import { utilities } from '@linn-it/linn-form-components-library';
import history from '../history';
import * as actionTypes from '../actions/index';

export default () => next => action => {
    const result = next(action);

    if (action.type.startsWith('RECEIVE_NEW_')) {
        if (
            action.type !==
                actionTypes.RECEIVE_NEW_PREFERRED_SUPPLIER_CHANGERECEIVE_NEW_SIGNING_LIMIT ||
            action.type !== actionTypes.RECEIVE_NEW_PREFERRED_SUPPLIER_CHANGE
        ) {
            history.push(utilities.getSelfHref(action.payload.data));
        }
    }

    return result;
};
