import { ItemType } from '@linn-it/linn-form-components-library';

export const ordersBySupplierReport = new ItemType(
    'ordersBySupplierReport',
    'ORDERS_BY_SUPPLIER_REPORT',
    '/purchasing/reports/orders-by-supplier/report'
);

export const ordersByPartReport = new ItemType(
    'ordersByPartReport',
    'ORDERS_BY_PART_REPORT',
    '/purchasing/reports/orders-by-part/report'
);

export const spendBySupplierReport = new ItemType(
    'spendBySupplierReport',
    'SPEND_BY_SUPPLIER',
    '/purchasing/reports/spend-by-supplier/report'
);

export const suppliersWithUnacknowledgedOrders = new ItemType(
    'suppliersWithUnacknowledgedOrders',
    'SUPPLIERS_WITH_UNACKNOWLEDGED_ORDERS',
    '/purchasing/reports/suppliers-with-unacknowledged-orders'
);
