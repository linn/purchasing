import { ProcessActions } from '@linn-it/linn-form-components-library';
import { sendPurchaseOrderReqEmail } from '../itemTypes';
import { sendPurchaseOrderReqEmailActionTypes as actionTypes } from './index';

import config from '../config';

export default new ProcessActions(
    sendPurchaseOrderReqEmail.item,
    sendPurchaseOrderReqEmail.actionType,
    sendPurchaseOrderReqEmail.uri,
    actionTypes,
    config.appRoot,
    'application/pdf'
);
