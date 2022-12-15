const initialState = {
    selectedLayout: null,
    selectedRevision: null,
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
            let selectedLayout;
            let selectedRevision;
            if (action.payload.layouts && action.payload.layouts.length > 0) {
                selectedLayout = [
                    action.payload.layouts[action.payload.layouts.length - 1].layoutCode
                ];
                selectedRevision = getLastRevisionCodeArray(action.payload, selectedLayout[0]);
            } else {
                selectedLayout = [];
                selectedRevision = [];
            }

            const layouts = action.payload.layouts ? [...action.payload.layouts] : [];
            return {
                ...state,
                board: {
                    ...action.payload,
                    layouts
                },
                selectedLayout,
                selectedRevision
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
                selectedLayout: action.payload,
                selectedRevision: getLastRevisionCodeArray(state.board, action.payload[0])
            };
        }
        case 'setSelectedRevision': {
            return {
                ...state,
                selectedRevision: action.payload
            };
        }
        case 'setSelectedComponent': {
            return {
                ...state,
                selectedComponent: action.payload
            };
        }
        default:
            return state;
    }
}
