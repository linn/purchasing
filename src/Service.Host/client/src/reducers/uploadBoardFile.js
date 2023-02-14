import { processStoreFactory } from '@linn-it/linn-form-components-library';
import { uploadBoardFileActionTypes as actionTypes } from '../actions';
import { uploadBoardFile } from '../processTypes';

const defaultState = { working: false, messageText: '', messageVisible: false };

export default processStoreFactory(
    uploadBoardFile.actionType,
    actionTypes,
    defaultState,
    'Uploaded'
);
