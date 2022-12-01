import { utilities } from '@linn-it/linn-form-components-library';

const initialState = {
    selectedLayout: null,
    selectedRevision: null,
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

const getLastRevisionCodeArray = (board, layoutCode) => {
    const layout = board.layouts?.find(a => a.layoutCode === layoutCode);
    if (layout?.revisions && layout.revisions.length) {
        return [layout.revisions[layout.revisions.length - 1].revisionCode];
    }

    return [];
};

export default function boardReducer(state = initialState, action) {
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
        case 'newRevision': {
            if (state.selectedLayout?.length && state.board.layouts) {
                let lastRevisionNumber;
                let lastVersionNumber;

                const layout = state.board.layouts.find(
                    a => a.layoutCode === state.selectedLayout[0]
                );
                if (layout.revisions && layout.revisions.length) {
                    lastRevisionNumber = Math.max(...layout.revisions.map(o => o.revisionNumber));
                    lastVersionNumber = Math.max(...layout.revisions.map(o => o.versionNumber));
                } else {
                    lastRevisionNumber = 0;
                    lastVersionNumber = 0;
                }

                const newRevisionCode = `${state.selectedLayout[0]}R${lastRevisionNumber + 1}`;
                const newRevision = {
                    layoutCode: state.selectedLayout[0],
                    creating: true,
                    splitBom: 'N',
                    revisionCode: newRevisionCode,
                    revisionNumber: lastRevisionNumber + 1,
                    versionNumber: lastVersionNumber + 1
                };

                const index = state.board.layouts.findIndex(
                    s => s.layoutCode === state.selectedLayout[0]
                );

                let updatedLayout = state.board.layouts.splice(index, 1)[0];
                updatedLayout = {
                    ...updatedLayout,
                    revisions: [...updatedLayout.revisions, newRevision]
                };
                return {
                    ...state,
                    board: {
                        ...state.board,
                        layouts: utilities.sortEntityList(
                            [...state.board.layouts, updatedLayout],
                            'revisionNumber'
                        )
                    },
                    selectedRevision: [newRevisionCode]
                };
            }

            return state;
        }
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
