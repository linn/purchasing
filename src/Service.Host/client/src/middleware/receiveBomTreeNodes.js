import * as actionTypes from '../actions/index';
import actions from '../actions/bomTreeNodeActions';

export default ({ dispatch }) =>
    next =>
    action => {
        const result = next(action);
        if (action.type === actionTypes.bomTreeNodesActionTypes.RECEIVE_BOM_TREE_NODES) {
            action.payload.data.details.forEach(detail => {
                if (detail.bomId) {
                    dispatch(actions.fetch(detail.bomId));
                }
            });
        }

        return result;
    };
