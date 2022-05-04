const initialState = { supplier: {} };

export default function partReducer(state = initialState, action) {
    const isMainOrderContact = state.supplier?.supplierContacts?.some(
        x => x.isMainOrderContact === 'Y'
    )
        ? 'N'
        : 'Y';
    const isMainInvoiceContact = state.supplier?.supplierContacts?.some(
        x => x.isMainInvoiceContact === 'Y'
    )
        ? 'N'
        : 'Y';
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
                        ? [
                              ...state.supplier?.supplierContacts,
                              {
                                  ...action.payload,
                                  isMainOrderContact,
                                  isMainInvoiceContact
                              }
                          ]
                        : [{ ...action.payload, isMainOrderContact, isMainInvoiceContact }]
                }
            };
        default:
            return state;
    }
}
