const initialState = { supplier: {} };

export default function partReducer(state = initialState, action) {
    switch (action.type) {
        case 'initialise':
            return { ...state, supplier: action.payload };
        case 'fieldChange':
            return {
                ...state,
                supplier: { ...state.supplier, [action.fieldName]: action.payload }
            };
        case 'updateMainContact':
            return {
                ...state,
                supplier: {
                    ...state.supplier,
                    contacts: state.supplier.contacts.map(c =>
                        c.contact.contactId === action.payload.id
                            ? { ...c, [action.propertyName]: action.payload.newValue }
                            : c
                    )
                }
            };
        default:
            return state;
    }
}
