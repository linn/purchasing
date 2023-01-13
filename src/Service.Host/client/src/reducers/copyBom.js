import { processStoreFactory } from '@linn-it/linn-form-components-library';
import { copyBomActionTypes as actionTypes } from '../actions';
import { copyBom } from '../processTypes';

const defaultState = { working: false, messageText: '', messageVisible: false };

export default processStoreFactory(copyBom.actionType, actionTypes, defaultState, 'Copied!');
