import { ReportActions } from '@linn-it/linn-form-components-library';
import { ordersByPartReportActionTypes as actionTypes } from './index';
import * as reportTypes from '../reportTypes';
import config from '../config';

export default new ReportActions(
    reportTypes.ordersByPartReport.item,
    reportTypes.ordersByPartReport.actionType,
    reportTypes.ordersByPartReport.uri,
    actionTypes,
    config.appRoot
);
