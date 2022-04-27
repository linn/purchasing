import { processStoreFactory } from '@linn-it/linn-form-components-library';
import { sendPurchaseOrderReqFinanceEmailActionTypes as actionTypes } from '../actions';
import { sendPurchaseOrderReqFinanceEmail } from '../itemTypes';

const defaultState = { working: false, messageText: '', messageVisible: false };

export default processStoreFactory(
    sendPurchaseOrderReqFinanceEmail.actionType,
    actionTypes,
    defaultState
);
