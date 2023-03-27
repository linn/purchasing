import { ReportActions } from '@linn-it/linn-form-components-library';
import { currentPhaseInWeeksReportActionTypes as actionTypes } from './index';
import * as reportTypes from '../reportTypes';
import config from '../config';

export default new ReportActions(
    reportTypes.currentPhaseInWeeksReport.item,
    reportTypes.currentPhaseInWeeksReport.actionType,
    reportTypes.currentPhaseInWeeksReport.uri,
    actionTypes,
    config.appRoot
);
