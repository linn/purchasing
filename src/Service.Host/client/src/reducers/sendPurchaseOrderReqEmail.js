import { processStoreFactory } from '@linn-it/linn-form-components-library';
import { sendPurchaseOrderReqEmailActionTypes as actionTypes } from '../actions';
import { sendPurchaseOrderReqEmail } from '../itemTypes';

const defaultState = { working: false, messageText: '', messageVisible: false };

export default processStoreFactory(
    sendPurchaseOrderReqEmail.actionType,
    actionTypes,
    defaultState,
    'EMAIL REQUESTED'
);
