import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { purchaseOrderReqStatesActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.purchaseOrderReqStates.item,
    itemTypes.purchaseOrderReqStates.actionType,
    itemTypes.purchaseOrderReqStates.uri,
    actionTypes,
    config.appRoot,
    null,
    false
);
