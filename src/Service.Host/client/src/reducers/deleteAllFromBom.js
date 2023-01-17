import { processStoreFactory } from '@linn-it/linn-form-components-library';
import { deleteAllFromBomActionTypes as actionTypes } from '../actions';
import { deleteAllFromBom } from '../processTypes';

const defaultState = { working: false, messageText: '', messageVisible: false };

export default processStoreFactory(
    deleteAllFromBom.actionType,
    actionTypes,
    defaultState,
    'Deleted!'
);
