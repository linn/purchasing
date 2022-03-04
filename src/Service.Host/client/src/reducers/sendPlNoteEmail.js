import { processStoreFactory } from '@linn-it/linn-form-components-library';
import { sendPlNoteEmailActionTypes as actionTypes } from '../actions';
import { sendPlNoteEmail } from '../itemTypes';

const defaultState = { working: false, messageText: '', messageVisible: false };

export default processStoreFactory(
    sendPlNoteEmail.actionType,
    actionTypes,
    defaultState,
    'EMAIL REQUESTED'
);
