import { ProcessActions } from '@linn-it/linn-form-components-library';
import { sendPurchaseOrderPdfEmail } from '../itemTypes';
import { sendPurchaseOrderPdfEmailActionTypes as actionTypes } from './index';

import config from '../config';

export default new ProcessActions(
    sendPurchaseOrderPdfEmail.item,
    sendPurchaseOrderPdfEmail.actionType,
    sendPurchaseOrderPdfEmail.uri,
    actionTypes,
    config.appRoot
);
