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
        case 'updateContact':
            return {
                ...state,
                supplier: {
                    ...state.supplier,
                    supplierContacts: state.supplier.supplierContacts.map(c =>
                        c.id === action.payload.id ? action.payload : c
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
        case 'deleteContacts':
            return {
                ...state,
                supplier: {
                    ...state.supplier,
                    supplierContacts: state.supplier?.supplierContacts.filter(
                        x => !action.payload.includes(x.id)
                    )
                }
            };
        default:
            return state;
    }
}
