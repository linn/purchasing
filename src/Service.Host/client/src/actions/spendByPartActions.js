import { ReportActions } from '@linn-it/linn-form-components-library';
import { spendByPartReportActionTypes as actionTypes } from './index';
import * as reportTypes from '../reportTypes';
import config from '../config';

export default new ReportActions(
    reportTypes.spendByPartReport.item,
    reportTypes.spendByPartReport.actionType,
    reportTypes.spendByPartReport.uri,
    actionTypes,
    config.appRoot
);
