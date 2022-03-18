import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { purchaseOrderReqsActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.purchaseOrderReqs.item,
    itemTypes.purchaseOrderReqs.actionType,
    itemTypes.purchaseOrderReqs.uri,
    actionTypes,
    config.appRoot
);
