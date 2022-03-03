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

export const purchaseOrderReq = new ItemType(
    'purchaseOrderReq',
    'PURCHASE_ORDER_REQ',
    '/purchasing/purchase-orders/reqs'
);
