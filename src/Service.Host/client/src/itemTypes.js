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

export const suppliers = new ItemType('suppliers', 'SUPPLIERS', '/purchasing/suppliers');

export const employees = new ItemType('employees', 'EMPLOYEES', '/inventory/employees');

export const parts = new ItemType('parts', 'PARTS', '/parts');

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
