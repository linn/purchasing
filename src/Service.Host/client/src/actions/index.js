import { makeActionTypes } from '@linn-it/linn-form-components-library';
import * as itemTypes from '../itemTypes';

export const signingLimitActionTypes = makeActionTypes(itemTypes.signingLimit.actionType);
export const signingLimitsActionTypes = makeActionTypes(itemTypes.signingLimits.actionType, false);

export const partSuppliersActionTypes = makeActionTypes(itemTypes.partSuppliers.actionType, false);
export const partSupplierActionTypes = makeActionTypes(itemTypes.partSupplier.actionType);

export const testAction = () => ({
    type: 'TEST_ACTION',
    payload: {}
});

export const employeesActionTypes = makeActionTypes(itemTypes.employees.actionType);

export const suppliersActionTypes = makeActionTypes(itemTypes.suppliers.actionType);

export const partsActionTypes = makeActionTypes(itemTypes.parts.actionType);

export const currenciesActionTypes = makeActionTypes(itemTypes.currencies.actionType);

export const orderMethodsActionTypes = makeActionTypes(itemTypes.orderMethods.actionType);

export const deliveryAddressesActionTypes = makeActionTypes(itemTypes.deliveryAddresses.actionType);

export const unitsOfMeasureActionTypes = makeActionTypes(itemTypes.unitsOfMeasure.actionType);
