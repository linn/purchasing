import { ReportActions } from '@linn-it/linn-form-components-library';
import { bomPrintReportActionTypes as actionTypes } from './index';
import * as reportTypes from '../reportTypes';
import config from '../config';

export default new ReportActions(
    reportTypes.bomPrintReport.item,
    reportTypes.bomPrintReport.actionType,
    reportTypes.bomPrintReport.uri,
    actionTypes,
    config.appRoot
);
