import { processStoreFactory } from '@linn-it/linn-form-components-library';
import { sendPurchaseOrderAuthEmailActionTypes as actionTypes } from '../actions';
import { sendPurchaseOrderAuthEmail } from '../itemTypes';

const defaultState = { working: false, messageText: '', messageVisible: false };

export default processStoreFactory(
    sendPurchaseOrderAuthEmail.actionType,
    actionTypes,
    defaultState,
    'ORDER AUTH EMAIL REQUESTED'
);
