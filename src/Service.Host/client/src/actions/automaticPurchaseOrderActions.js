import { UpdateApiActions } from '@linn-it/linn-form-components-library';
import { automaticPurchaseOrderActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new UpdateApiActions(
    itemTypes.automaticPurchaseOrder.item,
    itemTypes.automaticPurchaseOrder.actionType,
    itemTypes.automaticPurchaseOrder.uri,
    actionTypes,
    config.appRoot
);
