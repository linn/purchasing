import { StateApiActions } from '@linn-it/linn-form-components-library';
import { purchaseOrderReqActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new StateApiActions(
    itemTypes.purchaseOrderReq.item,
    itemTypes.purchaseOrderReq.actionType,
    itemTypes.purchaseOrderReq.uri,
    actionTypes,
    config.appRoot,
    'application-state'
);
