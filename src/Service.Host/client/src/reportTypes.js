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

export const unacknowledgedOrdersReport = new ItemType(
    'unacknowledgedOrdersReport',
    'UNACKNOWLEDGED_ORDERS_REPORT',
    '/purchasing/reports/unacknowledged-orders'
);

export const spendByPartReport = new ItemType(
    'spendByPartReport',
    'SPEND_BY_PART',
    '/purchasing/reports/spend-by-part/report'
);

export const partsReceivedReport = new ItemType(
    'partsReceivedReport',
    'PARTS_RECEIVED_REPORT',
    '/purchasing/reports/parts-received'
);

export const whatsDueInReport = new ItemType(
    'whatsDueInReport',
    'WHATS_DUE_IN_REPORT',
    '/purchasing/reports/whats-due-in'
);

export const outstandingPoReqsReport = new ItemType(
    'outstandingPoReqsReport',
    'OUTSTANDING_PO_REQS_REPORT',
    '/purchasing/reports/outstanding-po-reqs/report'
);

export const prefSupReceiptsReport = new ItemType(
    'prefSupReceiptsReport',
    'PREF_SUP_RECEIPTS_REPORT',
    '/purchasing/reports/pref-sup-receipts/report'
);

export const whatsInInspectionReport = new ItemType(
    'whatsInInspectionReport',
    'WHATS_IN_INSPECTION_REPORT',
    '/purchasing/reports/whats-in-inspection/report'
);
