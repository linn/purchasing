import { UpdateApiActions } from '@linn-it/linn-form-components-library';
import { mrReportOrdersActionTypes as actionTypes } from './index';
import * as itemTypes from '../itemTypes';
import config from '../config';

export default new UpdateApiActions(
    itemTypes.mrReportOrders.item,
    itemTypes.mrReportOrders.actionType,
    itemTypes.mrReportOrders.uri,
    actionTypes,
    config.appRoot
);
