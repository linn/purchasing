import * as actionTypes from '../actions';
import partDataSheetValuesListActions from '../actions/partDataSheetValuesListActions';

export default ({ dispatch }) =>
    next =>
    action => {
        const result = next(action);

        switch (action.type) {
            case actionTypes.partDataSheetValuesActionTypes.RECEIVE_UPDATED_PART_DATA_SHEET_VALUES:
            case actionTypes.signingLimitActionTypes.RECEIVE_NEW_PART_DATA_SHEET_VALUES:
                dispatch(partDataSheetValuesListActions.fetch());
                break;
            default:
        }

        return result;
    };
