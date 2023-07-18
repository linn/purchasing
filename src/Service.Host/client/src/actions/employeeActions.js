import { UpdateApiActions } from '@linn-it/linn-form-components-library';
import { employeeActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new UpdateApiActions(
    itemTypes.employee.item,
    itemTypes.employee.actionType,
    itemTypes.employee.uri,
    actionTypes,
    config.appRoot
);
