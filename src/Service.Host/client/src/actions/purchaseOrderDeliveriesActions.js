import { UpdateApiActions } from '@linn-it/linn-form-components-library';
import { purchaseOrderDeliveriesActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new UpdateApiActions(
    itemTypes.purchaseOrderDeliveries.item,
    itemTypes.purchaseOrderDeliveries.actionType,
    itemTypes.purchaseOrderDeliveries.uri,
    actionTypes,
    config.appRoot
);
