import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { priceChangeReasonsActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.priceChangeReasons.item,
    itemTypes.priceChangeReasons.actionType,
    itemTypes.priceChangeReasons.uri,
    actionTypes,
    config.appRoot
);
