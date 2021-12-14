import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { manufacturersActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.manufacturers.item,
    itemTypes.manufacturers.actionType,
    itemTypes.manufacturers.uri,
    actionTypes,
    config.appRoot
);
