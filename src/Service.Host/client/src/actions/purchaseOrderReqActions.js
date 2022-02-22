import { UpdateApiActions } from '@linn-it/linn-form-components-library';
import { purchaseOrderReqActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new UpdateApiActions(
    itemTypes.purchaseOrderReq.item,
    itemTypes.purchaseOrderReq.actionType,
    itemTypes.purchaseOrderReq.uri,
    actionTypes,
    config.appRoot
);
