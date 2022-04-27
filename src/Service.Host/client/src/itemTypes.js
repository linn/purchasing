import { ItemType } from '@linn-it/linn-form-components-library';

export const signingLimit = new ItemType(
    'signingLimit',
    'SIGNING_LIMIT',
    '/purchasing/signing-limits'
);
export const signingLimits = new ItemType(
    'signingLimits',
    'SIGNING_LIMITS',
    '/purchasing/signing-limits'
);

export const partSuppliers = new ItemType(
    'partSuppliers',
    'PART_SUPPLIERS',
    '/purchasing/part-suppliers'
);

export const partSupplier = new ItemType(
    'partSupplier',
    'PART_SUPPLIER',
    '/purchasing/part-suppliers/record'
);

export const supplier = new ItemType('supplier', 'SUPPLIER', '/purchasing/suppliers');

export const suppliers = new ItemType('suppliers', 'SUPPLIERS', '/purchasing/suppliers');

export const employees = new ItemType('employees', 'EMPLOYEES', '/inventory/employees');

export const parts = new ItemType('parts', 'PARTS', '/parts');

export const part = new ItemType('part', 'PART', '/parts/manufacturer-data');

export const purchaseOrder = new ItemType(
    'purchaseOrder',
    'PURCHASE_ORDER',
    '/purchasing/purchase-orders'
);

export const purchaseOrders = new ItemType(
    'purchaseOrders',
    'PURCHASE_ORDERS',
    '/purchasing/purchase-orders'
);

export const currencies = new ItemType(
    'currencies',
    'CURRENCIES',
    '/purchasing/purchase-orders/currencies'
);

export const orderMethods = new ItemType(
    'orderMethods',
    'ORDER_METHODS',
    '/purchasing/purchase-orders/methods'
);

export const deliveryAddresses = new ItemType(
    'deliveryAddresses',
    'DELIVERY_ADDRESSES',
    '/purchasing/purchase-orders/delivery-addresses'
);

export const unitsOfMeasure = new ItemType(
    'unitsOfMeasure',
    'UNITS_OF_MEASURE',
    '/purchasing/purchase-orders/units-of-measure'
);

export const tariffs = new ItemType('tariffs', 'TARIFFS', '/purchasing/purchase-orders/tariffs');

export const packagingGroups = new ItemType(
    'packagingGroups',
    'PACKAGING_GROUPS',
    '/purchasing/purchase-orders/packaging-groups'
);

export const manufacturers = new ItemType(
    'manufacturers',
    'MANUFACTURERS',
    '/purchasing/manufacturers'
);

export const preferredSupplierChange = new ItemType(
    'preferredSupplierChange',
    'PREFERRED_SUPPLIER_CHANGE',
    '/purchasing/preferred-supplier-changes'
);

export const priceChangeReasons = new ItemType(
    'priceChangeReasons',
    'PRICE_CHANGE_REASONS',
    '/purchasing/price-change-reasons'
);

export const partPriceConversions = new ItemType(
    'partPriceConversions',
    'PART_PRICE_CONVERSIONS',
    '/purchasing/part-suppliers/part-price-conversions'
);

export const partCategories = new ItemType(
    'partCategories',
    'PART_CATEGORIES',
    '/purchasing/part-categories'
);

export const accountingCompanies = new ItemType(
    'accountingCompanies',
    'ACCOUNTING_COMPANIES',
    '/inventory/accounting-companies'
);

export const putSupplierOnHold = new ItemType(
    'putSupplierOnHold',
    'PUT_SUPPLIER_ON_HOLD',
    '/purchasing/suppliers/hold'
);

export const address = new ItemType('address', 'ADDRESS', '/purchasing/addresses');

export const addresses = new ItemType('address', 'ADDRESSES', '/purchasing/addresses');

export const countries = new ItemType('countries', 'COUNTRIES', '/purchasing/countries');

export const vendorManagers = new ItemType(
    'vendorManagers',
    'VENDOR_MANAGERS',
    '/purchasing/vendor-managers'
);

export const planners = new ItemType('planners', 'PLANNERS', '/purchasing/suppliers/planners');

export const contacts = new ItemType('contacts', 'CONTACTS', '/purchasing/suppliers/contacts');

export const plCreditDebitNote = new ItemType(
    'plCreditDebitNote',
    'PL_CREDIT_DEBIT_NOTE',
    '/purchasing/pl-credit-debit-notes'
);

export const plCreditDebitNotes = new ItemType(
    'plCreditDebitNotes',
    'PL_CREDIT_DEBIT_NOTES',
    '/purchasing/pl-credit-debit-notes'
);

export const openDebitNotes = new ItemType(
    'plCreditDebitNotes',
    'PL_CREDIT_DEBIT_NOTES',
    '/purchasing/open-debit-notes'
);

export const sendPlNoteEmail = new ItemType(
    'sendPlNoteEmail',
    'SEND_PL_NOTE_EMAIL',
    '/purchasing/pl-credit-debit-notes/email/'
);

export const sendPurchaseOrderReqEmail = new ItemType(
    'sendPurchaseOrderReqEmail',
    'SEND_PURCHASE_ORDER_REQ_EMAIL',
    '/purchasing/purchase-orders/reqs/email'
);

export const bulkLeadTimesUpload = new ItemType(
    'bulkLeadTimesUpload',
    'BULK_LEAD_TIMES_UPLOAD',
    '/purchasing/suppliers/bulk-lead-times/'
);

export const supplierGroups = new ItemType(
    'supplierGroups',
    'SUPPLIER_GROUPS',
    '/purchasing/supplier-groups/'
);

export const tqmsJobrefs = new ItemType('tqmsJobrefs', 'TQMS_JOBREFS', '/purchasing/tqms-jobrefs/');
export const purchaseOrderReq = new ItemType(
    'purchaseOrderReq',
    'PURCHASE_ORDER_REQ',
    '/purchasing/purchase-orders/reqs'
);

export const departments = new ItemType('departments', 'DEPARTMENTS', '/inventory/departments');

export const nominals = new ItemType('nominal', 'NOMINAL', '/inventory/nominal-accounts');

export const purchaseOrderReqs = new ItemType(
    'purchaseOrderReqs',
    'PURCHASE_ORDER_REQS',
    '/purchasing/purchase-orders/reqs'
);

export const purchaseOrderReqStates = new ItemType(
    'purchaseOrderReqStates',
    'PURCHASE_ORDER_REQ_STATES',
    '/purchasing/purchase-orders/reqs/states'
);

export const sendPurchaseOrderReqAuthEmail = new ItemType(
    'sendPurchaseOrderReqAuthEmail',
    'SEND_PURCHASE_ORDER_REQ_AUTH_EMAIL',
    '/purchasing/purchase-orders/reqs/email-for-authorisation'
);

export const sendPurchaseOrderReqFinanceEmail = new ItemType(
    'sendPurchaseOrderReqFinanceEmail',
    'SEND_PURCHASE_ORDER_REQ_FINANCE_EMAIL',
    '/purchasing/purchase-orders/reqs/email-for-finance'
);

export const mrpRunLog = new ItemType(
    'mrpRunLog',
    'MRP_RUN_LOG',
    '/purchasing/material-requirements/run-logs'
);

export const mrpRunLogs = new ItemType(
    'mrpRunLogs',
    'MRP_RUN_LOGS',
    '/purchasing/material-requirements/run-logs'
);

export const runMrp = new ItemType(
    'runMrp',
    'RUN_MRP',
    '/purchasing/material-requirements/run-mrp'
);

export const mrMaster = new ItemType(
    'mrMaster',
    'MR_MASTER',
    '/purchasing/material-requirements/last-run'
);

export const applyForecastingPercentageChange = new ItemType(
    'applyForecastingPercentageChange',
    'APPLY_FORECASTING_PERCENTAGE_CHANGE',
    '/purchasing/forecasting/apply-percentage-change'
);
