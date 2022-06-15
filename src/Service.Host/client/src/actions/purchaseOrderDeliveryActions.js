import { UpdateApiActions } from '@linn-it/linn-form-components-library';
import { purchaseOrderDeliveryActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new UpdateApiActions(
    itemTypes.purchaseOrderDelivery.item,
    itemTypes.purchaseOrderDelivery.actionType,
    itemTypes.purchaseOrderDelivery.uri,
    actionTypes,
    config.appRoot
);
