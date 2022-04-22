import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { mrpRunLogsActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.mrpRunLogs.item,
    itemTypes.mrpRunLogs.actionType,
    itemTypes.mrpRunLogs.uri,
    actionTypes,
    config.appRoot
);
