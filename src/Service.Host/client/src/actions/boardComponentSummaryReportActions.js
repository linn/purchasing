import { ReportActions } from '@linn-it/linn-form-components-library';
import { boardComponentSummaryReportActionTypes as actionTypes } from './index';
import * as reportTypes from '../reportTypes';
import config from '../config';

export default new ReportActions(
    reportTypes.boardComponentSummaryReport.item,
    reportTypes.boardComponentSummaryReport.actionType,
    reportTypes.boardComponentSummaryReport.uri,
    actionTypes,
    config.appRoot
);
