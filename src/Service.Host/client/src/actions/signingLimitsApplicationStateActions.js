import { StateApiActions } from '@linn-it/linn-form-components-library';
import { signingLimitsActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new StateApiActions(
    itemTypes.signingLimits.item,
    itemTypes.signingLimits.actionType,
    itemTypes.signingLimits.uri,
    actionTypes,
    config.appRoot,
    'application-state'
);
