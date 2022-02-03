const initialState = { supplier: {} };

export default function partReducer(state = initialState, action) {
    switch (action.type) {
        case 'initialise':
            return { ...state, supplier: action.payload, prevPartSupplier: action.payload };
        case 'fieldChange':
            return {
                ...state,
                supplier: { ...state.supplier, [action.fieldName]: action.payload }
            };
        default:
            return state;
    }
}
