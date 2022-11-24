const initialState = {
    selectedLayout: null,
    board: { coreBoard: 'N', clusterBoard: 'N', idBoard: 'N', splitBom: 'N', layouts: [] }
};

export default function boardReducer(state = initialState, action) {
    switch (action.type) {
        case 'initialise':
            return initialState;
        case 'populate': {
            let selectedLayout;
            if (action.payload.layouts && action.payload.layouts.length > 0) {
                selectedLayout = [
                    action.payload.layouts[action.payload.layouts.length - 1].layoutCode
                ];
            } else {
                selectedLayout = [];
            }

            const layouts = action.payload.layouts ? [...action.payload.layouts] : [];
            return {
                ...state,
                board: {
                    ...action.payload,
                    layouts
                },
                selectedLayout
            };
        }
        case 'fieldChange':
            return {
                ...state,
                board: { ...state.board, [action.fieldName]: action.payload }
            };
        case 'newLayout': {
            const newLayout = { layoutType: 'L', creating: true, layoutCode: 'new' };
            return {
                ...state,
                board: { ...state.board, layouts: [...state.board.layouts, newLayout] },
                selectedLayout: ['new']
            };
        }
        case 'setSelectedLayout': {
            return {
                ...state,
                selectedLayout: action.payload
            };
        }
        case 'updateLayout': {
            if (state.selectedLayout.length) {
                const index = state.board.layouts.findIndex(
                    s => s.layoutCode === state.selectedLayout[0]
                );
                let layout = state.board.layouts.splice(index, 1)[0];
                layout = { ...layout, [action.fieldName]: action.payload };

                return {
                    ...state,
                    board: { ...state.board, layouts: [...state.board.layouts, layout] },
                    selectedLayout: [layout.layoutCode]
                };
            }

            return state;
        }
        default:
            return state;
    }
}
