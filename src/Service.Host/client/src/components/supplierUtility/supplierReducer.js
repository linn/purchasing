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
                    supplierContacts: state.supplier.supplierContacts.map(c =>
                        c.id === action.payload.id
                            ? { ...c, [action.propertyName]: action.payload.newValue }
                            : c
                    )
                }
            };
        case 'updateContactDetail':
            return {
                ...state,
                supplier: {
                    ...state.supplier,
                    supplierContacts: state.supplier.supplierContacts.map(c =>
                        c.id === action.payload.id
                            ? {
                                  ...c,
                                  [action.propertyName]: action.payload.newValue
                              }
                            : c
                    )
                }
            };
        case 'addContact':
            return {
                ...state,
                supplier: {
                    ...state.supplier,
                    supplierContacts: state.supplier?.supplierContacts
                        ? [...state.supplier?.supplierContacts, action.payload]
                        : [action.payload]
                }
            };
        default:
            return state;
    }
}
