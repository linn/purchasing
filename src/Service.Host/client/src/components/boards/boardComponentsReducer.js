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
        case 'setSelectedRevisionToCrf': {
            if (!action.payload) {
                return state;
            }

            const layoutCodeForCrf = action.payload.substr(0, action.payload.indexOf('R'));
            const selectedLayoutForCrf = state.board.layouts.find(
                a => a.layoutCode === layoutCodeForCrf
            );

            return {
                ...state,
                layoutSelectionModel: [selectedLayoutForCrf.layoutCode],
                selectedLayout: selectedLayoutForCrf,
                revisionSelectionModel: [action.payload],
                selectedRevision: selectedLayoutForCrf.revisions.find(
                    a => a.revisionCode === action.payload
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
        case 'updateComponent': {
            if (!action.payload?.boardLine) {
                return state;
            }

            const { components } = state.board;

            const componentIndex = components.findIndex(
                i => i.boardLine === action.payload.boardLine
            );

            const componentToUpdate = action.payload;
            componentToUpdate.cRef = componentToUpdate.cRef
                ? componentToUpdate.cRef.toUpperCase()
                : null;
            components[componentIndex] = componentToUpdate;

            return {
                ...state
            };
        }
        case 'deleteProposedComponent': {
            if (!action.payload?.component?.boardLine) {
                return state;
            }

            const { components } = state.board;

            const componentIndexToMarkForRemove = components.findIndex(
                i => i.boardLine === action.payload.component.boardLine
            );

            const componentToUpdate = { ...components[componentIndexToMarkForRemove] };

            componentToUpdate.removing = true;
            componentToUpdate.deleteChangeDocumentNumber = null;
            components[componentIndexToMarkForRemove] = componentToUpdate;

            return {
                ...state
            };
        }
        case 'deleteComponent': {
            if (!action.payload?.component?.boardLine) {
                return state;
            }

            const { components } = state.board;

            const componentIndexToMarkForRemove = components.findIndex(
                i => i.boardLine === action.payload.component.boardLine
            );

            const componentToUpdate = { ...components[componentIndexToMarkForRemove] };

            componentToUpdate.removing = true;
            componentToUpdate.deleteChangeDocumentNumber = action.payload.crfNumber;
            components[componentIndexToMarkForRemove] = componentToUpdate;

            return {
                ...state
            };
        }
        case 'newComponent': {
            const { components } = state.board;
            if (!components) {
                return state;
            }

            const lastLine = Math.max(...components.map(o => o.boardLine));
            components.push({
                adding: true,
                changeState: 'PROPOS',
                boardCode: state.board.boardCode,
                boardLine: lastLine + 1,
                fromLayoutVersion: state.selectedRevision.layoutSequence,
                fromRevisionNumber: state.selectedRevision.versionNumber,
                quantity: action.payload.component?.quantity ?? 1,
                addChangeDocumentNumber: action.payload.crfNumber,
                partNumber: action.payload.component?.partNumber ?? null,
                cRef: action.payload.component?.cRef ?? null
            });

            return {
                ...state
            };
        }
        case 'replaceProposedComponent': {
            if (!action.payload?.component?.boardLine) {
                return state;
            }

            const { components } = state.board;
            if (!components) {
                return state;
            }

            const componentIndexToMarkForRemove = components.findIndex(
                i => i.boardLine === action.payload.component.boardLine
            );

            const componentToUpdate = { ...components[componentIndexToMarkForRemove] };

            componentToUpdate.removing = true;
            componentToUpdate.deleteChangeDocumentNumber = null;
            components[componentIndexToMarkForRemove] = componentToUpdate;

            const lastLine = Math.max(...components.map(o => o.boardLine));
            components.push({
                adding: true,
                changeState: 'PROPOS',
                boardCode: state.board.boardCode,
                boardLine: lastLine + 1,
                fromLayoutVersion: state.selectedRevision.layoutSequence,
                fromRevisionNumber: state.selectedRevision.versionNumber,
                quantity: action.payload.component?.quantity ?? 1,
                addChangeDocumentNumber: action.payload.crfNumber,
                partNumber: action.payload.component?.partNumber ?? null,
                cRef: action.payload.component?.cRef ?? null
            });

            return {
                ...state
            };
        }
        case 'replaceComponent': {
            if (!action.payload?.component?.boardLine) {
                return state;
            }

            const { components } = state.board;
            if (!components) {
                return state;
            }

            const componentIndexToMarkForRemove = components.findIndex(
                i => i.boardLine === action.payload.component.boardLine
            );

            const componentToUpdate = { ...components[componentIndexToMarkForRemove] };

            componentToUpdate.removing = true;
            componentToUpdate.deleteChangeDocumentNumber = action.payload.crfNumber;
            components[componentIndexToMarkForRemove] = componentToUpdate;

            const lastLine = Math.max(...components.map(o => o.boardLine));
            components.push({
                adding: true,
                changeState: 'PROPOS',
                boardCode: state.board.boardCode,
                boardLine: lastLine + 1,
                fromLayoutVersion: state.selectedRevision.layoutSequence,
                fromRevisionNumber: state.selectedRevision.versionNumber,
                quantity: action.payload.component?.quantity ?? 1,
                addChangeDocumentNumber: action.payload.crfNumber,
                partNumber: action.payload.component?.partNumber ?? null,
                cRef: action.payload.component?.cRef ?? null
            });

            return {
                ...state
            };
        }
        case 'setComponentPart': {
            if (!action.payload?.boardLine) {
                return state;
            }

            const { components } = state.board;

            const componentIndex = components.findIndex(
                i => i.boardLine === action.payload.boardLine
            );

            let componentToUpdate = components[componentIndex];

            componentToUpdate = {
                ...componentToUpdate,
                partNumber: action.payload.part?.partNumber
            };

            components[componentIndex] = componentToUpdate;

            return {
                ...state
            };
        }
        default:
            return state;
    }
}
