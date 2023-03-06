import { ReportActions } from '@linn-it/linn-form-components-library';
import { outstandingChangesReportActionTypes as actionTypes } from './index';
import * as reportTypes from '../reportTypes';
import config from '../config';

export default new ReportActions(
    reportTypes.outstandingChangesReport.item,
    reportTypes.outstandingChangesReport.actionType,
    reportTypes.outstandingChangesReport.uri,
    actionTypes,
    config.appRoot
);
