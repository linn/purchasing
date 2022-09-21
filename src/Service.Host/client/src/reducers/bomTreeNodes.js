import { bomTreeNodesActionTypes } from '../actions';

export default (state = { loading: false, items: {} }, action) => {
    switch (action.type) {
        case bomTreeNodesActionTypes.REQUEST_BOM_TREE_NODES:
            return {
                ...state,
                items: state.items,
                loading: true
            };
        case bomTreeNodesActionTypes.RECEIVE_BOM_TREE_NODES:
            return {
                ...state,
                loading: false,
                items: {
                    ...state.items,
                    [action.payload.data.bomId]: {
                        bomId: action.payload.data.bomId,
                        bomName: action.payload.data.bomName,
                        children: action.payload.data.details
                    }
                }
            };
        default:
            return state;
    }
};
