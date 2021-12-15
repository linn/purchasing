import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { unitsOfMeasureActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.unitsOfMeasure.item,
    itemTypes.unitsOfMeasure.actionType,
    itemTypes.unitsOfMeasure.uri,
    actionTypes,
    config.appRoot,
    null,
    false
);
