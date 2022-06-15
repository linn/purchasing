import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { ediOrdersActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.ediOrders.item,
    itemTypes.ediOrders.actionType,
    itemTypes.ediOrders.uri,
    actionTypes,
    config.appRoot
);
