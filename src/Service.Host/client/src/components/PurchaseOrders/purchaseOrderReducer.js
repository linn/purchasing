const initialState = {};

export default function purchaseOrderReducer(state = initialState, action) {
    switch (action.type) {
        case 'initialise':
            return { ...action.payload };
        case 'orderFieldChange':
            return {
                ...state,
                [action.propertyName]: action.payload
            };
        case 'detailFieldChange':
            return {
                ...state,
                details: [
                    ...state.details.filter(x => x.lineNumber !== action.lineNumber),
                    action.payload
                ]
            };
        case 'deliveryFieldChange':
            return {
                ...state,
                details: [
                    ...state.details.map(detail => {
                        if (detail.line !== action.payload.orderLine) {
                            return detail;
                        }
                        return {
                            ...detail,
                            purchaseDeliveries: [
                                ...detail.purchaseDeliveries.filter(
                                    x => x.deliverySeq !== action.payload.deliverySeq
                                ),
                                action.payload
                            ]
                        };
                    })
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
