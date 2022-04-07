import { processStoreFactory } from '@linn-it/linn-form-components-library';
import { sendPurchaseOrderReqAuthEmailActionTypes as actionTypes } from '../actions';
import { sendPurchaseOrderReqAuthEmail } from '../itemTypes';

const defaultState = { working: false, messageText: '', messageVisible: false };

export default processStoreFactory(
    sendPurchaseOrderReqAuthEmail.actionType,
    actionTypes,
    defaultState,
    'REQ AUTH EMAIL REQUESTED'
);
