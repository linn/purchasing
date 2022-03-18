import { UpdateApiActions } from '@linn-it/linn-form-components-library';
import { purchaseOrderActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new UpdateApiActions(
    itemTypes.purchaseOrder.item,
    itemTypes.purchaseOrder.actionType,
    itemTypes.purchaseOrder.uri,
    actionTypes,
    config.appRoot
);
