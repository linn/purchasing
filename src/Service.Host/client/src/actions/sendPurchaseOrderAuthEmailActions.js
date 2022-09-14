import { ProcessActions } from '@linn-it/linn-form-components-library';
import { sendPurchaseOrderAuthEmail } from '../itemTypes';
import { sendPurchaseOrderReqAuthEmailActionTypes as actionTypes } from './index';

import config from '../config';

export default new ProcessActions(
    sendPurchaseOrderAuthEmail.item,
    sendPurchaseOrderAuthEmail.actionType,
    sendPurchaseOrderAuthEmail.uri,
    actionTypes,
    config.appRoot
);
