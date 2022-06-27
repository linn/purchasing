import { processStoreFactory } from '@linn-it/linn-form-components-library';
import { sendPurchaseOrderPdfEmailActionTypes as actionTypes } from '../actions';
import { sendPurchaseOrderPdfEmail } from '../itemTypes';

const defaultState = { working: false, messageText: '', messageVisible: false };

export default processStoreFactory(
    sendPurchaseOrderPdfEmail.actionType,
    actionTypes,
    defaultState,
    'PDF EMAIL REQUESTED'
);
