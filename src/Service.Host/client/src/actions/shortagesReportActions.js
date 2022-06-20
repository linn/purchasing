import { ReportActions } from '@linn-it/linn-form-components-library';
import { shortagesReportActionTypes as actionTypes } from './index';
import * as reportTypes from '../reportTypes';
import config from '../config';

export default new ReportActions(
    reportTypes.shortagesReport.item,
    reportTypes.shortagesReport.actionType,
    reportTypes.shortagesReport.uri,
    actionTypes,
    config.appRoot
);
