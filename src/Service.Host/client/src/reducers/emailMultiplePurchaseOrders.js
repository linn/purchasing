import { processStoreFactory } from '@linn-it/linn-form-components-library';
import { emailMultiplePurchaseOrdersActionTypes as actionTypes } from '../actions';
import { emailMultiplePurchaseOrders } from '../processTypes';

const defaultState = { working: false, messageText: '', messageVisible: false };

export default processStoreFactory(
    emailMultiplePurchaseOrders.actionType,
    actionTypes,
    defaultState,
    'Orders emailed successfully'
);
