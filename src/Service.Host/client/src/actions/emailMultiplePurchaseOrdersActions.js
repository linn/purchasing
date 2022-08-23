import { ProcessActions } from '@linn-it/linn-form-components-library';
import { emailMultiplePurchaseOrders } from '../processTypes';
import { emailMultiplePurchaseOrdersActionTypes as actionTypes } from './index';

import config from '../config';

export default new ProcessActions(
    emailMultiplePurchaseOrders.item,
    emailMultiplePurchaseOrders.actionType,
    emailMultiplePurchaseOrders.uri,
    actionTypes,
    config.appRoot,
    'application/json'
);
