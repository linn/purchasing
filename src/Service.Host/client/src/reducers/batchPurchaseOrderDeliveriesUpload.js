import { processStoreFactory } from '@linn-it/linn-form-components-library';
import { batchPurchaseOrderDeliveriesUploadActionTypes as actionTypes } from '../actions';
import { batchPurchaseOrderDeliveriesUpload } from '../itemTypes';

const defaultState = { working: false, messageText: '', messageVisible: false };

export default processStoreFactory(
    batchPurchaseOrderDeliveriesUpload.actionType,
    actionTypes,
    defaultState,
    'FILE UPLOADED'
);
