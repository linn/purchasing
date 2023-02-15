import * as actionTypes from '../actions';
import partDataSheetValuesActions from '../actions/partDataSheetValuesActions';

export default ({ dispatch }) =>
    next =>
    action => {
        const result = next(action);

        switch (action.type) {
            case actionTypes.partDataSheetValuesActionTypes.RECEIVE_UPDATED_PART_DATA_SHEET_VALUES:
            case actionTypes.signingLimitActionTypes.RECEIVE_NEW_PART_DATA_SHEET_VALUES:
                dispatch(partDataSheetValuesActions.fetch());
                break;
            default:
        }

        return result;
    };
