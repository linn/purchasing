const initialState = {};

export default function purchaseOrderReducer(state = initialState, action) {
    switch (action.type) {
        case 'initialise':
            return { ...action.payload };
        case 'orderFieldChange':
            return {
                ...state,
                [action.fieldName]: action.payload
            };
        case 'detailFieldChange':
            return {
                ...state,
                details: [
                    ...state.details.filter(x => x.lineNumber !== action.lineNumber),
                    action.payload
                ]
            };
        case 'nominalChange':
            return {
                ...state,
                details: [
                    ...state.details.map(detail => {
                        if (detail.line !== action.lineNumber) {
                            return detail;
                        }
                        console.log({ ...detail.orderPosting, nominalAccount: action.payload });
                        return {
                            ...detail,
                            orderPosting: {
                                ...detail.orderPosting,
                                nominalAccount: action.payload,
                                nominalAccountId: action.payload.accountId
                            }
                        };
                    })
                ]
            };
        default:
            return state;
    }
}
