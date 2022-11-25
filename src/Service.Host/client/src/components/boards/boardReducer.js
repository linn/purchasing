const initialState = {
    selectedLayout: null,
    board: { coreBoard: 'N', clusterBoard: 'N', idBoard: 'N', splitBom: 'N', layouts: [] }
};

const layoutCodesAreUnique = (layouts, newLayoutCode) => {
    if (!layouts && !layouts.length) {
        return true;
    }

    const codes = layouts.map(a => a.layoutCode);
    if (newLayoutCode) {
        codes.push(newLayoutCode);
    }

    return codes.every((a, i) => codes.indexOf(a) === i);
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
            let lastType;
            let lastLayoutNumber;
            let lastLayoutSequence;
            if (state.selectedLayout?.length) {
                const { layouts } = state.board;

                lastType = layouts.find(a => a.layoutCode === state.selectedLayout[0]).layoutType;
                lastLayoutNumber = Math.max(
                    ...layouts.filter(a => a.layoutType === lastType).map(o => o.layoutNumber)
                );
                lastLayoutSequence = Math.max(...layouts.map(o => o.layoutSequence));
            } else {
                lastType = 'P';
                lastLayoutNumber = 0;
                lastLayoutSequence = 0;
            }

            const newLayoutCode = `${lastType}${lastLayoutNumber + 1}`;
            const newLayout = {
                layoutType: lastType,
                creating: true,
                layoutCode: newLayoutCode,
                layoutNumber: lastLayoutNumber + 1,
                layoutSequence: lastLayoutSequence + 1
            };
            return {
                ...state,
                board: { ...state.board, layouts: [...state.board.layouts, newLayout] },
                selectedLayout: [newLayoutCode]
            };
        }
        case 'setSelectedLayout': {
            return {
                ...state,
                selectedLayout: action.payload
            };
        }
        case 'updateLayout': {
            if (state.selectedLayout?.length) {
                const index = state.board.layouts.findIndex(
                    s => s.layoutCode === state.selectedLayout[0]
                );

                if (action.fieldName === 'layoutCode') {
                    if (!layoutCodesAreUnique(state.board.layouts, action.payload)) {
                        return state;
                    }
                }

                let layout = state.board.layouts.splice(index, 1)[0];
                layout = { ...layout, [action.fieldName]: action.payload };

                if (action.fieldName === 'layoutType' || action.fieldName === 'layoutNumber') {
                    if (layout.layoutType && layout.layoutNumber) {
                        if (
                            !layoutCodesAreUnique(
                                state.board.layouts,
                                `${layout.layoutType}${layout.layoutNumber}`
                            )
                        ) {
                            layout = {
                                ...layout,
                                layoutCode: `duplicate`
                            };
                        } else {
                            layout = {
                                ...layout,
                                layoutCode: `${layout.layoutType}${layout.layoutNumber}`
                            };
                        }
                    }
                }
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
