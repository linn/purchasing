import { ProcessActions } from '@linn-it/linn-form-components-library';
import { batchPurchaseOrderDeliveriesUpload } from '../itemTypes';
import { batchPurchaseOrderDeliveriesUploadActionTypes as actionTypes } from './index';

import config from '../config';

export default new ProcessActions(
    batchPurchaseOrderDeliveriesUpload.item,
    batchPurchaseOrderDeliveriesUpload.actionType,
    batchPurchaseOrderDeliveriesUpload.uri,
    actionTypes,
    config.appRoot,
    'text/csv'
);
