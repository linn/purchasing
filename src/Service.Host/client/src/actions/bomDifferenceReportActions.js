import { ReportActions } from '@linn-it/linn-form-components-library';
import { bomDifferenceReportActionTypes as actionTypes } from './index';
import * as reportTypes from '../reportTypes';
import config from '../config';

export default new ReportActions(
    reportTypes.bomDifferenceReport.item,
    reportTypes.bomDifferenceReport.actionType,
    reportTypes.bomDifferenceReport.uri,
    actionTypes,
    config.appRoot
);
