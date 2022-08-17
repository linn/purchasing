import { ReportActions } from '@linn-it/linn-form-components-library';
import { deliveryPerformanceDetailReportActionTypes as actionTypes } from './index';
import * as reportTypes from '../reportTypes';
import config from '../config';

export default new ReportActions(
    reportTypes.deliveryPerformanceDetailReport.item,
    reportTypes.deliveryPerformanceDetailReport.actionType,
    reportTypes.deliveryPerformanceDetailReport.uri,
    actionTypes,
    config.appRoot
);
