import { processStoreFactory } from '@linn-it/linn-form-components-library';
import { runMrpActionTypes as actionTypes } from '../actions';
import { runMrp } from '../itemTypes';

const defaultState = { working: false, messageText: '', messageVisible: false };

export default processStoreFactory(runMrp.actionType, actionTypes, defaultState, 'MRP RUN');
