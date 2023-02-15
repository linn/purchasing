import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { partDataSheetValuesListActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.partDataSheetValuesList.item,
    itemTypes.partDataSheetValuesList.actionType,
    itemTypes.partDataSheetValuesList.uri,
    actionTypes,
    config.appRoot
);
