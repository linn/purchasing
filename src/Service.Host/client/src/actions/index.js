import { makeActionTypes } from '@linn-it/linn-form-components-library';
import * as itemTypes from '../itemTypes';

export const signingLimitActionTypes = makeActionTypes(itemTypes.signingLimit.actionType);
export const signingLimitsActionTypes = makeActionTypes(itemTypes.signingLimits.actionType, false);

export const testAction = () => ({
    type: 'TEST_ACTION',
    payload: {}
});

export const employeesActionTypes = makeActionTypes(itemTypes.employees.actionType);
