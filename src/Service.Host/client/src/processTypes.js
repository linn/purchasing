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

export const uploadBoardFile = new ItemType(
    'uploadBoardFile',
    'UPDATE_BOARD_FILE',
    '/purchasing/purchase-orders/boms/upload-board-file'
);

export const uploadSmtBoardFile = new ItemType(
    'uploadSmtBoardFile',
    'UPDATE_SMT_BOARD_FILE',
    '/purchasing/purchase-orders/boms/upload-smt-file'
);
