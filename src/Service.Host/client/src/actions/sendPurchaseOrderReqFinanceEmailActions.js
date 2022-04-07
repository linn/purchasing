import { ProcessActions } from '@linn-it/linn-form-components-library';
import { sendPurchaseOrderReqFinanceEmail } from '../itemTypes';
import { sendPurchaseOrderReqFinanceEmailActionTypes as actionTypes } from './index';

import config from '../config';

export default new ProcessActions(
    sendPurchaseOrderReqFinanceEmail.item,
    sendPurchaseOrderReqFinanceEmail.actionType,
    sendPurchaseOrderReqFinanceEmail.uri,
    actionTypes,
    config.appRoot
);
