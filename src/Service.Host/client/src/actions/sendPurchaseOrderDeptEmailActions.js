import { UpdateApiActions } from '@linn-it/linn-form-components-library';
import { sendPurchaseOrderDeptEmailActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new UpdateApiActions(
    itemTypes.sendPurchaseOrderDeptEmail.item,
    itemTypes.sendPurchaseOrderDeptEmail.actionType,
    itemTypes.sendPurchaseOrderDeptEmail.uri,
    actionTypes,
    config.appRoot
);
