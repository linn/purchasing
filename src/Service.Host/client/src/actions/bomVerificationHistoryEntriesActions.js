import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { bomVerificationHistoryEntriesActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.bomVerificationHistoryEntries.item,
    itemTypes.bomVerificationHistoryEntries.actionType,
    itemTypes.bomVerificationHistoryEntries.uri,
    actionTypes,
    config.appRoot
);
