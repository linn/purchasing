import { ItemType } from '@linn-it/linn-form-components-library';

export const authoriseMultiplePurchaseOrders = new ItemType(
    'authoriseMultiplePurchaseOrders',
    'AUTHORISE_MULTIPLE_PURCHASE_ORDERS',
    '/purchasing/purchase-orders/authorise-multiple'
);

export const emailMultiplePurchaseOrders = new ItemType(
    'emailMultiplePurchaseOrders',
    'EMAIL_MULTIPLE_PURCHASE_ORDERS',
    '/purchasing/purchase-orders/email-multiple'
);

export const copyBom = new ItemType('copyBom', 'COPY_BOM', '/purchasing/boms/copy');
