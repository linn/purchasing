import { ReportActions } from '@linn-it/linn-form-components-library';
import { spendByPartByDateReportActionTypes as actionTypes } from './index';
import * as reportTypes from '../reportTypes';
import config from '../config';

export default new ReportActions(
    reportTypes.spendByPartByDateReport.item,
    reportTypes.spendByPartByDateReport.actionType,
    reportTypes.spendByPartByDateReport.uri,
    actionTypes,
    config.appRoot
);
