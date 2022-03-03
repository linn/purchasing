import { makeActionTypes, makeReportActionTypes } from '@linn-it/linn-form-components-library';
import * as itemTypes from '../itemTypes';
import * as reportTypes from '../reportTypes';

export const signingLimitActionTypes = makeActionTypes(
    itemTypes.signingLimit.actionType,
    true,
    true
);
export const signingLimitsActionTypes = makeActionTypes(itemTypes.signingLimits.actionType, false);

export const partSuppliersActionTypes = makeActionTypes(itemTypes.partSuppliers.actionType, false);
export const partSupplierActionTypes = makeActionTypes(itemTypes.partSupplier.actionType);

export const employeesActionTypes = makeActionTypes(itemTypes.employees.actionType);

export const supplierActionTypes = makeActionTypes(itemTypes.supplier.actionType);

export const suppliersActionTypes = makeActionTypes(itemTypes.suppliers.actionType);

export const partsActionTypes = makeActionTypes(itemTypes.parts.actionType);

export const partActionTypes = makeActionTypes(itemTypes.part.actionType);

export const currenciesActionTypes = makeActionTypes(itemTypes.currencies.actionType);

export const orderMethodsActionTypes = makeActionTypes(itemTypes.orderMethods.actionType);

export const deliveryAddressesActionTypes = makeActionTypes(itemTypes.deliveryAddresses.actionType);

export const unitsOfMeasureActionTypes = makeActionTypes(itemTypes.unitsOfMeasure.actionType);

export const tariffsActionTypes = makeActionTypes(itemTypes.tariffs.actionType);

export const packagingGroupsActionTypes = makeActionTypes(itemTypes.packagingGroups.actionType);

export const manufacturersActionTypes = makeActionTypes(itemTypes.manufacturers.actionType);

export const ordersBySupplierReportActionTypes = makeActionTypes(
    reportTypes.ordersBySupplierReport.actionType,
    false
);

export const preferredSupplierChangeActionTypes = makeActionTypes(
    itemTypes.preferredSupplierChange.actionType
);

export const priceChangeReasonsActionTypes = makeActionTypes(
    itemTypes.priceChangeReasons.actionType,
    false
);

export const partPriceConversionsActionTypes = makeActionTypes(
    itemTypes.partPriceConversions.actionType,
    false
);

export const ordersByPartReportActionTypes = makeActionTypes(
    reportTypes.ordersByPartReport.actionType,
    false
);

export const partCategoriesActionTypes = makeActionTypes(
    itemTypes.partCategories.actionType,
    false
);

export const accountingCompaniesActionTypes = makeActionTypes(
    itemTypes.accountingCompanies.actionType,
    false
);

export const putSupplierOnHoldActionTypes = makeActionTypes(
    itemTypes.putSupplierOnHold.actionType,
    true
);

export const addressActionTypes = makeActionTypes(itemTypes.address.actionType, true);

export const addressesActionTypes = makeActionTypes(itemTypes.addresses.actionType, false);

export const countriesActionTypes = makeActionTypes(itemTypes.countries.actionType, false);

export const vendorManagersActionTypes = makeActionTypes(
    itemTypes.vendorManagers.actionType,
    false
);

export const spendBySupplierReportActionTypes = makeActionTypes(
    reportTypes.spendBySupplierReport.actionType,
    false
);

export const suppliersWithUnacknowledgedOrdersActionTypes = makeReportActionTypes(
    reportTypes.suppliersWithUnacknowledgedOrders.actionType
);

export const unacknowledgedOrdersReportActionTypes = makeReportActionTypes(
    reportTypes.unacknowledgedOrdersReport.actionType
);

export const plannersActionTypes = makeActionTypes(itemTypes.planners.actionType);

export const spendByPartReportActionTypes = makeActionTypes(
    reportTypes.spendByPartReport.actionType,
    false
);

export const plCreditDebitNoteActionTypes = makeActionTypes(itemTypes.plCreditDebitNote.actionType);

export const plCreditDebitNotesActionTypes = makeActionTypes(
    itemTypes.plCreditDebitNotes.actionType
);

export const openDebitNotesActionTypes = makeActionTypes(itemTypes.openDebitNotes.actionType);
