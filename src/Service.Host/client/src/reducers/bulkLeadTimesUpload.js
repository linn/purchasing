import { processStoreFactory } from '@linn-it/linn-form-components-library';
import { bulkLeadTimesUploadActionTypes as actionTypes } from '../actions';
import { bulkLeadTimesUpload } from '../itemTypes';

const defaultState = { working: false, messageText: '', messageVisible: false };

export default processStoreFactory(
    bulkLeadTimesUpload.actionType,
    actionTypes,
    defaultState,
    'FILE UPLOADED'
);
