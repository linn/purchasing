import { ReportActions } from '@linn-it/linn-form-components-library';
import { unacknowledgedOrdersReportActionTypes as actionTypes } from './index';
import * as reportTypes from '../reportTypes';
import config from '../config';

export default new ReportActions(
    reportTypes.unacknowledgedOrdersReport.item,
    reportTypes.unacknowledgedOrdersReport.actionType,
    reportTypes.unacknowledgedOrdersReport.uri,
    actionTypes,
    config.appRoot
);
