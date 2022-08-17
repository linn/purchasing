import { Decimal } from 'decimal.js';
import currencyConvert from '../../helpers/currencyConvert';

const initialState = {};

const recalculateDetailFields = (detail, exchangeRate) => {
    const netTotalCurrency = new Decimal(detail.ourQty).mul(detail.ourUnitPriceCurrency);
    const detailTotalCurrency = new Decimal(netTotalCurrency).plus(detail.vatTotalCurrency);
    const baseNetTotal = currencyConvert(netTotalCurrency, exchangeRate);
    const baseDetailTotal = currencyConvert(detailTotalCurrency, exchangeRate);

    //vat amount not yet calculated, can do it if include the supplier vat % in what's sent to front end

    return {
        ...detail,
        netTotalCurrency: new Decimal(netTotalCurrency).toDecimalPlaces(2, Decimal.ROUND_HALF_UP),
        detailTotalCurrency: new Decimal(detailTotalCurrency).toDecimalPlaces(
            2,
            Decimal.ROUND_HALF_UP
        ),
        baseNetTotal,
        baseDetailTotal
    };
};

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
        case 'detailCalculationFieldChange':
            return {
                ...state,
                details: [
                    ...state.details.filter(x => x.lineNumber !== action.lineNumber),
                    recalculateDetailFields(action.payload, state.exchangeRate)
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
        case 'supplierChange':
            return {
                ...state,
                supplier: action.payload
            };
        default:
            return state;
    }
}
