import { FetchApiActions } from '@linn-it/linn-form-components-library';
import { orderMethodsActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new FetchApiActions(
    itemTypes.orderMethods.item,
    itemTypes.orderMethods.actionType,
    itemTypes.orderMethods.uri,
    actionTypes,
    config.appRoot
);
