import { UpdateApiActions } from '@linn-it/linn-form-components-library';
import { partDataSheetValuesActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new UpdateApiActions(
    itemTypes.partDataSheetValues.item,
    itemTypes.partDataSheetValues.actionType,
    itemTypes.partDataSheetValues.uri,
    actionTypes,
    config.appRoot
);
