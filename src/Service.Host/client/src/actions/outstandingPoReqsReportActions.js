import { ReportActions } from '@linn-it/linn-form-components-library';
import { outstandingPoReqsReportActionTypes as actionTypes } from './index';
import * as reportTypes from '../reportTypes';
import config from '../config';

export default new ReportActions(
    reportTypes.outstandingPoReqsReport.item,
    reportTypes.outstandingPoReqsReport.actionType,
    reportTypes.outstandingPoReqsReport.uri,
    actionTypes,
    config.appRoot
);
