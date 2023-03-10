import { ReportActions } from '@linn-it/linn-form-components-library';
import { changeStatusReportActionTypes as actionTypes } from './index';
import * as reportTypes from '../reportTypes';
import config from '../config';

export default new ReportActions(
    reportTypes.changeStatusReport.item,
    reportTypes.changeStatusReport.actionType,
    reportTypes.changeStatusReport.uri,
    actionTypes,
    config.appRoot
);
