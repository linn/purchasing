import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { plannersActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.planners.item,
    itemTypes.planners.actionType,
    itemTypes.planners.uri,
    actionTypes,
    config.appRoot
);
