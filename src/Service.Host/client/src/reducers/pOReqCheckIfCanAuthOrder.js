import { processStoreFactory } from '@linn-it/linn-form-components-library';
import { pOReqCheckIfCanAuthOrderActionTypes as actionTypes } from '../actions';
import { pOReqCheckIfCanAuthOrder } from '../itemTypes';

const defaultState = { working: false, messageText: '', messageVisible: false };

export default processStoreFactory(
    pOReqCheckIfCanAuthOrder.actionType,
    actionTypes,
    defaultState,
    'CHECK REQUESTED'
);
