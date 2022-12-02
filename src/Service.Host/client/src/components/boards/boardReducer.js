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

const revisionCodesAreUnique = (revisions, newRevisionCode) => {
    if (!revisions && !revisions.length) {
        return true;
    }

    const codes = revisions.map(a => a.revisionCode);
    if (newRevisionCode) {
        codes.push(newRevisionCode);
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
                const revisionType =
                    layout.layoutType === 'L'
                        ? { typeCode: 'PRODUCTION' }
                        : { typeCode: 'PROTOTYPE' };
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
                    versionNumber: lastVersionNumber + 1,
                    revisionType,
                    boardCode: state.board.boardCode
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
                if (action.fieldName === 'layoutCode') {
                    if (!layoutCodesAreUnique(state.board.layouts, action.payload)) {
                        return state;
                    }
                }

                const layoutToUpdate = state.board.layouts.find(
                    s => s.layoutCode === state.selectedLayout[0]
                );

                layoutToUpdate[action.fieldName] = action.payload;

                if (action.fieldName === 'layoutType' || action.fieldName === 'layoutNumber') {
                    if (layoutToUpdate.layoutType && layoutToUpdate.layoutNumber) {
                        if (
                            !layoutCodesAreUnique(
                                state.board.layouts,
                                `${layoutToUpdate.layoutType}${layoutToUpdate.layoutNumber}`
                            )
                        ) {
                            layoutToUpdate.layoutCode = `duplicate`;
                        } else {
                            layoutToUpdate.layoutCode = `${layoutToUpdate.layoutType}${layoutToUpdate.layoutNumber}`;
                        }
                    }
                }
                return {
                    ...state,
                    selectedLayout: [layoutToUpdate.layoutCode]
                };
            }

            return state;
        }
        case 'updateRevision': {
            if (state.selectedLayout?.length && state.selectedRevision?.length) {
                const layoutIndex = state.board.layouts.findIndex(
                    s => s.layoutCode === state.selectedLayout[0]
                );

                if (action.fieldName === 'revisionCode') {
                    if (
                        !revisionCodesAreUnique(
                            state.board.layouts[layoutIndex].revisions,
                            action.payload
                        )
                    ) {
                        return state;
                    }
                }

                const currentLayout = state.board.layouts[layoutIndex];
                const revisionToUpdate = currentLayout.revisions?.find(
                    a => a.revisionCode === state.selectedRevision[0]
                );
                revisionToUpdate[action.fieldName] = action.payload;

                if (action.fieldName === 'revisionNumber') {
                    if (
                        !revisionCodesAreUnique(
                            currentLayout.revisions,
                            `${revisionToUpdate.layoutCode}R${revisionToUpdate.revisionNumber}`
                        )
                    ) {
                        revisionToUpdate.revisionCode = `duplicate`;
                    } else {
                        revisionToUpdate.revisionCode = `${revisionToUpdate.layoutCode}R${revisionToUpdate.revisionNumber}`;
                    }
                }

                return {
                    ...state,
                    selectedRevision: [revisionToUpdate.revisionCode]
                };
            }

            return state;
        }
        default:
            return state;
    }
}
