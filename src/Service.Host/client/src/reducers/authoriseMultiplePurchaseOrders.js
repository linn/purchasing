import { processStoreFactory } from '@linn-it/linn-form-components-library';
import { authoriseMultiplePurchaseOrdersActionTypes as actionTypes } from '../actions';
import { authoriseMultiplePurchaseOrders } from '../processTypes';

const defaultState = { working: false, messageText: '', messageVisible: false };

export default processStoreFactory(
    authoriseMultiplePurchaseOrders.actionType,
    actionTypes,
    defaultState,
    'Orders authorised successfully'
);
