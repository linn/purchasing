import { ReportActions } from '@linn-it/linn-form-components-library';
import { boardDifferenceReportActionTypes as actionTypes } from './index';
import * as reportTypes from '../reportTypes';
import config from '../config';

export default new ReportActions(
    reportTypes.boardDifferenceReport.item,
    reportTypes.boardDifferenceReport.actionType,
    reportTypes.boardDifferenceReport.uri,
    actionTypes,
    config.appRoot
);
