import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { exchangeRatesActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.exchangeRates.item,
    itemTypes.exchangeRates.actionType,
    itemTypes.exchangeRates.uri,
    actionTypes,
    config.proxyRoot
);
