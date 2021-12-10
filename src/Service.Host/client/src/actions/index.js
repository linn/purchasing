import { makeActionTypes } from '@linn-it/linn-form-components-library';
import * as itemTypes from '../itemTypes';
import * as reportTypes from '../reportTypes';

export const signingLimitActionTypes = makeActionTypes(itemTypes.signingLimit.actionType);
export const signingLimitsActionTypes = makeActionTypes(itemTypes.signingLimits.actionType, false);

export const partSuppliersActionTypes = makeActionTypes(itemTypes.partSuppliers.actionType, false);
export const partSupplierActionTypes = makeActionTypes(itemTypes.partSupplier.actionType);

export const testAction = () => ({
    type: 'TEST_ACTION',
    payload: {}
});

export const employeesActionTypes = makeActionTypes(itemTypes.employees.actionType);

export const suppliersActionTypes = makeActionTypes(itemTypes.suppliers.actionType, false);

export const ordersBySupplierReportActionTypes = makeActionTypes(
    reportTypes.ordersBySupplierReport.actionType,
    false
);
