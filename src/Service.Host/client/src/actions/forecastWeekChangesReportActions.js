import { ReportActions } from '@linn-it/linn-form-components-library';
import { forecastWeekChangesReportActionTypes as actionTypes } from './index';
import * as reportTypes from '../reportTypes';
import config from '../config';

export default new ReportActions(
    reportTypes.forecastWeekChangesReport.item,
    reportTypes.forecastWeekChangesReport.actionType,
    reportTypes.forecastWeekChangesReport.uri,
    actionTypes,
    config.appRoot
);
