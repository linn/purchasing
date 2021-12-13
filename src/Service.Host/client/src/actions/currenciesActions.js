import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { currenciesActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.currencies.item,
    itemTypes.currencies.actionType,
    itemTypes.currencies.uri,
    actionTypes,
    config.proxyRoot,
    null,
    false
);
