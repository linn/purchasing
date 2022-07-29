import { Decimal } from 'decimal.js';

const currencyConvert = (value, exchangeRate) => {
    const convertedValue = new Decimal(value)
        .div(exchangeRate)
        .toDecimalPlaces(2, Decimal.ROUND_HALF_UP)
        .valueOf();

    return convertedValue;
};

export default currencyConvert;
