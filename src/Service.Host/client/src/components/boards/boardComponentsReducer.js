const initialState = {
    layoutSelectionModel: [],
    selectedLayout: null,
    revisionSelectionModel: [],
    selectedRevision: null,
    componentSelectionModel: [],
    board: { coreBoard: 'N', clusterBoard: 'N', idBoard: 'N', splitBom: 'N', layouts: [] }
};

const getLastRevision = (board, layoutCode) => {
    const layout = board.layouts?.find(a => a.layoutCode === layoutCode);
    if (layout?.revisions && layout.revisions.length) {
        return layout.revisions[layout.revisions.length - 1];
    }

    return null;
};

export default function boardComponentsReducer(state = initialState, action) {
    switch (action.type) {
        case 'initialise':
            return initialState;
        case 'populate': {
            let layoutSelectionModel;
            let selectedLayout;
            let revisionSelectionModel;
            let selectedRevision;
            if (action.payload.layouts && action.payload.layouts.length > 0) {
                selectedLayout = action.payload.layouts[action.payload.layouts.length - 1];
                layoutSelectionModel = [selectedLayout.layoutCode];
                selectedRevision = getLastRevision(action.payload, selectedLayout.layoutCode);
                revisionSelectionModel = selectedRevision ? [selectedRevision.revisionCode] : [];
            } else {
                layoutSelectionModel = [];
                selectedLayout = null;
                revisionSelectionModel = [];
                selectedRevision = null;
            }

            const layouts = action.payload.layouts ? [...action.payload.layouts] : [];
            const components = action.payload.components ? [...action.payload.components] : [];
            return {
                ...state,
                board: {
                    ...action.payload,
                    layouts,
                    components
                },
                layoutSelectionModel,
                selectedLayout,
                revisionSelectionModel,
                selectedRevision,
                componentSelectionModel: []
            };
        }
        case 'fieldChange':
            return {
                ...state,
                board: { ...state.board, [action.fieldName]: action.payload }
            };
        case 'setSelectedLayout': {
            let selectedLayout;
            if (action.payload?.length && state.board.layouts?.length) {
                selectedLayout = state.board.layouts.find(a => a.layoutCode === action.payload[0]);
            } else {
                return state;
            }

            const lastRevisionForLayout = getLastRevision(state.board, selectedLayout.layoutCode);

            return {
                ...state,
                layoutSelectionModel: action.payload,
                selectedLayout,
                selectedRevision: lastRevisionForLayout,
                revisionSelectionModel: lastRevisionForLayout
                    ? [lastRevisionForLayout.revisionCode]
                    : [],
                componentSelectionModel: []
            };
        }
        case 'setSelectedRevision': {
            if (!action.payload?.length || !state.selectedLayout) {
                return state;
            }

            return {
                ...state,
                revisionSelectionModel: action.payload,
                selectedRevision: state.selectedLayout.revisions.find(
                    a => a.revisionCode === action.payload[0]
                ),
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
