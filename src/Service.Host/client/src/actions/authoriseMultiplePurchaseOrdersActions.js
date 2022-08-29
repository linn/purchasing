import { ProcessActions } from '@linn-it/linn-form-components-library';
import { authoriseMultiplePurchaseOrders } from '../processTypes';
import { authoriseMultiplePurchaseOrdersActionTypes as actionTypes } from './index';

import config from '../config';

export default new ProcessActions(
    authoriseMultiplePurchaseOrders.item,
    authoriseMultiplePurchaseOrders.actionType,
    authoriseMultiplePurchaseOrders.uri,
    actionTypes,
    config.appRoot,
    'application/json'
);
