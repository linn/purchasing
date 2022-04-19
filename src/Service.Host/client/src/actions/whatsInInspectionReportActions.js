import { ReportActions } from '@linn-it/linn-form-components-library';
import { whatsInInspectionReportActionTypes as actionTypes } from './index';
import * as reportTypes from '../reportTypes';
import config from '../config';

export default new ReportActions(
    reportTypes.whatsInInspectionReport.item,
    reportTypes.whatsInInspectionReport.actionType,
    reportTypes.whatsInInspectionReport.uri,
    actionTypes,
    config.appRoot
);
