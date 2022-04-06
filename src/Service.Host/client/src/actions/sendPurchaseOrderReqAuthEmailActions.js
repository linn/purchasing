import { ProcessActions } from '@linn-it/linn-form-components-library';
import { sendPurchaseOrderReqAuthEmail } from '../itemTypes';
import { sendPurchaseOrderReqAuthEmailActionTypes as actionTypes } from './index';

import config from '../config';

export default new ProcessActions(
    sendPurchaseOrderReqAuthEmail.item,
    sendPurchaseOrderReqAuthEmail.actionType,
    sendPurchaseOrderReqAuthEmail.uri,
    actionTypes,
    config.appRoot
);
