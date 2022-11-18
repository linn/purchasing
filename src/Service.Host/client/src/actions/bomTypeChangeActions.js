import { UpdateApiActions } from '@linn-it/linn-form-components-library';
import { bomTypeChangeActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new UpdateApiActions(
    itemTypes.bomTypeChange.item,
    itemTypes.bomTypeChange.actionType,
    itemTypes.bomTypeChange.uri,
    actionTypes,
    config.appRoot
);
