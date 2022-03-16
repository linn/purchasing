import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { purchaseOrdersActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.purchaseOrders.item,
    itemTypes.purchaseOrders.actionType,
    itemTypes.purchaseOrders.uri,
    actionTypes,
    config.appRoot
);
