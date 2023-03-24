import { processStoreFactory } from '@linn-it/linn-form-components-library';
import { uploadSmtBoardFileActionTypes as actionTypes } from '../actions';
import { uploadSmtBoardFile } from '../processTypes';

const defaultState = { working: false, messageText: '', messageVisible: false };

export default processStoreFactory(
    uploadSmtBoardFile.actionType,
    actionTypes,
    defaultState,
    'Uploaded'
);
