const initialState = {
    layoutSelectionModel: [],
    revisionSelectionModel: [],
    componentSelectionModel: [],
    board: { coreBoard: 'N', clusterBoard: 'N', idBoard: 'N', splitBom: 'N', layouts: [] }
};

const getLastRevisionCodeArray = (board, layoutCode) => {
    const layout = board.layouts?.find(a => a.layoutCode === layoutCode);
    if (layout?.revisions && layout.revisions.length) {
        return [layout.revisions[layout.revisions.length - 1].revisionCode];
    }

    return [];
};

export default function boardComponentsReducer(state = initialState, action) {
    switch (action.type) {
        case 'initialise':
            return initialState;
        case 'populate': {
            let layoutSelectionModel;
            let revisionSelectionModel;
            if (action.payload.layouts && action.payload.layouts.length > 0) {
                layoutSelectionModel = [
                    action.payload.layouts[action.payload.layouts.length - 1].layoutCode
                ];
                revisionSelectionModel = getLastRevisionCodeArray(
                    action.payload,
                    layoutSelectionModel[0]
                );
            } else {
                layoutSelectionModel = [];
                revisionSelectionModel = [];
            }

            const layouts = action.payload.layouts ? [...action.payload.layouts] : [];
            return {
                ...state,
                board: {
                    ...action.payload,
                    layouts
                },
                layoutSelectionModel,
                revisionSelectionModel
            };
        }
        case 'fieldChange':
            return {
                ...state,
                board: { ...state.board, [action.fieldName]: action.payload }
            };
        case 'setSelectedLayout': {
            return {
                ...state,
                layoutSelectionModel: action.payload,
                revisionSelectionModel: getLastRevisionCodeArray(state.board, action.payload[0]),
                componentSelectionModel: []
            };
        }
        case 'setSelectedRevision': {
            return {
                ...state,
                revisionSelectionModel: action.payload,
                componentSelectionModel: []
            };
        }
        case 'setSelectedComponent': {
            return {
                ...state,
                componentSelectionModel: action.payload
            };
        }
        default:
            return state;
    }
}
