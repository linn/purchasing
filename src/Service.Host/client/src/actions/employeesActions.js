// import FetchApiActions from './FetchApiActions';
import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { employeesActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.employees.item,
    itemTypes.employees.actionType,
    itemTypes.employees.uri,
    actionTypes,
    config.proxyRoot,
    null,
    false
);
