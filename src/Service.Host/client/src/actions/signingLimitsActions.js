import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { signingLimitsActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.signingLimits.item,
    itemTypes.signingLimits.actionType,
    itemTypes.signingLimits.uri,
    actionTypes,
    config.appRoot
);
