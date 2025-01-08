import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { currentEmployeesActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.currentEmployees.item,
    itemTypes.currentEmployees.actionType,
    itemTypes.currentEmployees.uri,
    actionTypes,
    config.proxyRoot,
    null,
    false
);
