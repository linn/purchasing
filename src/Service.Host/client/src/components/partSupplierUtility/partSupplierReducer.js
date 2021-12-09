const initialState = { partSupplier: {} };

export default function partReducer(state = initialState, action) {
    switch (action.type) {
        case 'initialise':
            return { ...state, partSupplier: action.payload, prevPartSupplier: action.payload };
        case 'fieldChange':
            return {
                ...state,
                partSupplier: { ...state.partSupplier, [action.fieldName]: action.payload }
            };
        default:
            return state;
    }
}
