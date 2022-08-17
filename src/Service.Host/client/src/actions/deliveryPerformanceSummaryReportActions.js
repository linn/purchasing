import { ReportActions } from '@linn-it/linn-form-components-library';
import { deliveryPerformanceSummaryReportActionTypes as actionTypes } from './index';
import * as reportTypes from '../reportTypes';
import config from '../config';

export default new ReportActions(
    reportTypes.deliveryPerformanceSummaryReport.item,
    reportTypes.deliveryPerformanceSummaryReport.actionType,
    reportTypes.deliveryPerformanceSummaryReport.uri,
    actionTypes,
    config.appRoot
);
