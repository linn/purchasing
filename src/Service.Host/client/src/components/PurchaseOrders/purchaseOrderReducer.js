import { Decimal } from 'decimal.js';
import currencyConvert from '../../helpers/currencyConvert';

const initialState = {};

const recalculateDetailFields = (detail, exchangeRate) => {
    const netTotalCurrency = new Decimal(detail.ourQty ?? 0).mul(detail.ourUnitPriceCurrency);
    const detailTotalCurrency = new Decimal(netTotalCurrency).plus(detail.vatTotalCurrency);
    const baseNetTotal = exchangeRate
        ? currencyConvert(netTotalCurrency, exchangeRate)
        : netTotalCurrency;
    const baseDetailTotal = exchangeRate
        ? currencyConvert(detailTotalCurrency, exchangeRate)
        : detailTotalCurrency;

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
            if (action.propertyName === 'requestedBy') {
                return {
                    ...state,
                    requestedBy: { id: action.payload || null }
                };
            }
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
        case 'detailFieldUpdate':
            return {
                ...state,
                details: [
                    ...state.details.map(x => {
                        if (x.lineNumber !== action.lineNumber) {
                            return x;
                        }

                        return { ...x, [action.payload.fieldName]: action.payload.value };
                    })
                ]
            };
        case 'currencyChange':
            return {
                ...state,
                currency: action.newCurrency,
                exchangeRate: action.newExchangeRate,
                details: state.details.map(detail =>
                    recalculateDetailFields(detail, action.newExchangeRate)
                )
            };
        case 'detailCalculationFieldChange':
            return {
                ...state,
                details: [
                    ...state.details.filter(x => x.lineNumber !== action.lineNumber),
                    recalculateDetailFields(action.payload, state.exchangeRate)
                ]
            };
        case 'dateRequestedChange':
            return {
                ...state,
                details: state.details.map(d =>
                    d.line === action.payload.line
                        ? {
                              ...d,
                              purchaseDeliveries: [{ dateRequested: action.payload.newValue }]
                          }
                        : d
                )
            };
        case 'nominalCodeChange':
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
                                nominalAccount: {
                                    ...detail.orderPosting.nominalAccount,
                                    nominal: {
                                        nominalCode: action.payload.id,
                                        description: action.payload.description
                                    }
                                }
                            }
                        };
                    })
                ]
            };
        case 'departmentCodeChange':
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
                                nominalAccount: {
                                    ...detail.orderPosting.nominalAccount,
                                    department: {
                                        departmentCode: action.payload.id,
                                        departmentDescription: action.payload.description
                                    }
                                }
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
        case 'orderTypeChange':
            return {
                ...state,
                documentType: { name: action.payload },
                details: [
                    ...state.details.map(detail => ({
                        ...detail,
                        originalOrderNumber: null,
                        originalOrderLine: action.payload === 'PO' ? null : 1
                    }))
                ]
            };
        default:
            return state;
    }
}
