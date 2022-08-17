import { ProcessActions } from '@linn-it/linn-form-components-library';
import { sendPurchaseOrderSupplierAssEmail } from '../itemTypes';
import { sendPurchaseOrderSupplierAssActionTypes as actionTypes } from './index';

import config from '../config';

export default new ProcessActions(
    sendPurchaseOrderSupplierAssEmail.item,
    sendPurchaseOrderSupplierAssEmail.actionType,
    sendPurchaseOrderSupplierAssEmail.uri,
    actionTypes,
    config.appRoot
);
