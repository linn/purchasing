import { ReportActions } from '@linn-it/linn-form-components-library';
import { suppliersWithUnacknowledgedOrdersActionTypes as actionTypes } from './index';
import * as reportTypes from '../reportTypes';
import config from '../config';

export default new ReportActions(
    reportTypes.suppliersWithUnacknowledgedOrders.item,
    reportTypes.suppliersWithUnacknowledgedOrders.actionType,
    reportTypes.suppliersWithUnacknowledgedOrders.uri,
    actionTypes,
    config.appRoot
);
