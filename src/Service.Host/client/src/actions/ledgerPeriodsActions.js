import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { ledgerPeriodsActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.ledgerPeriods.item,
    itemTypes.ledgerPeriods.actionType,
    itemTypes.ledgerPeriods.uri,
    actionTypes,
    config.appRoot,
    null,
    false
);
